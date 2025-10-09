import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { GetAllDepartments, CreateDepartment, DeleteDepartment, UpdateDepartment } from "../Services/DepartmentService";

export const fetchDepartments = createAsyncThunk("departments/fetch", async () => {
  const response = await GetAllDepartments();

  if (Array.isArray(response)) {
    return response.map((d) => ({
      DeptId: d.deptId,
      name: d.deptName,
      icon: "👥",          // default icon or adjust if available
      employeeCount: 0,    // default count or get from backend if possible
      description: "",     // add if you have it or omit
    }));
  }

  return [];
});

export const fetchEmployeeById = createAsyncThunk(
  "employees/fetchEmployeeById",
  async (id, thunkAPI) => {
    try {
      const response = await GetEmployeeById(id);
      return response;
    } catch (error) {
      return thunkAPI.rejectWithValue("Failed to fetch employee data");
    }
  }
);



export const addDepartment = createAsyncThunk("departments/add", async (department) => {
  const response = await CreateDepartment(department);
  return response;
});

export const editDepartment = createAsyncThunk(
  "departments/edit",
  async ({ id, department }) => {
    const response = await UpdateDepartment(id, department);
    return response;
  }
);

export const removeDepartment = createAsyncThunk("departments/delete", async (id) => {
  await DeleteDepartment(id);
  return id;  // Return the id so we can filter it out in reducer
});

const departmentSlice = createSlice({
  name: "departments",
  initialState: { items: [], loading: false, error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchDepartments.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchDepartments.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchDepartments.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })
      .addCase(addDepartment.fulfilled, (state, action) => {
  // action.payload is likely { deptId, deptName }
  const backendDept = action.payload;
  state.items.push({
    DeptId: backendDept.deptId,
    name: backendDept.deptName,
    icon: "👥",         // or your default
    employeeCount: 0,   // default or from backend
    description: ""     // default or from backend
  });
})

      .addCase(editDepartment.fulfilled, (state, action) => {
        const index = state.items.findIndex((d) => d.DeptId === action.payload.DeptId);
        if (index >= 0) state.items[index] = action.payload;
      })
      .addCase(removeDepartment.fulfilled, (state, action) => {
        state.items = state.items.filter((d) => d.DeptId !== action.payload);
      });
  },
});

export default departmentSlice.reducer;
