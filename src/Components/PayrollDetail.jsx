import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchAllPayrollDetails,
  deletePayrollDetail,
} from "../Features/PayRollDetailSlice";
import './PayrollDetail.css'
import AddPayrollDetail from "./AddPayrollDetail";

const PayrollDetail = () => {
  const dispatch = useDispatch();
  const { details, status, error } = useSelector((state) => state.payrollDetails);
  const [showAddModal, setShowAddModal] = useState(false);

  useEffect(() => {
    dispatch(fetchAllPayrollDetails());
  }, [dispatch]);

  const handleDelete = (id) => {
    if (window.confirm("Are you sure you want to delete this detail?")) {
      dispatch(deletePayrollDetail(id));
    }
  };

  return (
    <div className="payroll-details-container">
      <div className="header">
        <h2>Payroll Details</h2>
        <button onClick={() => setShowAddModal(true)}>Add Payroll Detail</button>
      </div>

      {status === "loading" && <p>Loading...</p>}
      {status === "failed" && <p>Error: {error}</p>}

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Payroll ID</th>
            <th>Employee ID</th>
            <th>Basic Salary</th>
            <th>Net Salary</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {details && details.length > 0 ? (
            details.map((detail) => (
              <tr key={detail.payrollDetailId}>
                <td>{detail.payrollDetailId}</td>
                <td>{detail.payrollId}</td>
                <td>{detail.empId}</td>
                <td>{detail.basicSalary}</td>
                <td>{detail.netSalary}</td>
                <td>
                  <button onClick={() => handleDelete(detail.payrollDetailId)}>
                    Delete
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6">No payroll details found.</td>
            </tr>
          )}
        </tbody>
      </table>

      {showAddModal && (
        <AddPayrollDetail
          onClose={() => setShowAddModal(false)}
        />
      )}
    </div>
  );
};

export default PayrollDetail;
