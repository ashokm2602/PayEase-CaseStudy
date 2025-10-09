import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchPayrollDetailsByEmployeeId } from "../../Features/PayRollDetailSlice";
import "./EmployeePayroll.css";

function PayrollCard({ payroll }) {
  console.log("Rendering PayrollCard with:", payroll);

  return (
    <div className="payroll-card">
      <div className="payroll-card-header">
        <div className="payroll-avatar">{payroll.empId?.toString()[0] || "N"}</div>
        <div className="payroll-columns">
          <div><strong>Payroll Detail ID:</strong> {payroll.payrollDetailId}</div>
          <div><strong>Payroll ID:</strong> {payroll.payrollId}</div>
          <div><strong>Employee ID:</strong> {payroll.empId}</div>
        </div>
      </div>
      <div className="payroll-row">
        <div><strong>Basic Salary:</strong> ₹{payroll.basicSalary}</div>
        <div><strong>Net Salary:</strong> ₹{payroll.netSalary}</div>
      </div>
    </div>
  );
}

export default function EmployeePayroll() {
  const dispatch = useDispatch();
  const [searchTerm, setSearchTerm] = useState("");

  const { details, status, error } = useSelector((state) => state.payrollDetails) || {};
  const payrolls = Array.isArray(details) ? details : [];

  const employeeId = localStorage.getItem("employeeId");
  console.log("Employee ID from localStorage:", employeeId);

  useEffect(() => {
    if (employeeId) {
      console.log("Fetching payroll details for employee:", employeeId);
      dispatch(fetchPayrollDetailsByEmployeeId(employeeId));
    }
  }, [dispatch, employeeId]);

  console.log("Payroll details from Redux:", payrolls);
  console.log("Current fetch status:", status);

  const filteredPayrolls = payrolls.filter((payroll) => {
    const empIdStr = payroll.empId?.toString() || "";
    return empIdStr.toLowerCase().includes(searchTerm.toLowerCase());
  });

  console.log("Filtered payroll records:", filteredPayrolls);

  return (
    <div className="payroll-container">
      <div className="payroll-header">
        <input
          type="text"
          placeholder="Search payroll records by Employee ID..."
          className="payroll-search"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className="payroll-list">
        {status === "loading" && <div className="payroll-msg">Loading...</div>}
        {status === "failed" && <div className="payroll-msg payroll-error">Error: {error}</div>}
        {status === "succeeded" && (
          filteredPayrolls.length > 0 ? (
            filteredPayrolls.map((payroll) => (
              <PayrollCard key={payroll.payrollDetailId} payroll={payroll} />
            ))
          ) : (
            <div className="payroll-msg">No payroll records found.</div>
          )
        )}
        {(status !== "loading" && status !== "failed" && status !== "succeeded") && (
          <div className="payroll-msg">No payroll records found.</div>
        )}
      </div>
    </div>
  );
}
