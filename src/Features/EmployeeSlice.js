import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import {
  GetAllEmployees,
  GetEmployeeById,
  AddEmployee,
  UpdateEmployeeById,
  DeleteEmployee
} from "../Services/EmployeeService";

// Fetch all employees
export const fetchEmployees = createAsyncThunk("employees/fetch", async () => {
  const response = await GetAllEmployees();
  if (Array.isArray(response)) {
    return response.map((e) => ({
      EmployeeId: e.employeeId,
      FirstName: e.firstName,
      LastName: e.lastName,
      DeptName: e.deptName,
      Email: e.email,
      DOB: e.dob,
      HireDate: e.hireDate,
      Location: e.location,
      Role: e.role,
      Status: e.status,
      // add more fields as per your backend shape
    }));
  }
  return [];
});

// Fetch employee by Id
export const fetchEmployeeById = createAsyncThunk(
  "employees/fetchById",
  async (id) => {
    const response = await GetEmployeeById(id);
    return response;
  }
);

// Add employee
export const addEmployee = createAsyncThunk("employees/add", async (employeeDTO) => {
  const response = await AddEmployee(employeeDTO);
  return response;
});

// Edit employee
export const editEmployee = createAsyncThunk(
  "employees/edit",
  async ({ id, employeeUpdateDTO }) => {
    const response = await UpdateEmployeeById(id, employeeUpdateDTO);
    return response;
  }
);

// Delete employee
export const removeEmployee = createAsyncThunk("employees/delete", async (id) => {
  await DeleteEmployee(id);
  return id;
});

const employeeSlice = createSlice({
  name: "employees",
  initialState: { items: [], loading: false, error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchEmployees.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchEmployees.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchEmployees.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })
      .addCase(addEmployee.fulfilled, (state, action) => {
        const backendEmp = action.payload;
        state.items.push({
          EmployeeId: backendEmp.employeeId,
          FirstName: backendEmp.firstName,
          LastName: backendEmp.lastName,
          DeptName: backendEmp.deptName,
          Email: backendEmp.email,
          DOB: backendEmp.dob,
          HireDate: backendEmp.hireDate,
          Location: backendEmp.location,
          Role: backendEmp.role,
          Status: backendEmp.status,
        });
      })
      .addCase(editEmployee.fulfilled, (state, action) => {
        const idx = state.items.findIndex((e) => e.EmployeeId === action.payload.employeeId);
        if (idx >= 0) state.items[idx] = action.payload;
      })
      .addCase(removeEmployee.fulfilled, (state, action) => {
        state.items = state.items.filter((e) => e.EmployeeId !== action.payload);
      });
  },
});

export default employeeSlice.reducer;
