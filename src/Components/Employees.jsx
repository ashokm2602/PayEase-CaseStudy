import React, { useState, useEffect } from "react";
import AddEmployee from "./AddEmployee";
import "./Employees.css";

function EmployeeCard({ emp, onDelete, getDepartmentName }) {
  return (
    <div className="employee-card">
      <div className="employee-avatar">
        {(emp.firstName || " ")[0].toUpperCase()}
      </div>

      <div className="employee-card-main">
        <div className="employee-header-row">
          <div>
            <span className="employee-name">
              {emp.firstName} {emp.lastName}
            </span>
            <span className="employee-id">#{emp.empId}</span>
          </div>
          <span className={`employee-status ${emp.status || "unknown"}`}>
            {emp.status || "Unknown"}
          </span>
        </div>

        <div className="employee-info">
          <div>
            <span className="employee-icon">&#9742;</span>
            <span>{emp.contactNumber || "No contact"}</span>
          </div>
          <div>
            <span className="employee-icon">&#127968;</span>
            <span>{emp.address || "No address"}</span>
          </div>
          <div>
            <span className="employee-icon">&#128188;</span>
            <span>{getDepartmentName(emp.deptId) || "Unknown Dept."}</span>
          </div>
          <div>
            <span className="employee-icon">&#36;</span>
            <span>{emp.baseSalary ? `$${emp.baseSalary}` : "Salary not set"}</span>
          </div>
          <div>
            <span className="employee-icon">&#128197;</span>
            <span>{emp.hireDate ? `Hired: ${new Date(emp.hireDate).toLocaleDateString()}` : ""}</span>
          </div>
          <div>
            <span>DOB: {emp.dob ? new Date(emp.dob).toLocaleDateString() : "N/A"}</span>
          </div>
        </div>

        <div className="employee-action-row">
          <button className="employee-delete-btn" onClick={onDelete}>
            <span className="employee-action-icon">&#128465;</span> Delete
          </button>
        </div>
      </div>
    </div>
  );
}

export default function Employees() {
  const [employees, setEmployees] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [search, setSearch] = useState("");
  const [showAdd, setShowAdd] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const token = localStorage.getItem("token");

  const getDepartmentName = (deptId) => {
    const dept = departments.find((d) => d.deptId === deptId);
    return dept ? dept.deptName : null;
  };

  function sanitizeEmployees(data) {
    return data.map((emp) => ({
      firstName: emp.firstName || "",
      lastName: emp.lastName || "",
      dob: emp.dob || "",
      contactNumber: emp.contactNumber || "",
      address: emp.address || "",
      deptId: emp.deptId || "",
      baseSalary: emp.baseSalary || "",
      hireDate: emp.hireDate || "",
      status: emp.status || "unknown",
      userId: emp.userId || emp.applicationUserId || "",
      empId: emp.empId || emp.id || emp.employeeId || "", // map empId from backend keys
    }));
  }

  const fetchEmployees = async () => {
    setLoading(true);
    setError("");
    try {
      const res = await fetch("https://localhost:7058/api/Employees/GetAllEmployees", {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });
      if (!res.ok) throw new Error("Failed to fetch employees");
      const data = await res.json();
      setEmployees(sanitizeEmployees(data));
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchDepartments = async () => {
    try {
      const res = await fetch("https://localhost:7058/api/Departments/GetAllDepartments", {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });
      if (!res.ok) return;
      const data = await res.json();
      setDepartments(data);
    } catch {
      // ignore errors
    }
  };

  useEffect(() => {
    fetchEmployees();
    fetchDepartments();
  }, []);

  const filteredEmployees = employees.filter((emp) => {
    const s = search.toLowerCase();
    return (
      (emp.firstName && emp.firstName.toLowerCase().includes(s)) ||
      (emp.lastName && emp.lastName.toLowerCase().includes(s)) ||
      (emp.contactNumber && emp.contactNumber.toLowerCase().includes(s)) ||
      (emp.address && emp.address.toLowerCase().includes(s)) ||
      (getDepartmentName(emp.deptId) &&
        getDepartmentName(emp.deptId).toLowerCase().includes(s))
    );
  });

  const onAddEmployee = () => {
    fetchEmployees();
    setShowAdd(false);
  };

  const handleDeleteEmployee = async (index) => {
    const confirmed = window.confirm("Delete this employee?");
    if (!confirmed) return;

    const empToDelete = employees[index];

    if (!empToDelete.empId) {
      setError("Cannot delete: employee ID missing");
      return;
    }

    console.log("Deleting employee with empId:", empToDelete.empId);

    try {
      const res = await fetch(
        `https://localhost:7058/api/Employees/DeleteEmployee${empToDelete.empId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      if (!res.ok) {
        const errorText = await res.text();
        throw new Error("Failed to delete employee: " + errorText);
      }
      setEmployees(employees.filter((_, i) => i !== index));
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="employees-container">
      <div className="employees-header-row">
        <input
          type="text"
          className="employees-search"
          placeholder="Search employees..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button className="add-employee-btn" onClick={() => setShowAdd(true)}>
          + Add Employee
        </button>
      </div>

      {loading && <div>Loading employees...</div>}
      {error && <div className="error-message">{error}</div>}

      <div className="employees-list">
        {filteredEmployees.length === 0 && !loading ? (
          <div className="no-employees-msg">No employees found.</div>
        ) : (
          filteredEmployees.map((emp, i) => (
            <EmployeeCard
              emp={emp}
              key={emp.userId || emp.empId || i}
              onDelete={() => handleDeleteEmployee(i)}
              getDepartmentName={getDepartmentName}
            />
          ))
        )}
      </div>

      {showAdd && <AddEmployee onCancel={() => setShowAdd(false)} onAdd={onAddEmployee} />}
    </div>
  );
}
