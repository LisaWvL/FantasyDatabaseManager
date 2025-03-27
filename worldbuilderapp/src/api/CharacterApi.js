import axiosInstance from './axiosInstance';

export const fetchCharacters = async () => {
    const response = await axiosInstance.get('/character');
    return response.data;
};

export const fetchCharacterById = async (id) => {
    const response = await axiosInstance.get(`/character/${id}`);
    return response.data;
};

export const createCharacter = async (payload) => {
    const response = await axiosInstance.post('/character/create', payload);
    return response.data;
};

export const updateCharacter = async (id, payload) => {
    const response = await axiosInstance.put(`/character/${id}`, payload);
    return response.data;
};

export const deleteCharacter = async (id) => {
    await axiosInstance.delete(`/character/${id}`);
};

export const duplicateCharacter = async (id) => {
    const response = await axiosInstance.get(`/character/${id}/duplicate`);
    return response.data;
};

export const fetchGroupedCharacters = async () => {
    const response = await axiosInstance.get('/character/grouped-by-name');
    return response.data;
};

export const fetchNewSnapshotForCharacter = async (id) => {
    const response = await axiosInstance.get(`/character/${id}/new-snapshot`);
    return response.data;
};
