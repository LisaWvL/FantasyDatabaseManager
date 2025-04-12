import axiosInstance from '../../src/api/axiosInstance';

export const fetchActs = async () => {
  const response = await axiosInstance.get('/act');
  return response.data;
};

export const fetchActById = async (id) => {
  const response = await axiosInstance.get(`/act/${id}`);
  return response.data;
};

export const createAct = async (payload) => {
  const response = await axiosInstance.post('/act/create', payload);
  return response.data;
};

export const updateAct = async (id, payload) => {
  const response = await axiosInstance.put(`/act/${id}`, payload);
  return response.data;
};

export const deleteAct = async (id) => {
  await axiosInstance.delete(`/act/${id}`);
};

export const duplicateAct = async (id) => {
  const response = await axiosInstance.get(`/act/${id}/duplicate`);
  return response.data;
};

export const getGroupedActsByName = async () => {
  const response = await axiosInstance.get('/act/grouped-by-name');
  return response.data;
};

export const getActWordCount = async (id) => {
  const response = await axiosInstance.get(`/act/${id}/wordcount`);
  return response.data;
};

export const reorderChaptersInAct = async (actId, orderedChapterIds) => {
  const response = await axiosInstance.post(`/act/${actId}/reorder-chapters`, orderedChapterIds);
  return response.data;
};
