import React, { useEffect, useState } from "react";
import {
  GetEmployeeById,
  UpdateEmployeeWithUserId,
} from "../../Services/EmployeeService";
import { GetDepartmentById } from "../../Services/DepartmentService";
import "./employeeprofile.css";

function EmployeeProfile() {
  const [employee, setEmployee] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [updatedProfile, setUpdatedProfile] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Robust date formatting for all cases
  const formatDate = (dateString) => {
    if (!dateString) return "";
    // ISO string
    if (dateString.includes("T")) {
      return dateString.split("T")[0];
    }
    // dd-mm-yyyy or yyyy-mm-dd
    const parts = dateString.split("-");
    if (parts.length === 3) {
      if (parts[0].length === 4) {
        // yyyy-mm-dd
        return dateString;
      } else if (parts[2].length === 4) {
        // dd-mm-yyyy -> yyyy-mm-dd
        return `${parts[2]}-${parts[1]}-${parts[0]}`;
      }
    }
    // fallback: try Date parse
    const d = new Date(dateString);
    if (!isNaN(d)) {
      return d.toISOString().split("T")[0];
    }
    return "";
  };

  useEffect(() => {
    const fetchEmployee = async () => {
      setLoading(true);
      const employeeId = localStorage.getItem("employeeId");

      if (!employeeId) {
        setError("No employee ID found in localStorage");
        setLoading(false);
        return;
      }

      try {
        const data = await GetEmployeeById(employeeId);
        if (!data) {
          setError("No employee data available");
          setLoading(false);
          return;
        }

        // Fetch department name
        let departmentName = "";
        if (data.deptId) {
          const dept = await GetDepartmentById(data.deptId);
          departmentName = dept?.deptName || "";
        } else {
          departmentName = data.deptName || "";
        }

        const employeeWithDept = { ...data, deptName: departmentName };

        setEmployee(employeeWithDept);
        setUpdatedProfile({
          firstName: data.firstName || "",
          lastName: data.lastName || "",
          dob: formatDate(data.dob),
          address: data.address || "",
          contactNumber: data.contactNumber || "",
          deptName: departmentName,
        });
      } catch (err) {
        console.error("Error fetching employee:", err);
        if (err.response?.status === 401) {
          setError("Unauthorized. Please log in again.");
        } else {
          setError("Failed to fetch employee data.");
        }
      } finally {
        setLoading(false);
      }
    };

    fetchEmployee();
  }, []);

  const handleEdit = () => setEditMode(true);

  const handleCancel = () => {
    setEditMode(false);
    if (employee) {
      setUpdatedProfile({
        firstName: employee.firstName || "",
        lastName: employee.lastName || "",
        dob: formatDate(employee.dob),
        address: employee.address || "",
        contactNumber: employee.contactNumber || "",
        deptName: employee.deptName || "",
      });
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUpdatedProfile((prev) => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    const userId = localStorage.getItem("userId");

    if (!userId) {
      alert("User ID not found.");
      return;
    }

    try {
      const updated = await UpdateEmployeeWithUserId(userId, updatedProfile);
      if (!updated) {
        alert("Failed to update profile.");
        return;
      }

      alert("Profile updated successfully!");
      setEmployee(updated);
      setEditMode(false);
    } catch (err) {
      console.error("Error updating profile:", err);
      if (err.response?.status === 401) {
        alert("Session expired. Please log in again.");
      } else {
        alert("Failed to update profile.");
      }
    }
  };

  if (loading) return <div>Loading profile...</div>;
  if (error) return <div className="error-message">{error}</div>;

  const displayHireDate = employee?.hireDate
    ? formatDate(employee.hireDate)
    : "";

  return (
    <div className="profile-container">
      <div className="profile-details">
        <div className="profile-avatar">
          <span className="avatar-letter">
            {(employee.firstName && employee.firstName.charAt(0).toUpperCase()) ||
              "E"}
          </span>
        </div>
        <div className="profile-info">
          <h2>
            {employee.firstName} {employee.lastName}
          </h2>
          <p className="profile-role">{employee.deptName}</p>
          <p>
            <strong>Location:</strong> {employee.address || "N/A"}
          </p>
        </div>
      </div>

      <div className="personal-info">
        <div className="header-row">
          <h3>Personal Information</h3>
          {!editMode && (
            <button className="edit-btn" onClick={handleEdit}>
              Edit
            </button>
          )}
        </div>

        <div className="info-grid">
          {["firstName", "lastName"].map((field) => (
  <div className="info-field" key={field}>
    <label>
      {field.charAt(0).toUpperCase() +
        field.slice(1).replace("Name", " Name")}
    </label>
    <input
      name={field}
      type="text"
      value={editMode ? updatedProfile[field] || "" : employee[field] || ""}
      onChange={handleChange}
      disabled={!editMode}
    />
  </div>
))}
<div className="info-field">
  <label>Dob</label>
  <input
    name="dob"
    type="date"
    value={
      editMode ? updatedProfile.dob || "" : formatDate(employee.dob) || ""
    }
    onChange={handleChange}
    disabled={!editMode}
  />
</div>


          {/* Editable Address (Location) */}
          <div className="info-field">
            <label>Address</label>
            <input
              name="address"
              type="text"
              value={
                editMode
                  ? updatedProfile.address || ""
                  : employee.address || ""
              }
              onChange={handleChange}
              disabled={!editMode}
            />
          </div>

          {/* Editable Contact Number */}
          <div className="info-field">
            <label>Contact Number</label>
            <input
              name="contactNumber"
              type="text"
              value={
                editMode
                  ? updatedProfile.contactNumber || ""
                  : employee.contactNumber || ""
              }
              onChange={handleChange}
              disabled={!editMode}
            />
          </div>

          {/* Department Name - not editable */}
          <div className="info-field">
            <label>Department</label>
            <input
              name="deptName"
              type="text"
              value={employee.deptName || ""}
              disabled
            />
          </div>

          {/* Hire Date - not editable */}
          <div className="info-field">
            <label>Hire Date</label>
            <input value={displayHireDate || ""} disabled />
          </div>
        </div>

        {editMode && (
          <div className="btn-row">
            <button className="save-btn" onClick={handleSave}>
              Save
            </button>
            <button className="cancel-btn" onClick={handleCancel}>
              Cancel
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

export default EmployeeProfile;
