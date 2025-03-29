//TODO
//PlotPointApi.js
//Add: fetchPlotPointsForSnapshot(snapshotId)
//Add: createPlotPointWithLinkedEntities(payload)
//Add: linkPlotPointToEntity(plotPointId, entityType, entityId)
//Add: unlinkPlotPointFromEntity(plotPointId, entityType, entityId)


// src/api/PlotPointApi.js

import axiosInstance from './axiosInstance';

export const fetchPlotPoints = async () => {
    try {
        const response = await axiosInstance.get('/plotpoint');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch plot points:', error.response?.data || error.message);
        return [];
    }
};

export const fetchPlotPointById = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot point ${id}:`, error.response?.data || error.message);
        throw error;
    }
};

export const createPlotPoint = async (payload) => {
    try {
        const response = await axiosInstance.post('/plotpoint/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create plot point:', error.response?.data || error.message);
        throw error;
    }
};

export const updatePlotPoint = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/plotpoint/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update plot point ${id}:`, error.response?.data || error.message);
        throw error;
    }
};

export const deletePlotPoint = async (id) => {
    try {
        await axiosInstance.delete(`/plotpoint/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete plot point ${id}:`, error.response?.data || error.message);
        throw error;
    }
};

export const fetchBySnapshot = async (snapshotId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/snapshot/${snapshotId}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot points for snapshot ${snapshotId}:`, error.response?.data || error.message);
        throw error;
    }
};


export const fetchByCalendarRange = async (startDateId, endDateId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/by-calendar-range?startDateId=${startDateId}&endDateId=${endDateId}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch plot points for calendar range`, error);
        throw error;
    }
};

// Add plotpoint with all linked entities at once
export const createPlotPointWithLinkedEntities = async (payload) => {
    try {
        const response = await axiosInstance.post('/plotpoint/create-with-links', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create plot point with links:', error);
        throw error;
    }
};


export const fetchForNewSnapshot = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}/new-snapshot`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch new snapshot for plot point ${id}:`, error.response?.data || error.message);
        throw error;
    }
};

export const fetchSnapshotEditPage = async (id) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${id}/new-snapshot-page`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch snapshot edit page for plot point ${id}:`, error.response?.data || error.message);
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
        console.error(`❌ Failed to update calendar for plot point ${plotPointId}:`, error.response?.data || error.message);
        throw error;
    }
};
