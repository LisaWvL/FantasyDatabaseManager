// src/api/eventApi.js
import axiosInstance from './axiosInstance';

export const fetchEvents = async () => {
    const res = await axiosInstance.get('/event');
    return res.data;
};

export const fetchEventById = async (id) => {
    const res = await axiosInstance.get(`/event/${id}`);
    return res.data;
};

export const createEvent = async (payload) => {
    const res = await axiosInstance.post('/event/create', payload);
    return res.data;
};

export const updateEvent = async (id, payload) => {
    const res = await axiosInstance.put(`/event/${id}`, payload);
    return res.data;
};

export const deleteEvent = async (id) => {
    await axiosInstance.delete(`/event/${id}`);
};

export const fetchDuplicateForChapter = async (id) => {
    const res = await axiosInstance.get(`/event/${id}/new-chapter`);
    return res.data;
};

export const fetchGroupedByName = async () => {
    const res = await axiosInstance.get(`/event/grouped-by-name`);
    return res.data;
};
