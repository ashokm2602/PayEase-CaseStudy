import React, { useState } from 'react';

const EmployeeList = () => {
  const initialEmployees = [
    { id: 1, name: "Alice Johnson", position: "Developer", email: "alice@company.com" },
    { id: 2, name: "Bob Smith", position: "Designer", email: "bob@company.com" },
    { id: 3, name: "Charlie Brown", position: "Manager", email: "charlie@company.com" },
  ];

  const [employees, setEmployees] = useState(initialEmployees);
  const [editingId, setEditingId] = useState(null);
  const [editFormData, setEditFormData] = useState({ name: "", position: "", email: "" });

  const handleEditClick = (employee) => {
    setEditingId(employee.id);
    setEditFormData({
      name: employee.name,
      position: employee.position,
      email: employee.email,
    });
  };

  const handleCancelClick = () => {
    setEditingId(null);
  };

  const handleDeleteClick = (id) => {
    if (window.confirm("Are you sure you want to delete this employee?")) {
      setEmployees(employees.filter(emp => emp.id !== id));
    }
  };

  const handleInputChange = (e) => {
    setEditFormData({ ...editFormData, [e.target.name]: e.target.value });
  };

  const handleSaveClick = (id) => {
    setEmployees(
      employees.map(emp => (emp.id === id ? { ...emp, ...editFormData } : emp))
    );
    setEditingId(null);
  };

  return (
    <div style={{ padding: "2rem", maxWidth: "800px", margin: "auto" }}>
      <h2>Employee List</h2>
      <table style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr style={{ borderBottom: "2px solid #ccc" }}>
            <th style={{ padding: "8px", textAlign: "left" }}>Name</th>
            <th style={{ padding: "8px", textAlign: "left" }}>Position</th>
            <th style={{ padding: "8px", textAlign: "left" }}>Email</th>
            <th style={{ padding: "8px" }}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {employees.map(emp => (
            <tr key={emp.id} style={{ borderBottom: "1px solid #ddd" }}>
              <td style={{ padding: "8px" }}>
                {editingId === emp.id ? (
                  <input
                    type="text"
                    name="name"
                    value={editFormData.name}
                    onChange={handleInputChange}
                  />
                ) : (
                  emp.name
                )}
              </td>
              <td style={{ padding: "8px" }}>
                {editingId === emp.id ? (
                  <input
                    type="text"
                    name="position"
                    value={editFormData.position}
                    onChange={handleInputChange}
                  />
                ) : (
                  emp.position
                )}
              </td>
              <td style={{ padding: "8px" }}>
                {editingId === emp.id ? (
                  <input
                    type="email"
                    name="email"
                    value={editFormData.email}
                    onChange={handleInputChange}
                  />
                ) : (
                  emp.email
                )}
              </td>
              <td style={{ padding: "8px", textAlign: "center" }}>
                {editingId === emp.id ? (
                  <>
                    <button onClick={() => handleSaveClick(emp.id)} style={{ marginRight: "0.5rem" }}>Save</button>
                    <button onClick={handleCancelClick}>Cancel</button>
                  </>
                ) : (
                  <>
                    <button onClick={() => handleEditClick(emp)} style={{ marginRight: "0.5rem" }}>Edit</button>
                    <button onClick={() => handleDeleteClick(emp.id)}>Delete</button>
                  </>
                )}
              </td>
            </tr>
          ))}
          {employees.length === 0 && (
            <tr>
              <td colSpan="4" style={{ textAlign: "center", padding: "1rem" }}>
                No employees found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default EmployeeList;
