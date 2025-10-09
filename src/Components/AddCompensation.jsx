import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { addCompensation } from "../Features/CompensationSlice";
import "./AddCompensation.css";

const AddCompensation = ({ onClose }) => {
  const dispatch = useDispatch();
  const [form, setForm] = useState({
    empId: "",
    adjustmentType: "Benefit",
    amount: "",
    category: "",
    appliedDate: new Date().toISOString().slice(0, 16)
  });
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      await dispatch(addCompensation({
        empId: Number(form.empId),
        adjustmentType: form.adjustmentType,
        amount: parseFloat(form.amount),
        category: form.category,
        appliedDate: new Date(form.appliedDate).toISOString()
      })).unwrap();
      onClose();
    } catch {
      setError("Failed to add compensation.");
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Add Compensation</h3>
        <form onSubmit={handleSubmit}>
          <input
            type="number"
            name="empId"
            value={form.empId}
            onChange={handleChange}
            placeholder="Employee ID"
            required
          />
          <select
            className="select-input"
            name="adjustmentType"
            value={form.adjustmentType}
            onChange={handleChange}
            required
          >
            <option value="Benefit">Benefit</option>
            <option value="Deduction">Deduction</option>
          </select>
          <input
            type="number"
            step="0.01"
            name="amount"
            value={form.amount}
            onChange={handleChange}
            placeholder="Amount"
            required
          />
          <input
            type="text"
            name="category"
            value={form.category}
            onChange={handleChange}
            placeholder="Category"
            required
          />
          <input
            type="datetime-local"
            name="appliedDate"
            value={form.appliedDate}
            onChange={handleChange}
            required
          />
          {error && <div className="error">{error}</div>}
          <div className="actions">
            <button type="submit">Add</button>
            <button type="button" onClick={onClose}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddCompensation;
