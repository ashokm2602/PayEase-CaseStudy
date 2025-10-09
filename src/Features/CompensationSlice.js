import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import api from "../Features/api";
import { toast } from "react-toastify";

// Get all compensations
export const fetchAllCompensations = createAsyncThunk(
  "compensation/fetchAll",
  async (_, thunkAPI) => {
    try {
      const response = await api.get("/CompensationAdjustments/GetAllCompensationAdjustments");
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Get single compensation by ID
export const fetchCompensationById = createAsyncThunk(
  "compensation/fetchById",
  async (id, thunkAPI) => {
    try {
      const response = await api.get(`/CompensationAdjustments/GetCompensationById${id}`);
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Get all compensations for an employee
export const fetchCompensationsByEmpId = createAsyncThunk(
  "compensation/fetchByEmpId",
  async (empId, thunkAPI) => {
    try {
      const response = await api.get(`/CompensationAdjustments/GetCompensationByEmpId${empId}`);
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Add compensation
export const addCompensation = createAsyncThunk(
  "compensation/add",
  async (compDto, thunkAPI) => {
    try {
      const response = await api.post("/CompensationAdjustments/AddCompensation", compDto);
      toast.success("Compensation added!");
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Update
export const updateCompensation = createAsyncThunk(
  "compensation/update",
  async ({ id, compDto }, thunkAPI) => {
    try {
      const response = await api.put(`/CompensationAdjustments/UpdateCompensation${id}`, compDto);
      toast.success("Compensation updated!");
      return response.data;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// Delete
export const deleteCompensation = createAsyncThunk(
  "compensation/delete",
  async (id, thunkAPI) => {
    try {
      await api.delete(`/CompensationAdjustments/DeleteCompensation${id}`);
      toast.success("Compensation deleted!");
      return id;
    } catch (error) {
      toast.error(error.response?.data || error.message);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

const compensationSlice = createSlice({
  name: "compensation",
  initialState: {
    items: [],
    selected: null,
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllCompensations.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAllCompensations.fulfilled, (state, action) => {
        state.items = Array.isArray(action.payload) ? action.payload : [];
        state.loading = false;
      })
      .addCase(fetchAllCompensations.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(fetchCompensationById.fulfilled, (state, action) => {
        state.selected = action.payload;
      })
      .addCase(fetchCompensationsByEmpId.fulfilled, (state, action) => {
        state.items = Array.isArray(action.payload) ? action.payload : [];
      })
      .addCase(addCompensation.fulfilled, (state, action) => {
        state.items.push(action.payload);
      })
      .addCase(updateCompensation.fulfilled, (state, action) => {
        const idx = state.items.findIndex(
          (item) => item.adjustmentId === action.payload.adjustmentId
        );
        if (idx >= 0) state.items[idx] = action.payload;
      })
      .addCase(deleteCompensation.fulfilled, (state, action) => {
        state.items = state.items.filter(
          (item) => item.adjustmentId !== action.payload
        );
      });
  },
});

export default compensationSlice.reducer;
