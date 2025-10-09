import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import {
  fetchDepartments,
  addDepartment,
  removeDepartment,
} from "../Features/DepartmentSlice";
import "./Departments.css";
import AddDepartment from "./AddDepartment";

function DepartmentCard({ dept, onEdit, onDelete }) {
  return (
    <div className="department-card">
      <div className="department-icon-bg">
        <span className="department-icon">{dept.icon}</span>
      </div>
      <div className="department-details">
        <div className="department-title">{dept.name}</div>
        <div className="department-meta">
          <div>
            <span className="department-meta-icon">👥</span> Employees: {dept.employeeCount}
          </div>
        </div>
        <div className="department-actions">
          <button className="department-edit-btn" onClick={onEdit}>
            Edit
          </button>
          <button className="department-delete-btn" onClick={onDelete}>
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}

export default function Departments() {
  const dispatch = useDispatch();
  const { items: departments = [], loading, error } = useSelector(
    (state) => state.departments
  );
  const [showAdd, setShowAdd] = useState(false);
  const [addError, setAddError] = useState(null);

  useEffect(() => {
    dispatch(fetchDepartments())
      .unwrap()
      .then((data) => console.log("Fetched departments:", data))
      .catch((err) => console.error("Fetch departments failed:", err));
  }, [dispatch]);

  const handleDelete = (id) => {
    if (window.confirm("Delete this department?")) {
      dispatch(removeDepartment(id))
        .unwrap()
        .catch((err) => alert("Failed to delete department: " + err.message));
    }
  };

  const handleEdit = (dept) => {
    alert("Edit Department: " + dept.name);
    // Implement edit modal or form here if needed
  };

  const handleAdd = async (deptName) => {
  if (typeof deptName !== "string" || deptName.trim() === "") {
    setAddError("Department name must be a non-empty string.");
    return;
  }
  try {
    setAddError(null);
    await dispatch(addDepartment(deptName.trim())).unwrap();
    setShowAdd(false);
  } catch (err) {
    setAddError("Failed to add department: " + (err.message || err));
  }
};


  return (
    <div className="departments-container">
      <div className="departments-header-row">
        <button className="add-department-btn" onClick={() => setShowAdd(true)}>
          + Add Department
        </button>
      </div>

      {addError && <div className="error-msg">{addError}</div>}

      <div className="departments-list">
        {loading ? (
          <div>Loading...</div>
        ) : error ? (
          <div className="error-msg">{error}</div>
        ) : departments.length === 0 ? (
          <div className="no-departments-msg">No departments found.</div>
        ) : (
          departments.map((dept) =>
            dept ? (
              <DepartmentCard
                key={dept.DeptId || dept.name}
                dept={dept}
                onEdit={() => handleEdit(dept)}
                onDelete={() => handleDelete(dept.DeptId)}
              />
            ) : null
          )
        )}
      </div>

      {showAdd && (
        <AddDepartment
          onCancel={() => {
            setAddError(null);
            setShowAdd(false);
          }}
          onAdd={handleAdd}
        />
      )}
    </div>
  );
}
