// src/api/priceExampleApi.js
import axiosInstance from '../../src/api/axiosInstance';

export const fetchPriceExamples = async () => {
  try {
    const response = await axiosInstance.get('/priceexample');
    return response.data;
  } catch (error) {
    console.error('❌ Failed to fetch price examples:', error);
    throw error;
  }
};

export const fetchPriceExampleById = async (id) => {
  try {
    const response = await axiosInstance.get(`/priceexample/${id}`);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to fetch price example with id ${id}:`, error);
    throw error;
  }
};

export const createPriceExample = async (data) => {
  try {
    const response = await axiosInstance.post('/priceexample/create', data);
    return response.data;
  } catch (error) {
    console.error('❌ Failed to create price example:', error);
    throw error;
  }
};

export const updatePriceExample = async (id, data) => {
  try {
    const response = await axiosInstance.put(`/priceexample/${id}`, data);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to update price example with id ${id}:`, error);
    throw error;
  }
};

export const deletePriceExample = async (id) => {
  try {
    await axiosInstance.delete(`/priceexample/${id}`);
  } catch (error) {
    console.error(`❌ Failed to delete price example with id ${id}:`, error);
    throw error;
  }
};
