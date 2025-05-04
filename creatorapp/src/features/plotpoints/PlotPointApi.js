import axiosInstance from '../../api/axiosInstance';

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
        const response = await axiosInstance.post('/plotpoint/create', payload);
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

export const getRelatedActs = async (bookId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${bookId}/relatedActs`);
        return response.data;
    } catch (error) {
        if (error.response?.status === 404) {
            console.warn(`⚠️ No chapters found for act ${bookId}`);
            return []; // ← Return an empty list if none found
        }
        console.error(`❌ Failed to fetch related acts for book ${bookId}:`, error.response?.data || error.message);
        throw error;
    }
};


export const getRelatedChapters = async (actId) => {
    try {
        const response = await axiosInstance.get(`/plotpoint/${actId}/relatedChapters`);
        return response.data;
    } catch (error) {
        if (error.response?.status === 404) {
            console.warn(`⚠️ No chapters found for act ${actId}`);
            return []; // ← Return an empty list if none found
        }
        console.error(`❌ Failed to fetch related chapters for act ${actId}:`, error.response?.data || error.message);
        throw error;
    }
};

export const getDashboardGrid = async () => {
    try {
        const { data } = await axiosInstance.get('/dashboard/getDashboardGrid');
        return data;
    } catch (error) {
        console.error('❌ Failed to load dashboard:', error.response?.data || error.message);
        throw error;
    }
};

export const getDashboardCards = async () => {
    try {
        const { data } = await axiosInstance.get('/cards/getDashboardCards');
        return data;
    } catch (error) {
        console.error('❌ Failed to load dashboard:', error.response?.data || error.message);
        throw error;
    }
};



