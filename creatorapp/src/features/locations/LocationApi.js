// src/api/locationApi.js
import axiosInstance from '../../api/axiosInstance';

export const fetchLocations = async () => {
    try {
        const response = await axiosInstance.get('/location');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch locations:', error);
        throw error;
    }
};

export const fetchLocationById = async (id) => {
    try {
        const response = await axiosInstance.get(`/location/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch location with id ${id}:`, error);
        throw error;
    }
};

export const createLocation = async (payload) => {
    try {
        const response = await axiosInstance.post('/location/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create location:', error);
        throw error;
    }
};

export const updateLocation = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/location/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update location ${id}:`, error);
        throw error;
    }
};

export const deleteLocation = async (id) => {
    try {
        await axiosInstance.delete(`/location/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete location ${id}:`, error);
        throw error;
    }
};

export const fetchLocationChapter = async (id) => {
    try {
        const response = await axiosInstance.get(`/location/${id}/new-chapter`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to load chapter draft for location ${id}:`, error);
        throw error;
    }
};

export const fetchGroupedLocations = async () => {
    try {
        const response = await axiosInstance.get(`/location/grouped-by-name`);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch grouped locations:', error);
        throw error;
    }
};
