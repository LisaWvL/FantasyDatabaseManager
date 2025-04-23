import axiosInstance from './axiosInstance';

export const fetchFactions = async () => {
    const res = await axiosInstance.get('/dropdown/factions');
    return res.data;
};

export const fetchLocations = async () => {
    const res = await axiosInstance.get('/dropdown/locations');
    return res.data;
};

export const fetchLanguages = async () => {
    const res = await axiosInstance.get('/dropdown/languages');
    return res.data;
};
export const fetchRivers = async () => {
    const res = await axiosInstance.get('/dropdown/rivers');
    return res.data;
};
export const fetchRoutes = async () => {
    const res = await axiosInstance.get('/dropdown/routes');
    return res.data;
};
export const fetchChapters = async () => {
    const res = await axiosInstance.get('/dropdown/chapters');
    return res.data;
};
export const fetchBooks = async () => {
    const res = await axiosInstance.get('/dropdown/books');
    return res.data;
};
export const fetchActs = async () => {
    const res = await axiosInstance.get('/dropdown/acts');
    return res.data;
};
