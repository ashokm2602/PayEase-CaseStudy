import React, { useState } from "react";
import "./AddEmployee.css";

export default function AddEmployee({ onCancel }) {
  const [userForm, setUserForm] = useState({
    username: "",
    email: "",
    password: "",
    role: "Employee",
  });

  const [employeeForm, setEmployeeForm] = useState({
    userId: "",
    firstName: "",
    lastName: "",
    dob: "",
    hireDate: "",
    deptId: "",
    contactNumber: "",
    address: "",
    baseSalary: "",
  });

  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [usersList, setUsersList] = useState([]);
  const [selectedUserId, setSelectedUserId] = useState("");

  const handleUserChange = (e) => {
    setUserForm({ ...userForm, [e.target.name]: e.target.value });
  };

  const handleEmployeeChange = (e) => {
    setEmployeeForm({ ...employeeForm, [e.target.name]: e.target.value });
  };

  const fetchUsers = async () => {
    try {
      const res = await fetch("https://localhost:7058/api/Users/getallusers", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      if (!res.ok) throw new Error("Failed to fetch users.");
      const data = await res.json();
      setUsersList(data);
    } catch (err) {
      setError(err.message);
    }
  };

  const handleRegisterUser = async () => {
    setLoading(true);
    setError("");
    try {
      const res = await fetch("https://localhost:7058/api/Authentication/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userForm),
      });

      if (!res.ok) {
        const errMsg = await res.text();
        throw new Error(errMsg || "User registration failed.");
      }

      await res.json();
      await fetchUsers();
      setStep(2);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAddEmployee = async () => {
    const userIdToUse = selectedUserId || employeeForm.userId;
    if (!userIdToUse) {
      setError("Please select or enter a UserId.");
      return;
    }
    setLoading(true);
    setError("");
    try {
      const employeeData = {
        ...employeeForm,
        userId: userIdToUse,
        deptId: Number(employeeForm.deptId),
        baseSalary: Number(employeeForm.baseSalary),
      };

      const res = await fetch("https://localhost:7058/api/Employees/AddEmployee", {
  method: "POST",
  headers: { 
    "Content-Type": "application/json",
    "Authorization": `Bearer ${localStorage.getItem("token")}` // <-- Add this line!
  },
  body: JSON.stringify(employeeData),
});


      if (!res.ok) {
        const errMsg = await res.text();
        throw new Error(errMsg || "Adding employee failed.");
      }

      await res.json();
      onCancel();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="add-employee-overlay">
      <div className="add-employee-modal">
        <div className="add-employee-header">
          <span>{step === 1 ? "Register User" : "Add Employee Details"}</span>
          <button className="close-btn" onClick={onCancel} disabled={loading}>
            &#10005;
          </button>
        </div>

        {step === 1 && (
          <div className="add-employee-fields">
            <div className="field-row">
              <label>Username</label>
              <input name="username" value={userForm.username} onChange={handleUserChange} autoFocus />
            </div>
            <div className="field-row">
              <label>Email</label>
              <input name="email" type="email" value={userForm.email} onChange={handleUserChange} />
            </div>
            <div className="field-row">
              <label>Password</label>
              <input name="password" type="password" value={userForm.password} onChange={handleUserChange} />
            </div>
            <div className="field-row">
              <label>Role</label>
              <select name="role" value={userForm.role} onChange={handleUserChange}>
                <option value="Employee">Employee</option>
                <option value="Admin">Admin</option>
              </select>
            </div>
          </div>
        )}

        {step === 2 && (
          <>
            <div className="users-list-container">
              <h3>Registered Users (Select UserId)</h3>
              <table className="users-table">
                <thead>
                  <tr>
                    <th>UserId</th>
                    <th>Username</th>
                    <th>Email</th>
                    <th>Roles</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  {usersList.map((user) => (
                    <tr key={user.userId}>
                      <td>
                        <input
                          type="text"
                          value={user.userId}
                          readOnly
                          onClick={(e) => e.target.select()}
                          className="user-id-input"
                        />
                      </td>
                      <td>{user.username}</td>
                      <td>{user.email}</td>
                      <td>{user.roles.join(", ")}</td>
                      <td>
                        <button className="select-user-btn" onClick={() => setSelectedUserId(user.userId)}>
                          Use this UserId
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="field-row">
                <label>UserId for Employee Creation</label>
                <input
                  type="text"
                  value={selectedUserId || employeeForm.userId}
                  onChange={(e) => setEmployeeForm({ ...employeeForm, userId: e.target.value })}
                  placeholder="Or enter UserId here"
                />
              </div>
            </div>

            <div className="add-employee-fields">
              <div className="field-row">
                <label>First Name</label>
                <input name="firstName" value={employeeForm.firstName} onChange={handleEmployeeChange} autoFocus />
              </div>
              <div className="field-row">
                <label>Last Name</label>
                <input name="lastName" value={employeeForm.lastName} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Date of Birth</label>
                <input name="dob" type="date" value={employeeForm.dob} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Hire Date</label>
                <input name="hireDate" type="date" value={employeeForm.hireDate} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Department ID</label>
                <input name="deptId" type="number" value={employeeForm.deptId} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Contact Number</label>
                <input name="contactNumber" value={employeeForm.contactNumber} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Address</label>
                <input name="address" value={employeeForm.address} onChange={handleEmployeeChange} />
              </div>
              <div className="field-row">
                <label>Base Salary</label>
                <input name="baseSalary" type="number" value={employeeForm.baseSalary} onChange={handleEmployeeChange} />
              </div>
            </div>
          </>
        )}

        {error && <div className="error-message">{error}</div>}

        <div className="add-employee-actions">
          <button className="cancel-btn" onClick={onCancel} disabled={loading}>
            Cancel
          </button>
          {step === 1 ? (
            <button className="add-btn" onClick={handleRegisterUser} disabled={loading}>
              {loading ? "Registering..." : "Register User"}
            </button>
          ) : (
            <button className="add-btn" onClick={handleAddEmployee} disabled={loading}>
              {loading ? "Adding Employee..." : "Add Employee"}
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
