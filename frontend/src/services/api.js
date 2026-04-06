const BASE_URL = 'http://localhost:5284/api';

/**
 * Common fetch handler
 */
async function handleResponse(response) {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: response.statusText }));
    throw new Error(error.message || error.Error || 'API request failed');
  }
  return response.json();
}

export const getSchedules = async (params) => {
  const query = new URLSearchParams(params).toString();
  const response = await fetch(`${BASE_URL}/schedules?${query}`);
  return handleResponse(response);
};

export const getClasses = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/classes?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const getTeachers = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/teachers?schoolId=${schoolId}`);
  return handleResponse(response);
};

export const getDepartments = async (schoolId) => {
  const response = await fetch(`${BASE_URL}/departments?schoolId=${schoolId}`);
  return handleResponse(response);
};
