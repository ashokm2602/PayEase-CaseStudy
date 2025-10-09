import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { requestLeave } from "../../Features/LeaveSlice";
import "./RequestLeave.css";

export default function RequestLeave({ onClose }) {
  const dispatch = useDispatch();
  const [form, setForm] = useState({
    leaveType: "",
    startDate: "",
    endDate: "",
    reason: "",
  });
  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const empId = Number(localStorage.getItem("employeeId")) || 0;
    const leaveData = {
      empId,
      leaveType: form.leaveType,
      startDate: form.startDate,
      endDate: form.endDate,
      status: "pending",
      reason: form.reason
    };
    await dispatch(requestLeave(leaveData));
    setSuccess(true);
    setForm({
      leaveType: "",
      startDate: "",
      endDate: "",
      reason: "",
    });
    setTimeout(() => setSuccess(false), 3000); // hides message after 3 seconds
  };

  const handleCancel = () => {
    if (typeof onClose === "function") onClose();
  };

  return (
    <div className="request-leave-card">
      <form onSubmit={handleSubmit} className="leave-request-form">
        <h3>Request Leave</h3>
        {success && (
          <div className="success-message" style={{ color: "green", marginBottom: "10px" }}>
            Leave requested successfully!
          </div>
        )}
        <label>Type</label>
        <input
          name="leaveType"
          value={form.leaveType}
          onChange={handleChange}
          required
        />
        <label>Start Date</label>
        <input
          type="date"
          name="startDate"
          value={form.startDate}
          onChange={handleChange}
          required
        />
        <label>End Date</label>
        <input
          type="date"
          name="endDate"
          value={form.endDate}
          onChange={handleChange}
          required
        />
        <label>Reason</label>
        <textarea
          name="reason"
          value={form.reason}
          onChange={handleChange}
          required
        />
        <div style={{ marginTop: '16px' }}>
          <button type="submit" className="approve-btn">Submit</button>
          <button type="button" className="cancel-btn" onClick={handleCancel}>
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
}
