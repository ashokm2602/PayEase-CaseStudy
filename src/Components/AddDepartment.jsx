import React, { useState } from "react";
import "./AddDepartment.css";

function AddDepartment({ onCancel, onAdd }) {
  const [form, setForm] = useState({
    name: "",
    description: "",
    head: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleAdd = () => {
  if (form.name.trim() === "") {
    alert("Department name cannot be empty.");
    return;
  }
  onAdd(form.name.trim());
};


  return (
    <div className="add-department-overlay">
      <div className="add-department-modal">
        <div className="add-department-header">
          <span>Add New Department</span>
          <button className="close-btn" onClick={onCancel}>&#10005;</button>
        </div>
        <div className="add-department-fields">
          <div className="field-row">
            <label>Department Name</label>
            <input
              name="name"
              value={form.name}
              onChange={handleChange}
              autoFocus
            />
          </div>
          
        </div>
        <div className="add-department-actions">
          <button className="cancel-btn" onClick={onCancel}>Cancel</button>
          <button className="add-btn" onClick={handleAdd}>Add Department</button>
        </div>
      </div>
    </div>
  );
}

export default AddDepartment;
