// src/api/calendarApi.js
import axiosInstance from './axiosInstance';

export async function fetchCalendarGrid() {
    try {
        const response = await axiosInstance.get('/calendar/grid');
        return response.data;
    } catch (error) {
        console.error('? Failed to fetch calendar grid:', error);
        return [];
    }
}
