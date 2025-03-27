// Path: src/api/eraApi.js
import axiosInstance from './axiosInstance';

export const fetchEras = async () => {
    try {
        const response = await axiosInstance.get('/era');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch eras:', error);
        throw error;
    }
};

export const fetchEraById = async (id) => {
    try {
        const response = await axiosInstance.get(`/era/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch era ${id}:`, error);
        throw error;
    }
};

export const createEra = async (payload) => {
    try {
        const response = await axiosInstance.post('/era/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create era:', error);
        throw error;
    }
};

export const updateEra = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/era/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update era ${id}:`, error);
        throw error;
    }
};

export const deleteEra = async (id) => {
    try {
        await axiosInstance.delete(`/era/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete era ${id}:`, error);
        throw error;
    }
};

export const fetchNewSnapshotEra = async (id) => {
    try {
        const response = await axiosInstance.get(`/era/${id}/new-snapshot`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch snapshot version of era ${id}:`, error);
        throw error;
    }
};
