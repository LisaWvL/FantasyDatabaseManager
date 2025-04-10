// src/api/riverApi.js
import axiosInstance from '../../src/api/axiosInstance';

export const fetchRivers = async () => {
  const response = await axiosInstance.get('/river');
  return response.data;
};

export const fetchRiverById = async (id) => {
  const response = await axiosInstance.get(`/river/${id}`);
  return response.data;
};

export const createRiver = async (payload) => {
  const response = await axiosInstance.post('/river/create', payload);
  return response.data;
};

export const updateRiver = async (id, payload) => {
  const response = await axiosInstance.put(`/river/${id}`, payload);
  return response.data;
};

export const deleteRiver = async (id) => {
  await axiosInstance.delete(`/river/${id}`);
};
