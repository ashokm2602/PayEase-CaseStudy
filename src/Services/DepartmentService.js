import axios from "axios";

const api_url = "https://localhost:7058/api/Departments";

const handleError = (error) => {
  if (error.response) {
    console.log(
      `Server Error (${error.response.status}): ${
        error.response.data?.message || error.message
      }`
    );
    return error.response.data;
  } else if (error.request) {
    console.log("No Response from Server. Please check the server or try again.");
    return null;
  } else {
    console.log(`Error: ${error.message}`);
    return null;
  }
};

// ✅ Get all departments
export const GetAllDepartments = async () => {
  try {
    const response = await axios.get(`${api_url}/GetAllDepartments`);
    return response.data;
  } catch (error) {
    handleError(error);
    return [];
  }
};

// ✅ Get department by Id (backend expects GetDepartmentById{id})
export const GetDepartmentById = async (id) => {
  try {
    const response = await axios.get(`${api_url}/GetDepartmentById${id}`);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// ✅ Create department
export const CreateDepartment = async (departmentName) => {
  try {
    const response = await axios.post(`${api_url}/CreateDepartment`, JSON.stringify(departmentName), {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// ✅ Update department
export const UpdateDepartment = async (id, department) => {
  try {
    const response = await axios.put(`${api_url}/UpdateDepartment${id}`, department, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// ✅ Delete department
export const DeleteDepartment = async (id) => {
  try {
    const response = await axios.delete(`${api_url}/DeleteDepartment${id}`);
    return response.data;
  } catch (error) {
    handleError(error);
    throw error;
  }
};
