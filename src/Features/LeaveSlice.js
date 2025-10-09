import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import LeaveService from "../Services/LeaveService";

// Thunks for fetching, requesting, approving, rejecting leaves (unchanged)
export const fetchLeaves = createAsyncThunk(
  "leaves/fetchAll",
  async (_, thunkAPI) => {
    try {
      const response = await LeaveService.getAllLeaves();
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchLeavesByEmployee = createAsyncThunk(
  "leaves/fetchByEmployeeId",
  async (employeeId, thunkAPI) => {
    try {
      const response = await LeaveService.getLeavesByEmployeeId(employeeId);
      console.log("API response:", response.data);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const approveLeave = createAsyncThunk(
  "leaves/approve",
  async (leaveId, thunkAPI) => {
    try {
      const response = await LeaveService.approveLeave(leaveId);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const rejectLeave = createAsyncThunk(
  "leaves/reject",
  async (leaveId, thunkAPI) => {
    try {
      const response = await LeaveService.rejectLeave(leaveId);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchLeavesForCurrentEmployee = createAsyncThunk(
  "leaves/fetchForEmployee",
  async (_, thunkAPI) => {
    try {
      const employeeId = localStorage.getItem("employeeId");
      if (!employeeId) return [];
      const response = await LeaveService.getLeavesByEmployeeId(employeeId);
      console.log("Employee leaves API response:", response.data);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Request Leave thunk
export const requestLeave = createAsyncThunk(
  "leaves/request",
  async (leaveData, thunkAPI) => {
    try {
      const response = await LeaveService.requestLeave(leaveData);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Delete Leave thunk
export const deleteLeave = createAsyncThunk(
  "leaves/delete",
  async (leaveId, thunkAPI) => {
    try {
      await LeaveService.deleteLeave(leaveId);
      return leaveId; // only return deleted id
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

const leaveSlice = createSlice({
  name: "leaves",
  initialState: {
    leaves: [],
    status: "idle",
    error: null,
    searchTerm: "",
  },
  reducers: {
    setSearchTerm(state, action) {
      state.searchTerm = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchLeaves.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchLeaves.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.leaves = Array.isArray(action.payload) ? action.payload : [];
      })
      .addCase(fetchLeaves.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      .addCase(approveLeave.fulfilled, (state, action) => {
        const updated = action.payload;
        const idx = state.leaves.findIndex((l) => String(l.leaveId) === String(updated.leaveId));
        if (idx !== -1) state.leaves[idx] = updated;
      })
      .addCase(rejectLeave.fulfilled, (state, action) => {
        const updated = action.payload;
        const idx = state.leaves.findIndex((l) => String(l.leaveId) === String(updated.leaveId));
        if (idx !== -1) state.leaves[idx] = updated;
      })
      .addCase(requestLeave.fulfilled, (state, action) => {
        state.leaves.push(action.payload);
      })
      .addCase(requestLeave.rejected, (state, action) => {
        state.error = action.payload;
      })
      .addCase(fetchLeavesForCurrentEmployee.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchLeavesForCurrentEmployee.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.leaves = Array.isArray(action.payload) ? action.payload : [];
      })
      .addCase(fetchLeavesForCurrentEmployee.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload;
      })
      // Add deleteLeave reducer
      .addCase(deleteLeave.fulfilled, (state, action) => {
        state.leaves = state.leaves.filter(l => String(l.leaveId) !== String(action.payload));
      })
      .addCase(deleteLeave.rejected, (state, action) => {
        state.error = action.payload;
      });
  },
});

export const { setSearchTerm } = leaveSlice.actions;
export default leaveSlice.reducer;
