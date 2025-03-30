// src/api/SnapshotApi.js
import axiosInstance from './axiosInstance';

export const fetchSnapshots = async () => {
    try {
        const response = await axiosInstance.get('/snapshot');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch snapshots:', error);
        throw error;
    }
};

export const fetchSnapshotById = async (id) => {
    try {
        const response = await axiosInstance.get(`/snapshot/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch snapshot ${id}:`, error);
        throw error;
    }
};

export const createSnapshot = async (payload) => {
    try {
        const response = await axiosInstance.post('/snapshot/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create snapshot:', error);
        throw error;
    }
};

export const updateSnapshot = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/snapshot/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update snapshot ${id}:`, error);
        throw error;
    }
};

export const deleteSnapshot = async (id) => {
    try {
        await axiosInstance.delete(`/snapshot/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete snapshot ${id}:`, error);
        throw error;
    }
};

export const duplicateSnapshot = async (id) => {
    try {
        const response = await axiosInstance.get(`/snapshot/${id}/duplicate`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to duplicate snapshot ${id}:`, error);
        throw error;
    }
};

export const getGroupedByName = async () => {
    try {
        const response = await axiosInstance.get('/snapshot/grouped-by-name');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch grouped snapshots:', error);
        throw error;
    }
};

export const getNewSnapshotVersion = async (id) => {
    try {
        const response = await axiosInstance.get(`/snapshot/${id}/new-snapshot`);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to get new snapshot version:', error);
        throw error;
    }
};

export const fetchSnapshotEntities = async (snapshotId) => {
    try {
        const response = await axiosInstance.get(`/snapshot/${snapshotId}/entities`);
    return response.data;
    } catch (error) {
        console.error("Failed to fetch snapshot entities", error);
        throw error;
    }
};