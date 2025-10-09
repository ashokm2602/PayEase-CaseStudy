// EditEmployee.jsx
import React, { useState, useEffect } from "react";
import "./editemployee.css";

function EditEmployee({ employee, onClose, onSave }) {
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    dob: "",
    contactNumber: "",
    address: "",
  });

  useEffect(() => {
    if (employee) {
      setForm({
        firstName: employee.firstName || "",
        lastName: employee.lastName || "",
        dob: employee.dob ? new Date(employee.dob).toISOString().slice(0, 10) : "",
        contactNumber: employee.contactNumber || "",
        address: employee.address || "",
      });
    }
  }, [employee]);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleUpdate = () => {
    // Pass the updated form data back to parent to trigger backend update
    onSave(form);
    onClose();
  };

  return (
    <div className="edit-employee-overlay">
      <div className="edit-employee-modal">
        <div className="edit-employee-header">
          <span>Edit Employee</span>
          <button className="close-btn" onClick={onClose}>
            &#10005;
          </button>
        </div>
        <div className="edit-employee-fields">
          <div className="field-row">
            <label>First Name</label>
            <input name="firstName" value={form.firstName} onChange={handleChange} autoFocus />
          </div>
          <div className="field-row">
            <label>Last Name</label>
            <input name="lastName" value={form.lastName} onChange={handleChange} />
          </div>
          <div className="field-row">
            <label>Date of Birth</label>
            <input name="dob" type="date" value={form.dob} onChange={handleChange} />
          </div>
          <div className="field-row">
            <label>Contact Number</label>
            <input name="contactNumber" value={form.contactNumber} onChange={handleChange} />
          </div>
          <div className="field-row">
            <label>Address</label>
            <input name="address" value={form.address} onChange={handleChange} />
          </div>
        </div>

        <div className="edit-employee-actions">
          <button className="cancel-btn" onClick={onClose}>
            Cancel
          </button>
          <button className="update-btn" onClick={handleUpdate}>
            Update Employee
          </button>
        </div>
      </div>
    </div>
  );
}

export default EditEmployee;
