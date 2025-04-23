// src/api/dateApi.js
import axiosInstance from '../../api/axiosInstance';

export async function fetchDateGrid() {
    try {
        const response = await axiosInstance.get('/date/grid');
        return response.data;
    } catch (error) {
        console.error('❌ Date Grid Error:', error.response?.data || error.message);
        return [];
    }
}

export const fetchDateDayByMonthAndDay = async (month, day) => {
    const response = await axiosInstance.get(`/date/day/${month}/${day}`);
    return response.data;
};

export const fetchMonths = async () => {
    const response = await axiosInstance.get('/date/months');
    return response.data;
};

export const fetchWeekdays = async () => {
    const response = await axiosInstance.get('/date/weekdays');
    return response.data;
};