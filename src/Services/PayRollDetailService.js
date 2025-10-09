import api from "../Features/api";

const PayrollDetailService = {
  // Get all payroll details (for Payroll Processor, Manager)
  getAllPayrollDetails: () => api.get("/PayrollDetails/GetAllPayrollDetails"),

  // Get payroll details by Employee ID (for Employee role)
  getPayrollDetailsByEmployeeId: (employeeId) =>
    api.get(`/PayrollDetails/GetPayrollDetailsByEmployeeId${employeeId}`),

  // Get payroll detail by payroll detail id
  getPayrollDetailById: (id) =>
    api.get(`/PayrollDetails/GetPayrollDetailById/${id}`),

  // Add new payroll detail
  addPayrollDetail: (payrollDetail) =>
    api.post("/PayrollDetails/AddPayrollDetail", payrollDetail),

  // Delete payroll detail by id
  deletePayrollDetail: (id) =>
    api.delete(`/PayrollDetails/DeletePayrollDetail/${id}`),
};

export default PayrollDetailService;
