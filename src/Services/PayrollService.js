// PayrollService.js
import api from "../Features/api";

const BASE_URL = "/Payrolls";

export async function getAllPayrolls() {
  const response = await api.get(`${BASE_URL}/GetAllPayrolls`);
  return response.data;
}

export async function getPayrollById(id) {
  const response = await api.get(`${BASE_URL}/GetPayrollById/${id}`);
  return response.data;
}

export async function addPayroll(payrollDto) {
  const response = await api.post(`${BASE_URL}/AddPayroll`, payrollDto);
  return response.data;
}

export async function updatePayroll(id, payrollDto) {
  const response = await api.put(`${BASE_URL}/UpdatePayroll/${id}`, payrollDto);
  return response.data;
}

export async function deletePayroll(id) {
  const response = await api.delete(`${BASE_URL}/DeletePayroll${id}`);
  return response.data;
}
