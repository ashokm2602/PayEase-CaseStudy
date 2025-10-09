import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import api from "./api";
import { toast } from "react-toastify";

// Fetch all payroll details
export const fetchAllPayrollDetails = createAsyncThunk(
  "payrollDetails/fetchAll",
  async (_, thunkAPI) => {
    try {
      const response = await api.get("/PayrollDetails/GetAllPayrollDetails");
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Fetch by employee
export const fetchPayrollDetailsByEmployeeId = createAsyncThunk(
  "payrollDetails/fetchByEmployeeId",
  async (employeeId, thunkAPI) => {
    try {
      const response = await api.get(`/PayrollDetails/GetPayrollDetailsByEmployeeId${employeeId}`);
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Fetch single detail
export const fetchPayrollDetailById = createAsyncThunk(
  "payrollDetails/fetchById",
  async (id, thunkAPI) => {
    try {
      const response = await api.get(`/PayrollDetails/GetPayrollDetailById${id}`);
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Add payroll detail
export const addPayrollDetail = createAsyncThunk(
  "payrollDetails/add",
  async (payrollDetail, thunkAPI) => {
    try {
      const response = await api.post("/PayrollDetails/AddPayrollDetail", payrollDetail);
      toast.success("Payroll detail added successfully!");
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Delete payroll detail
export const deletePayrollDetail = createAsyncThunk(
  "payrollDetails/delete",
  async (id, thunkAPI) => {
    try {
      await api.delete(`/PayrollDetails/DeletePayrollDetail${id}`);
      toast.success("Payroll detail deleted!");
      return id;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

const payrollDetailSlice = createSlice({
  name: "payrollDetails",
  initialState: {
    details: [],
    detail: null,
    status: "idle",
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllPayrollDetails.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchAllPayrollDetails.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.details = Array.isArray(action.payload) ? action.payload : [];
      })
      .addCase(fetchAllPayrollDetails.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      .addCase(fetchPayrollDetailsByEmployeeId.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchPayrollDetailsByEmployeeId.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.details = Array.isArray(action.payload) ? action.payload : [];
      })
      .addCase(fetchPayrollDetailsByEmployeeId.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      .addCase(fetchPayrollDetailById.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchPayrollDetailById.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.detail = action.payload;
      })
      .addCase(fetchPayrollDetailById.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      .addCase(addPayrollDetail.pending, (state) => {
        state.status = "loading";
      })
      .addCase(addPayrollDetail.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.details.push(action.payload);
      })
      .addCase(addPayrollDetail.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      .addCase(deletePayrollDetail.pending, (state) => {
        state.status = "loading";
      })
      .addCase(deletePayrollDetail.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.details = state.details.filter(
          (detail) => detail.payrollDetailId !== action.payload
        );
      })
      .addCase(deletePayrollDetail.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      });
  },
});

export default payrollDetailSlice.reducer;
