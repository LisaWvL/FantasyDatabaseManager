// src/api/ChapterApi.js
import axiosInstance from '../../src/api/axiosInstance';

export const fetchChapters = async () => {
  try {
    const response = await axiosInstance.get('/chapter');
    return response.data;
  } catch (error) {
    console.error('❌ Failed to fetch chapters:', error);
    throw error;
  }
};

export const fetchChapterById = async (id) => {
  try {
    const response = await axiosInstance.get(`/chapter/${id}`);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to fetch chapter ${id}:`, error);
    throw error;
  }
};

export const createChapter = async (payload) => {
  try {
    const response = await axiosInstance.post('/chapter/create', payload);
    return response.data;
  } catch (error) {
    console.error('❌ Failed to create chapter:', error);
    throw error;
  }
};

export const updateChapter = async (id, payload) => {
  try {
    const response = await axiosInstance.put(`/chapter/${id}`, payload);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to update chapter ${id}:`, error);
    throw error;
  }
};

export const deleteChapter = async (id) => {
  try {
    await axiosInstance.delete(`/chapter/${id}`);
  } catch (error) {
    console.error(`❌ Failed to delete chapter ${id}:`, error);
    throw error;
  }
};

export const duplicateChapter = async (id) => {
  try {
    const response = await axiosInstance.get(`/chapter/${id}/duplicate`);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to duplicate chapter ${id}:`, error);
    throw error;
  }
};

export const getGroupedByName = async () => {
  try {
    const response = await axiosInstance.get('/chapter/grouped-by-name');
    return response.data;
  } catch (error) {
    console.error('❌ Failed to fetch grouped chapters:', error);
    throw error;
  }
};

export const getNewChapterVersion = async (id) => {
  try {
    const response = await axiosInstance.get(`/chapter/${id}/new-chapter`);
    return response.data;
  } catch (error) {
    console.error('❌ Failed to get new chapter version:', error);
    throw error;
  }
};

export const fetchChapterEntities = async (chapterId) => {
  try {
    const response = await axiosInstance.get(`/chapter/${chapterId}/entities`);
    return response.data; // ✅ this line was missing in your snippet
  } catch (error) {
    console.error('Failed to fetch chapter entities', error);
    throw error;
  }
};
