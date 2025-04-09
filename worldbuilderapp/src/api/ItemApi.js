// src/api/itemApi.js
import axiosInstance from './axiosInstance';

export const fetchItems = async () => {
    try {
        const res = await axiosInstance.get('/item');
        return res.data;
    } catch (err) {
        console.error('❌ Failed to fetch items:', err);
        throw err;
    }
};

export const fetchItemById = async (id) => {
    try {
        const res = await axiosInstance.get(`/item/${id}`);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to fetch item ${id}:`, err);
        throw err;
    }
};

export const createItem = async (payload) => {
    try {
        const res = await axiosInstance.post('/item/create', payload);
        return res.data;
    } catch (err) {
        console.error('❌ Failed to create item:', err);
        throw err;
    }
};

export const updateItem = async (id, payload) => {
    try {
        const res = await axiosInstance.put(`/item/${id}`, payload);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to update item ${id}:`, err);
        throw err;
    }
};

export const deleteItem = async (id) => {
    try {
        await axiosInstance.delete(`/item/${id}`);
    } catch (err) {
        console.error(`❌ Failed to delete item ${id}:`, err);
        throw err;
    }
};

export const fetchItemDuplicate = async (id) => {
    try {
        const res = await axiosInstance.get(`/item/${id}/duplicate`);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to fetch duplicate for item ${id}:`, err);
        throw err;
    }
};

export const fetchItemChapter = async (id) => {
    try {
        const res = await axiosInstance.get(`/item/${id}/new-chapter`);
        return res.data;
    } catch (err) {
        console.error(`❌ Failed to prepare chapter for item ${id}:`, err);
        throw err;
    }
};
