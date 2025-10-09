import api from "../Features/api";

const LeaveService = {
  // Fetch all leaves (Admin view)
  getAllLeaves: () => api.get("/Leaves/GetAllLeaves"),

  // Fetch leaves by employee ID (Employee view)
  getLeavesByEmployeeId: (employeeId) => {
  console.log("LeaveService API call with employeeId:", employeeId);
  // Append employeeId directly to the endpoint
  return api.get(`/Leaves/GetLeavesByEmployeeId${employeeId}`);
},

  deleteLeave: (leaveId) => api.delete(`/Leaves/DeleteLeave${leaveId}`),




  // Approve or reject leave
  // Correct version:
approveLeave: (leaveId) =>
  api.put(`/Leaves/UpdateLeave${leaveId}?leave=Approved`),

rejectLeave: (leaveId) =>
  api.put(`/Leaves/UpdateLeave${leaveId}?leave=Rejected`),

  // Request new leave
  requestLeave: (leaveData) => api.post("/Leaves/AddLeave", leaveData),
};

export default LeaveService;
