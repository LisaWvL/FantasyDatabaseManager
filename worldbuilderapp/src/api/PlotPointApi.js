// src/api/plotPointApi.js

import axiosInstance from './axiosInstance';

export const fetchPlotPoints = async () => {
    try {
        const response = await axiosInstance.get('/plotpoint');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch plot points:', error);
        return [];
    }
};

export const fetchPlotPointById = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot point ${id}:`, error);
        throw error;
    }
};

export const createPlotPoint = async (payload) => {
    try {
        const response = await axiosInstance.post('/plotpoint/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create plot point:', error);
        throw error;
    }
};

export const updatePlotPoint = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/plotpoint/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update plot point ${id}:`, error);
        throw error;
    }
};

export const deletePlotPoint = async (id) => {
    try {
        await axiosInstance.delete(`/plotpoint/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete plot point ${id}:`, error);
        throw error;
    }
};

export const fetchBySnapshot = async (snapshotId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/snapshot/${snapshotId}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot points for snapshot ${snapshotId}:`, error);
        throw error;
    }
};

export const fetchByCalendar = async (calendarId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/by-calendar/${calendarId}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot points for calendar ${calendarId}:`, error);
        throw error;
    }
};

export const fetchForNewSnapshot = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}/new-snapshot`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch new snapshot for plot point ${id}:`, error);
        throw error;
    }
};

export const fetchSnapshotEditPage = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}/new-snapshot-page`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch snapshot edit page for plot point ${id}:`, error);
        throw error;
    }
};

export const updatePlotPointCalendar = async (plotPointId, calendarId) => {
    try {
        const plotPoint = await fetchPlotPointById(plotPointId);
        plotPoint.calendarId = calendarId;

        const response = await updatePlotPoint(plotPointId, plotPoint);
        return response;
    } catch (error) {
        console.error(`❌ Failed to update calendar for plot point ${plotPointId}:`, error);
        throw error;
    }
};
