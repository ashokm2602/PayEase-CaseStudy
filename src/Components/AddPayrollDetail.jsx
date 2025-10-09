import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { addPayrollDetail } from "../Features/PayRollDetailSlice";
import './AddPayrollDetail.css';

const AddPayrollDetail = ({ onClose }) => {
  const dispatch = useDispatch();
  const [form, setForm] = useState({
    payrollId: "",
    empId: ""
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
      await dispatch(addPayrollDetail({
        payrollId: Number(form.payrollId),
        empId: Number(form.empId)
      })).unwrap();
      onClose();
    } catch (err) {
      setError("Failed to add payroll detail.");
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Add Payroll Detail</h3>
        <form onSubmit={handleSubmit}>
          <input
            type="number"
            name="payrollId"
            value={form.payrollId}
            onChange={handleChange}
            placeholder="Payroll ID"
            required
          />
          <input
            type="number"
            name="empId"
            value={form.empId}
            onChange={handleChange}
            placeholder="Employee ID"
            required
          />
          {error && <div className="error">{error}</div>}
          <div className="actions">
            <button type="submit">Add</button>
            <button type="button" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPayrollDetail;
