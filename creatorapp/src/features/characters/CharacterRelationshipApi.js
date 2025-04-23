// src/api/CharacterRelationshipApi.js
import axiosInstance from '../../api/axiosInstance';

export const fetchCharacterRelationships = async () => {
    try {
        const response = await axiosInstance.get('/characterrelationship');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to fetch character relationships:', error);
        throw error;
    }
};

export const fetchCharacterRelationshipById = async (id) => {
    try {
        const response = await axiosInstance.get(`/characterrelationship/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch character relationship ${id}:`, error);
        throw error;
    }
};

export const createCharacterRelationship = async (payload) => {
    try {
        const response = await axiosInstance.post('/characterrelationship/create', payload);
        return response.data;
    } catch (error) {
        console.error('❌ Failed to create character relationship:', error);
        throw error;
    }
};

export const updateCharacterRelationship = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/characterrelationship/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update character relationship ${id}:`, error);
        throw error;
    }
};

export const deleteCharacterRelationship = async (id) => {
    try {
        const response = await axiosInstance.delete(`/characterrelationship/${id}`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to delete character relationship ${id}:`, error);
        throw error;
    }
};

export const fetchRelationshipsByCharacter = async (characterId) => {
    try {
        const response = await axiosInstance.get(
            `/characterrelationship/filter/by-character/${characterId}`
        );
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to fetch relationships for character ${characterId}:`, error);
        throw error;
    }
};

export const getNewChapterViewModel = async (id) => {
    try {
        const response = await axiosInstance.get(`/characterrelationship/${id}/new-chapter`);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to prepare new chapter for character relationship ${id}:`, error);
        throw error;
    }
};
