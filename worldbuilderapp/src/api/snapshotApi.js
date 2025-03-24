import api from './axiosInstance';

export async function fetchSnapshots() {
    const res = await api.get('snapshot');
    return res.data;
}
