import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import {
  fetchLeaves,
  fetchLeavesByEmployee,
  approveLeave,
  rejectLeave,
  setSearchTerm,
} from "../Features/LeaveSlice";
import "./Leaves.css";

function LeaveCard({ leave, onApprove, onReject }) {
  const displayName = leave.employeeName || leave.name || "No Name";
  const displayEmpId = leave.empId || leave.employeeId || "N/A";
  const displayLeaveType = leave.leaveType || leave.type || "N/A";
  const displayStatus = leave.status || "N/A";

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
          📅 {new Date(leave.startDate || leave.start_date).toLocaleDateString()} -{" "}
          {new Date(leave.endDate || leave.end_date).toLocaleDateString()}
        </div>
        <div className="leave-info-item">🆔 {displayEmpId}</div>
      </div>
      {displayStatus === "Pending" && (
        <div className="leave-actions">
          <button className="approve-btn" onClick={() => onApprove(leave.leaveId)}>
            Approve
          </button>
          <button className="reject-btn" onClick={() => onReject(leave.leaveId)}>
            Reject
          </button>
        </div>
      )}
    </div>
  );
}

export default function Leaves({ employeeId }) {
  const dispatch = useDispatch();
  const { leaves, status, error, searchTerm } = useSelector((state) => state.leaves) || {};
  const safeLeaves = Array.isArray(leaves) ? leaves : [];
  const [filterStatus, setFilterStatus] = useState("All Status");

  useEffect(() => {
    if (employeeId) {
      dispatch(fetchLeavesByEmployee(employeeId));
    } else {
      dispatch(fetchLeaves());
    }
  }, [dispatch, employeeId]);

  const filteredLeaves = safeLeaves.filter((leave) => {
    const displayName = leave.employeeName || leave.name || "";
    const displayEmpId = leave.empId || leave.employeeId || "";
    const leaveStatus = leave.status || "";

    const matchesStatus = filterStatus === "All Status" || leaveStatus === filterStatus;
    const matchesSearch =
      displayName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      displayEmpId.toString().toLowerCase().includes(searchTerm.toLowerCase());

    return matchesStatus && matchesSearch;
  });

  const handleApprove = (leaveId) => {
    console.log("Approve clicked for leaveId:", leaveId);
    dispatch(approveLeave(leaveId));
  };

  const handleReject = (leaveId) => {
    dispatch(rejectLeave(leaveId));
  };

  return (
    <div className="leave-container">
      <div className="leave-header">
        <input
          type="text"
          placeholder="Search leave requests..."
          className="leave-search"
          value={searchTerm}
          onChange={(e) => dispatch(setSearchTerm(e.target.value))}
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

      <div className="leave-list">
        {status === "loading" && <div>Loading...</div>}
        {status === "failed" && <div>Error: {error}</div>}
        {status === "succeeded" &&
          (filteredLeaves.length ? (
            filteredLeaves.map((leave) => (
              <LeaveCard
                key={leave.leaveId}
                leave={leave}
                onApprove={handleApprove}
                onReject={handleReject}
              />
            ))
          ) : (
            <div className="no-leaves-msg">No leave requests found.</div>
          ))}
      </div>
    </div>
  );
}
