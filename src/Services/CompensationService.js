// compensationService.js
const BASE_URL = "https://localhost:7058/api/CompensationAdjustments";

export async function getAllCompensations() {
  const res = await fetch(`${BASE_URL}/GetAllCompensationAdjustments`);
  if (!res.ok) throw new Error("Failed to fetch compensation adjustments");
  return res.json();
}

export async function getCompensationById(id) {
  const res = await fetch(`${BASE_URL}/GetCompensationById${id}`);
  if (!res.ok) throw new Error(`Compensation adjustment with ID ${id} not found`);
  return res.json();
}

export async function getCompensationByEmpId(empId) {
  const res = await fetch(`${BASE_URL}/GetCompensationByEmpId${empId}`);
  if (!res.ok) throw new Error(`Compensation adjustments for empId ${empId} not found`);
  return res.json();
}

export async function addCompensation(compDto) {
  const res = await fetch(`${BASE_URL}/AddCompensation`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(compDto),
  });
  if (!res.ok) throw new Error("Failed to add compensation adjustment");
  return res.json();
}

export async function updateCompensation(id, compDto) {
  const res = await fetch(`${BASE_URL}/UpdateCompensation${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(compDto),
  });
  if (!res.ok) throw new Error(`Failed to update compensation adjustment with ID ${id}`);
  return res.json();
}

export async function deleteCompensation(id) {
  const res = await fetch(`${BASE_URL}/DeleteCompensation${id}`, {
    method: "DELETE",
  });
  if (!res.ok) throw new Error(`Failed to delete compensation adjustment with ID ${id}`);
}
