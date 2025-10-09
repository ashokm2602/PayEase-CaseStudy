import axios from "axios";

const api_url = "https://localhost:7058/api/Employees";

// ✅ Helper: Get token from localStorage
const getToken = () => localStorage.getItem("token");

// ✅ Centralized error handler
const handleError = (error) => {
  if (error.response) {
    console.error(
      `Server Error (${error.response.status}): ${error.response.data?.message || error.message}`
    );
    return error.response.data;
  } else if (error.request) {
    console.error("No Response from Server. Please check the server or try again.");
    return null;
  } else {
    console.error(`Error: ${error.message}`);
    return null;
  }
};

// ✅ Get all employees
export const GetAllEmployees = async () => {
  try {
    const token = getToken();
    const response = await axios.get(`${api_url}/GetAllEmployees`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    handleError(error);
    return [];
  }
};

// ✅ Get employee by ID
export const GetEmployeeById = async (id) => {
  try {
    const token = getToken();
    console.log("Fetching employee by ID:", id);
    const response = await axios.get(`${api_url}/GetEmployeeById?id=${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    handleError(error);
    return null;
  }
};

// ✅ Add employee
export const AddEmployee = async (employeeDTO) => {
  try {
    const token = getToken();
    const response = await axios.post(`${api_url}/AddEmployee`, employeeDTO, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error);
    return null;
  }
};

// ✅ Update employee by ID
export const UpdateEmployeeById = async (id, employeeUpdateDTO) => {
  try {
    const token = getToken();
    const response = await axios.put(
      `${api_url}/UpdateEmployee?id=${id}`,
      employeeUpdateDTO,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;
  } catch (error) {
    handleError(error);
    return null;
  }
};

// ✅ Update employee using User ID
export const UpdateEmployeeWithUserId = async (userid, employeeUpdateDTO) => {
  try {
    const token = getToken();
    const response = await axios.put(
      `${api_url}/UpdateEmployeeWithUserId?userid=${userid}`,
      employeeUpdateDTO,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;
  } catch (error) {
    handleError(error);
    return null;
  }
};

// ✅ Delete employee
export const DeleteEmployee = async (id) => {
  try {
    const token = getToken();
    const response = await axios.delete(`${api_url}/DeleteEmployee/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    handleError(error);
    throw error; // important for Redux thunks
  }
};
