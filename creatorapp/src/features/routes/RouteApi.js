// src/api/routeApi.js
import axiosInstance from '../../api/axiosInstance';

export const fetchRoutes = async () => {
    const response = await axiosInstance.get('/route');
    return response.data;
};

export const fetchRouteById = async (id) => {
    const response = await axiosInstance.get(`/route/${id}`);
    return response.data;
};

export const createRoute = async (payload) => {
    const response = await axiosInstance.post('/route/create', payload);
    return response.data;
};

export const updateRoute = async (id, payload) => {
    const response = await axiosInstance.put(`/route/${id}`, payload);
    return response.data;
};

export const deleteRoute = async (id) => {
    await axiosInstance.delete(`/route/${id}`);
};
