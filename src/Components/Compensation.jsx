import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchAllCompensations,
  deleteCompensation,
} from "../Features/CompensationSlice";
import AddCompensation from "./AddCompensation";
import "./Compensation.css";

const Compensation = () => {
  const dispatch = useDispatch();
  const { items, loading, error } = useSelector((state) => state.compensation);
  const [showAddModal, setShowAddModal] = useState(false);

  useEffect(() => {
    dispatch(fetchAllCompensations());
  }, [dispatch]);

  const handleDelete = (id) => {
    if (window.confirm("Are you sure you want to delete this compensation?")) {
      dispatch(deleteCompensation(id));
    }
  };

  return (
    <div className="comp-container">
      <div className="comp-header">
        <h2>Compensations</h2>
        <button className="add-comp-btn" onClick={() => setShowAddModal(true)}>
          Add Compensation
        </button>
      </div>
      {loading && <p>Loading...</p>}
      {error && <p className="comp-error">{error}</p>}
      <div className="comp-list">
        {items && items.length > 0 ? (
          items.map((comp) => (
            <div className="comp-card" key={comp.adjustmentId}>
              <div><strong>ID:</strong> {comp.adjustmentId}</div>
              <div><strong>EmpID:</strong> {comp.empId}</div>
              <div><strong>Type:</strong> {comp.adjustmentType}</div>
              <div><strong>Amount:</strong> {comp.amount}</div>
              <div><strong>Category:</strong> {comp.category}</div>
              <div>
                <strong>Date:</strong>{" "}
                {comp.appliedDate?.slice(0, 16).replace("T", " ")}
              </div>
              <button
                className="comp-delete-btn"
                onClick={() => handleDelete(comp.adjustmentId)}
              >
                Delete
              </button>
            </div>
          ))
        ) : (
          <div className="no-comp-msg">No compensations found.</div>
        )}
      </div>
      {showAddModal && <AddCompensation onClose={() => setShowAddModal(false)} />}
    </div>
  );
};

export default Compensation;
