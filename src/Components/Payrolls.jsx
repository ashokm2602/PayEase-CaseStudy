import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { fetchPayrolls, deletePayroll } from "../Features/PayrollSlice";
import AddPayroll from "./AddPayroll";
import "./Payrolls.css";

export default function Payrolls() {
  const dispatch = useDispatch();
  const payrolls = useSelector((state) => state.payroll.items);
  const loading = useSelector((state) => state.payroll.loading);
  const error = useSelector((state) => state.payroll.error);
  const [showAdd, setShowAdd] = useState(false);

  useEffect(() => {
    dispatch(fetchPayrolls());
  }, [dispatch]);

  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this payroll?")) {
      await dispatch(deletePayroll(id));
      dispatch(fetchPayrolls());
    }
  };

  return (
    <div className="payrolls-container">
      <div className="payrolls-header">
        <h2>Payrolls</h2>
        <button className="add-payroll-btn" onClick={() => setShowAdd(true)}>
          + Add Payroll
        </button>
      </div>

      {loading && <div>Loading payrolls...</div>}
      {error && <div style={{ color: "red" }}>{error}</div>}
      {!loading && !error && payrolls.length === 0 && (
        <div className="no-payrolls-msg">No payrolls found.</div>
      )}
      {!loading && !error && payrolls.length > 0 && (
        <div className="payroll-list">
          {payrolls.map((p) => (
            <div className="payroll-card" key={p.payrollId}>
              <div className="payroll-card-header">
                <div className="payroll-icon-bg">
                  {p.status === "Processed"
                    ? "💰"
                    : p.status === "Verified"
                    ? "✅"
                    : "⏳"}
                </div>
                <div className="payroll-label">#{p.payrollId}</div>
              </div>
              <div className="payroll-details">
                <div className="payroll-info">
                  <div className="payroll-info-item">
                    <strong>Start:</strong>{" "}
                    {p.payrollPeriodStart
                      ? new Date(p.payrollPeriodStart).toLocaleDateString()
                      : "-"}
                  </div>
                  <div className="payroll-info-item">
                    <strong>End:</strong>{" "}
                    {p.payrollPeriodEnd
                      ? new Date(p.payrollPeriodEnd).toLocaleDateString()
                      : "-"}
                  </div>
                  <div
                    className={`payroll-info-item payroll-status payroll-status-${p.status?.toLowerCase()}`}
                  >
                    {p.status ?? "-"}
                  </div>
                </div>
                <div className="payroll-applied-date">
                  <strong>Date:</strong>{" "}
                  {p.processedDate
                    ? new Date(p.processedDate).toLocaleDateString()
                    : "-"}
                </div>
                <button
                  className="payroll-delete-btn"
                  onClick={() => handleDelete(p.payrollId)}
                  title="Delete Payroll"
                >
                  Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
      {showAdd && <AddPayroll onClose={() => setShowAdd(false)} />}
    </div>
  );
}
