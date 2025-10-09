import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchLeavesForCurrentEmployee,
  deleteLeave,
} from "../../Features/LeaveSlice";
import "./EmployeeLeaves.css";

function LeaveCard({ leave, onDelete }) {
  const displayName = leave.EmployeeName || leave.employeeName || "No Name";
  const displayEmpId = leave.EmpId || leave.employeeId || "N/A";
  const displayLeaveType = leave.LeaveType || leave.leaveType || "N/A";
  const displayStatus = leave.Status || leave.status || "N/A";
  const leaveId = leave.LeaveId || leave.leaveId || leave.id;

  return (
    <div className="leave-card">
      <div className="leave-card-header">
        <div className="leave-avatar">{displayName[0]}</div>
        <div className="leave-details">
          <div className="leave-name">{displayName}</div>
          <div className="leave-type">{displayLeaveType}</div>
        </div>
        <div
          className={`leave-status leave-status-${displayStatus.toLowerCase()}`}
        >
          {displayStatus}
        </div>
      </div>
      <div className="leave-info">
        <div className="leave-info-item">
          📅 {new Date(leave.StartDate || leave.startDate).toLocaleDateString()} -{" "}
          {new Date(leave.EndDate || leave.endDate).toLocaleDateString()}
        </div>
        <div className="leave-info-item">🆔 {displayEmpId}</div>
      </div>
      <button
        className="delete-btn"
        onClick={() => onDelete(leaveId)}
        style={{
          marginTop: "8px",
          background: "#e53935",
          color: "#fff",
          border: "none",
          borderRadius: "4px",
          padding: "4px 10px",
          cursor: "pointer",
        }}
      >
        Delete
      </button>
    </div>
  );
}

export default function EmployeeLeaves() {
  const dispatch = useDispatch();
  const [filterStatus, setFilterStatus] = useState("All Status");
  const [searchTerm, setSearchTerm] = useState("");
  const [successMsg, setSuccessMsg] = useState("");

  const { leaves, status, error } = useSelector((state) => state.leaves) || {};
  const safeLeaves = Array.isArray(leaves) ? leaves : [];

  useEffect(() => {
    dispatch(fetchLeavesForCurrentEmployee());
  }, [dispatch]);

  const filteredLeaves = safeLeaves.filter((leave) => {
    const displayName = leave.EmployeeName || leave.employeeName || "";
    const displayEmpId = leave.EmpId || leave.employeeId || "";
    const leaveStatus = leave.Status || leave.status || "";

    const matchesStatus =
      filterStatus === "All Status" || leaveStatus === filterStatus;
    const matchesSearch =
      displayName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      displayEmpId.toString().toLowerCase().includes(searchTerm.toLowerCase());

    return matchesStatus && matchesSearch;
  });

  const handleDelete = async (leaveId) => {
    if (window.confirm("Are you sure you want to delete this leave request?")) {
      const resultAction = await dispatch(deleteLeave(leaveId));
      if (deleteLeave.fulfilled.match(resultAction)) {
        setSuccessMsg("Leave deleted successfully!");
        setTimeout(() => setSuccessMsg(""), 3000);
      }
    }
  };

  return (
    <div className="leave-container">
      <div className="leave-header">
        <input
          type="text"
          placeholder="Search leave requests..."
          className="leave-search"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <select
          className="leave-status-filter"
          value={filterStatus}
          onChange={(e) => setFilterStatus(e.target.value)}
        >
          <option>All Status</option>
          <option>Pending</option>
          <option>Approved</option>
          <option>Rejected</option>
        </select>
      </div>
      {successMsg && (
        <div className="success-message" style={{ color: "green", margin: "10px 0" }}>
          {successMsg}
        </div>
      )}
      <div className="leave-list">
        {status === "loading" && <div>Loading...</div>}
        {status === "failed" && <div>Error: {error}</div>}
        {status === "succeeded" &&
          (filteredLeaves.length ? (
            filteredLeaves.map((leave, index) => (
              <LeaveCard
                key={leave.LeaveId || leave.leaveId || leave.id || `${leave.employeeId}-${index}`}
                leave={leave}
                onDelete={handleDelete}
              />
            ))
          ) : (
            <div className="no-leaves-msg">No leave requests found.</div>
          ))}
      </div>
    </div>
  );
}
