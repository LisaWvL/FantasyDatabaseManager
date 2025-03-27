// src/api/languageApi.js
import axiosInstance from './axiosInstance';

export const fetchLanguages = async () => {
    try {
        const res = await axiosInstance.get('/language');
        return res.data;
    } catch (err) {
        console.error('❌ Failed to fetch languages:', err);
        throw err;
    }
};

export const fetchLanguageById = async (id) => {
    try {
        const res = await axiosInstance.get(`/language/${id}`);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to fetch language ${id}:`, err);
        throw err;
    }
};

export const createLanguage = async (payload) => {
    try {
        const res = await axiosInstance.post('/language/create', payload);
        return res.data;
    } catch (err) {
        console.error('❌ Failed to create language:', err);
        throw err;
    }
};

export const updateLanguage = async (id, payload) => {
    try {
        const res = await axiosInstance.put(`/language/${id}`, payload);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to update language ${id}:`, err);
        throw err;
    }
};

export const deleteLanguage = async (id) => {
    try {
        await axiosInstance.delete(`/language/${id}`);
    } catch (err) {
        console.error(`❌ Failed to delete language ${id}:`, err);
        throw err;
    }
};

export const fetchLanguageSnapshot = async (id) => {
    try {
        const res = await axiosInstance.get(`/language/${id}/new-snapshot`);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to fetch snapshot for language ${id}:`, err);
        throw err;
    }
};
