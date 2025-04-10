// src/api/calendarApi.js
import axiosInstance from '../../src/api/axiosInstance';

export async function fetchCalendarGrid() {
  try {
    const response = await axiosInstance.get('/calendar/grid');
    return response.data;
  } catch (error) {
    console.error('❌ Calendar Grid Error:', error.response?.data || error.message);
    return [];
  }
}

export const fetchCalendarDayByMonthAndDay = async (month, day) => {
  const response = await axiosInstance.get(`/calendar/day/${month}/${day}`);
  return response.data;
};

export const fetchMonths = async () => {
  const response = await axiosInstance.get('/calendar/months');
  return response.data;
};

export const fetchWeekdays = async () => {
  const response = await axiosInstance.get('/calendar/weekdays');
  return response.data;
};
