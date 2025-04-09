import axiosInstance from './axiosInstance';

export const fetchScenes = async () => {
    const response = await axiosInstance.get('/scene');
    return response.data;
};

export const fetchSceneById = async (id) => {
    const response = await axiosInstance.get(`/scene/${id}`);
    return response.data;
};

export const createScene = async (payload) => {
    const response = await axiosInstance.post('/scene/create', payload);
    return response.data;
};

export const updateScene = async (id, payload) => {
    const response = await axiosInstance.put(`/scene/${id}`, payload);
    return response.data;
};

export const deleteScene = async (id) => {
    await axiosInstance.delete(`/scene/${id}`);
};

export const duplicateScene = async (id) => {
    const response = await axiosInstance.get(`/scene/${id}/duplicate`);
    return response.data;
};

export const getGroupedScenesByName = async () => {
    const response = await axiosInstance.get('/scene/grouped-by-name');
    return response.data;
};

export const reorderScenesInChapter = async (chapterId, orderedSceneIds) => {
    const response = await axiosInstance.post(`/scene/${chapterId}/reorder-scenes`, orderedSceneIds);
    return response.data;
};
