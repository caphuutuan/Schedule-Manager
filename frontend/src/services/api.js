const API_BASE_URL =
  process.env.REACT_APP_API_BASE_URL?.replace(/\/$/, '') || 'http://localhost:5284/api';

/**
 * Common fetch handler
 */
async function handleResponse(response) {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: response.statusText }));
    throw new Error(error.message || error.Error || 'API request failed');
  }
  if (response.status === 204) return null;
  const contentType = response.headers.get("content-type");
  if (contentType && contentType.indexOf("application/json") !== -1) {
    return response.json();
  }
  return null;
}

// --- Schedules ---
export const getSchedules = async (schoolId, params) => {
  const query = new URLSearchParams(params).toString();
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/schedules?${query}`);
  return handleResponse(response);
};

export const createSchedule = async (schoolId, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/schedules`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSchedule = async (schoolId, id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/schedules/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSchedule = async (schoolId, id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/schedules/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Classes ---
export const getClasses = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/classes`);
  return handleResponse(response);
};

export const createClass = async (schoolId, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/classes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateClass = async (schoolId, id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/classes/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteClass = async (schoolId, id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/classes/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Teachers ---
export const getTeachers = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/teachers`);
  return handleResponse(response);
};

export const createTeacher = async (schoolId, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/teachers`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateTeacher = async (schoolId, id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/teachers/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteTeacher = async (schoolId, id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/teachers/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Departments ---
export const getDepartments = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/departments`);
  return handleResponse(response);
};

export const createDepartment = async (schoolId, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/departments`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateDepartment = async (schoolId, id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/departments/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteDepartment = async (schoolId, id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/departments/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Subjects ---
export const getSubjects = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/subjects`);
  return handleResponse(response);
};

export const createSubject = async (schoolId, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/subjects`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSubject = async (schoolId, id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/subjects/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSubject = async (schoolId, id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/subjects/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Schools ---
export const getSchools = async () => {
  const response = await fetch(`${API_BASE_URL}/schools`);
  return handleResponse(response);
};

export const getSchool = async (id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${id}`);
  return handleResponse(response);
};

export const createSchool = async (data) => {
  const response = await fetch(`${API_BASE_URL}/schools`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSchool = async (id, data) => {
  const response = await fetch(`${API_BASE_URL}/schools/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSchool = async (id) => {
  const response = await fetch(`${API_BASE_URL}/schools/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Academic Years ---
export const getAcademicYears = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/academic-years`);
  return handleResponse(response);
};

export const getActiveAcademicYear = async (schoolId) => {
  const response = await fetch(`${API_BASE_URL}/schools/${schoolId}/academic-years/active`);
  return handleResponse(response);
};