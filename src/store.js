import { configureStore } from '@reduxjs/toolkit';
import departmentReducer from './Features/DepartmentSlice';
import leaveReducer from './Features/LeaveSlice';
import employeeReducer from './Features/EmployeeSlice';
import payrollReducer from './Features/PayrollSlice';
import payrollDetailReducer from './Features/PayRollDetailSlice';
import compensationReducer from './Features/CompensationSlice'; // Add this if using compensation feature

const store = configureStore({
  reducer: {
    departments: departmentReducer,
    leaves: leaveReducer,
    employees: employeeReducer,
    payroll: payrollReducer,
    payrollDetails: payrollDetailReducer,
    compensation: compensationReducer, // Optional: include if you have a compensation slice
  },
});

export default store;
