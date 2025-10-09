import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { addPayroll, fetchPayrolls } from "../Features/PayrollSlice";
import "./AddPayroll.css";

export default function AddPayroll({ onClose }) {
  const dispatch = useDispatch();
  const [form, setForm] = useState({
    payrollPeriodStart: "",
    payrollPeriodEnd: "",
    status: "",
    processedDate: "",
  });
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setError("");
    try {
      // The keys must match your DTO (PayrollDTO)
      await dispatch(
        addPayroll({
          payrollPeriodStart: form.payrollPeriodStart,
          payrollPeriodEnd: form.payrollPeriodEnd,
          status: form.status,
          processedDate: form.processedDate,
        })
      ).unwrap();
      dispatch(fetchPayrolls());
      onClose();
    } catch (err) {
      setError(err.message || "Failed to add payroll");
    }
    setSubmitting(false);
  };

  return (
    <div className="modal-backdrop">
      <div className="modal-window">
        <h3>Add Payroll</h3>
        <form onSubmit={handleSubmit} className="addpayroll-form">
          <label>
            Start Date:
            <input
              name="payrollPeriodStart"
              type="date"
              value={form.payrollPeriodStart}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            End Date:
            <input
              name="payrollPeriodEnd"
              type="date"
              value={form.payrollPeriodEnd}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Status:
            <select name="status" value={form.status} onChange={handleChange} required>
              <option value="">--Select Status--</option>
              <option value="Pending">Pending</option>
              <option value="Processed">Processed</option>
              <option value="Verified">Verified</option>
            </select>
          </label>
          <label>
            Processed Date:
            <input
              name="processedDate"
              type="date"
              value={form.processedDate}
              onChange={handleChange}
              required
            />
          </label>
          {error && <div style={{ color: "red" }}>{error}</div>}
          <div className="modal-actions">
            <button type="submit" disabled={submitting}>
              Add Payroll
            </button>
            <button type="button" onClick={onClose} disabled={submitting}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
