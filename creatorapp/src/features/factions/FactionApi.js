// src/api/factionApi.js
import axiosInstance from '../../api/axiosInstance';

export const fetchFactions = async () => {
    try {
        const response = await axiosInstance.get('/faction');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch factions:', error);
        throw error;
    }
};

export const fetchFactionById = async (id) => {
    try {
        const response = await axiosInstance.get(`/faction/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch faction with id ${id}:`, error);
        throw error;
    }
};

export const createFaction = async (payload) => {
    try {
        const response = await axiosInstance.post('/faction/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create faction:', error);
        throw error;
    }
};

export const updateFaction = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/faction/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update faction with id ${id}:`, error);
        throw error;
    }
};

export const deleteFaction = async (id) => {
    try {
        await axiosInstance.delete(`/faction/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete faction with id ${id}:`, error);
        throw error;
    }
};

export const fetchNewChapter = async (id) => {
    try {
        const response = await axiosInstance.get(`/faction/${id}/new-chapter`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch new chapter for faction ${id}:`, error);
        throw error;
    }
};
