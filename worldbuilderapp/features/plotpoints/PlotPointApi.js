import axiosInstance from '../../src/api/axiosInstance';

export const fetchPlotPoints = async () => {
  try {
    const response = await axiosInstance.get('/plotpoint');
    return response.data;
  } catch (error) {
    console.error('❌ Failed to fetch plot points:', error.response?.data || error.message);
    return [];
  }
};

export const fetchPlotPointById = async (id) => {
  try {
    const response = await axiosInstance.get(`/plotpoint/${id}`);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to fetch plot point ${id}:`, error.response?.data || error.message);
    throw error;
  }
};

export const createPlotPoint = async (payload) => {
  try {
    const response = await axiosInstance.post('/plotpoint', payload);
    return response.data;
  } catch (error) {
    console.error('❌ Failed to create plot point:', error.response?.data || error.message);
    throw error;
  }
};

export const fetchPlotPointsForChapter = async (chapterId) => {
  try {
    const response = await axiosInstance.get(`/plotpoint/chapter/${chapterId}`);
    return response.data;
  } catch (error) {
    console.error(
      `❌ Failed to fetch plot points for chapter ${chapterId}:`,
      error.response?.data || error.message
    );
    return [];
  }
};

export const fetchPlotPointEntities = async (plotPointId) => {
  try {
    const response = await axiosInstance.get(`/plotpoint/${plotPointId}/PlotPointentities`);
    return response.data;
  } catch (error) {
    console.error(`❌ Failed to fetch plot point entities:`, error.response?.data || error.message);
    return {};
  }
};

export const createPlotPointWithLinkedEntities = async (payload) => {
  try {
    const response = await axiosInstance.post('/plotpoint/create-with-entities', payload);
    return response.data;
  } catch (error) {
    console.error(
      '❌ Failed to create plot point with entities:',
      error.response?.data || error.message
    );
    throw error;
  }
};

export const linkPlotPointToEntity = async (plotPointId, entityType, entityId) => {
  try {
    const response = await axiosInstance.post(`/plotpoint/${plotPointId}/link`, {
      entityType,
      entityId,
    });
    return response.data;
  } catch (error) {
    console.error(
      `❌ Failed to link ${entityType} (${entityId}) to plot point ${plotPointId}:`,
      error.response?.data || error.message
    );
    throw error;
  }
};

export const unlinkPlotPointFromEntity = async (plotPointId, entityType, entityId) => {
  try {
    const response = await axiosInstance.post(`/plotpoint/${plotPointId}/unlink`, {
      entityType,
      entityId,
    });
    return response.data;
  } catch (error) {
    console.error(
      `❌ Failed to unlink ${entityType} (${entityId}) from plot point ${plotPointId}:`,
      error.response?.data || error.message
    );
    throw error;
  }
};

export const deletePlotPoint = async (id) => {
  try {
    await axiosInstance.delete(`/plotpoint/${id}`);
  } catch (error) {
    console.error(`❌ Failed to delete plot point ${id}:`, error.response?.data || error.message);
    throw error;
  }
};

export const fetchForNewChapter = async (sourceChapterId) => {
  try {
    const response = await axiosInstance.get(`/plotpoint/from-chapter/${sourceChapterId}`);
    return response.data;
  } catch (error) {
    console.error('❌ Failed to fetch for new chapter:', error.response?.data || error.message);
    return [];
  }
};

export const updatePlotPoint = async (id, payload) => {
    try {
        const response = await axiosInstance.put(`/plotpoint/${id}`, payload);
        return response.data;
    } catch (error) {
        console.error(`❌ Failed to update plot point ${id}:`, error.response?.data || error.message);
        throw error;
    }
};
