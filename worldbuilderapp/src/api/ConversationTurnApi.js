import axiosInstance from './axiosInstance';

export const fetchConversationHistory = async (plotpointId) => {
    const response = await axiosInstance.get(`/conversationturn/by-plotpoint/${plotpointId}`);
    return response.data;
};

export const fetchLatestTurns = async (plotpointId, limit = 10) => {
    const response = await axiosInstance.get(`/conversationturn/latest/${plotpointId}?limit=${limit}`);
    return response.data;
};

export const fetchSummaryTurn = async (plotpointId) => {
    const response = await axiosInstance.get(`/conversationturn/summary/${plotpointId}`);
    return response.data;
};

export const logConversationTurn = async (payload) => {
    const response = await axiosInstance.post('/conversationturn/log', payload);
    return response.data;
};

export const summarizeTurns = async (payload) => {
    const response = await axiosInstance.post('/conversationturn/summarize', payload);
    return response.data;
};

export const sendChatTurn = async (payload) => {
    const response = await fetch('http://localhost:8000/chat', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        throw new Error("Chat API failed");
    }

    return await response.json();
};

export const requestSummaryTurn = async (payload) => {
    const res = await axiosInstance.post(`/conversationturn/summarize`, payload);
    return res.data;
};

