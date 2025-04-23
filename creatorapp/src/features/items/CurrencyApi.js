import axiosInstance from '../../api/axiosInstance';

export const fetchCurrencies = async () => {
    try {
        const response = await axiosInstance.get('/currency');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch currencies:', error);
        throw error;
    }
};
export const fetchCurrencyById = async (id) => {
    try {
        const response = await axiosInstance.get(`/currency/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch currency with id ${id}:`, error);
        throw error;
    }
};
export const createCurrency = async (data) => {
    try {
        const response = await axiosInstance.post('/currency/create', data);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create currency:', error);
        throw error;
    }
};
export const updateCurrency = async (id, data) => {
    try {
        const response = await axiosInstance.put(`/currency/${id}`, data);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update currency with id ${id}:`, error);
        throw error;
    }
};
export const deleteCurrency = async (id) => {
    try {
        await axiosInstance.delete(`/currency/${id}`);
    } catch (error) {
        console.error(`❌ Failed to delete currency with id ${id}:`, error);
        throw error;
    }
};
export const duplicateCurrency = async (id) => {
    try {
        const response = await axiosInstance.get(`/currency/${id}/duplicate`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to duplicate currency with id ${id}:`, error);
        throw error;
    }
};
export const getGroupedCurrenciesByName = async () => {
    try {
        const response = await axiosInstance.get('/currency/grouped-by-name');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch grouped currencies by name:', error);
        throw error;
    }
};
