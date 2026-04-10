const BASE_URL = 'http://localhost:5284/api';

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
export const getSchedules = async (params) => {
  const query = new URLSearchParams(params).toString();
  const response = await fetch(`${BASE_URL}/schedules?${query}`);
  return handleResponse(response);
};

export const createSchedule = async (data) => {
  const response = await fetch(`${BASE_URL}/schedules`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSchedule = async (id, data) => {
  const response = await fetch(`${BASE_URL}/schedules/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSchedule = async (id) => {
  const response = await fetch(`${BASE_URL}/schedules/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Classes ---
export const getClasses = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/classes?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const createClass = async (data) => {
  const response = await fetch(`${BASE_URL}/classes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateClass = async (id, data) => {
  const response = await fetch(`${BASE_URL}/classes/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteClass = async (id) => {
  const response = await fetch(`${BASE_URL}/classes/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Teachers ---
export const getTeachers = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/teachers?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const createTeacher = async (data) => {
  const response = await fetch(`${BASE_URL}/teachers`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateTeacher = async (id, data) => {
  const response = await fetch(`${BASE_URL}/teachers/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteTeacher = async (id) => {
  const response = await fetch(`${BASE_URL}/teachers/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Departments ---
export const getDepartments = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/departments?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const createDepartment = async (data) => {
  const response = await fetch(`${BASE_URL}/departments`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateDepartment = async (id, data) => {
  const response = await fetch(`${BASE_URL}/departments/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteDepartment = async (id) => {
  const response = await fetch(`${BASE_URL}/departments/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Subjects ---
export const getSubjects = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/subjects?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const createSubject = async (data) => {
  const response = await fetch(`${BASE_URL}/subjects`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSubject = async (id, data) => {
  const response = await fetch(`${BASE_URL}/subjects/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSubject = async (id) => {
  const response = await fetch(`${BASE_URL}/subjects/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

// --- Schools ---
export const getSchools = async () => {
  const response = await fetch(`${BASE_URL}/schools`);
  return handleResponse(response);
};

export const getSchool = async (id) => {
  const response = await fetch(`${BASE_URL}/schools/${id}`);
  return handleResponse(response);
};

export const createSchool = async (data) => {
  const response = await fetch(`${BASE_URL}/schools`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const updateSchool = async (id, data) => {
  const response = await fetch(`${BASE_URL}/schools/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
};

export const deleteSchool = async (id) => {
  const response = await fetch(`${BASE_URL}/schools/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};


const API_URL = "https://schedule-manager-zon3.onrender.com";