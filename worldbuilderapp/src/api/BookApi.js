import axiosInstance from './axiosInstance';

export const fetchBooks = async () => {
    const response = await axiosInstance.get('/book');
    return response.data;
};

export const fetchBookById = async (id) => {
    const response = await axiosInstance.get(`/book/${id}`);
    return response.data;
};

export const createBook = async (payload) => {
    const response = await axiosInstance.post('/book/create', payload);
    return response.data;
};

export const updateBook = async (id, payload) => {
    const response = await axiosInstance.put(`/book/${id}`, payload);
    return response.data;
};

export const deleteBook = async (id) => {
    await axiosInstance.delete(`/book/${id}`);
};

export const duplicateBook = async (id) => {
    const response = await axiosInstance.get(`/book/${id}/duplicate`);
    return response.data;
};

export const getGroupedBooksByName = async () => {
    const response = await axiosInstance.get('/book/grouped-by-name');
    return response.data;
};

export const getBookWordCount = async (id) => {
    const response = await axiosInstance.get(`/book/${id}/wordcount`);
    return response.data;
};

export const reorderActsInBook = async (bookId, orderedActIds) => {
    const response = await axiosInstance.post(`/book/${bookId}/reorder-acts`, orderedActIds);
    return response.data;
};
