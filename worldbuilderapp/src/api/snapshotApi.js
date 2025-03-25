import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:5001/api', // ✅ Match the actual API port!
    withCredentials: false
});

export const fetchSnapshots = async () => {
    try {
        const response = await api.get('/snapshot');
        return response.data;
    } catch (error) {
        console.error('Failed to fetch snapshots:', error);
        throw error;
    }
};
