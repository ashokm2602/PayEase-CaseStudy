// PayrollSlice.js
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import * as PayrollService from "../Services/PayrollService";

export const fetchPayrolls = createAsyncThunk("payroll/fetchAll", async () => {
  return await PayrollService.getAllPayrolls();
});

export const fetchPayrollById = createAsyncThunk(
  "payroll/fetchById",
  async (id) => await PayrollService.getPayrollById(id)
);

export const addPayroll = createAsyncThunk(
  "payroll/add",
  async (payrollDto) => await PayrollService.addPayroll(payrollDto)
);

export const updatePayroll = createAsyncThunk(
  "payroll/update",
  async ({ id, payrollDto }) => await PayrollService.updatePayroll(id, payrollDto)
);

export const deletePayroll = createAsyncThunk(
  "payroll/delete",
  async (id) => {
    await payrollService.deletePayroll(id);
    return id;
  }
);

const payrollSlice = createSlice({
  name: "payroll",
  initialState: {
    items: [],
    selected: null,
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch all payrolls
      .addCase(fetchPayrolls.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchPayrolls.fulfilled, (state, action) => {
        state.items = action.payload;
        state.loading = false;
      })
      .addCase(fetchPayrolls.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })

      // Fetch payroll by ID
      .addCase(fetchPayrollById.fulfilled, (state, action) => {
        state.selected = action.payload;
      })

      // Add payroll
      .addCase(addPayroll.fulfilled, (state, action) => {
        state.items.push(action.payload);
      })

      // Update payroll
      .addCase(updatePayroll.fulfilled, (state, action) => {
        const index = state.items.findIndex(
          (item) => item.payrollId === action.payload.payrollId
        );
        if (index >= 0) state.items[index] = action.payload;
      })

      // Delete payroll
      .addCase(deletePayroll.fulfilled, (state, action) => {
        state.items = state.items.filter(
          (item) => item.payrollId !== action.payload
        );
      });
  },
});

export default payrollSlice.reducer;
