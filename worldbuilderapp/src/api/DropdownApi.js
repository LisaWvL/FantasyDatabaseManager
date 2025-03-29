import axiosInstance from "./axiosInstance";

export const fetchFactions = async () => {
    const res = await axiosInstance.get("/dropdown/factions");
    return res.data;
};

export const fetchLocations = async () => {
    const res = await axiosInstance.get("/dropdown/locations");
    return res.data;
};

export const fetchLanguages = async () => {
    const res = await axiosInstance.get("/dropdown/languages");
    return res.data;
};
export const fetchRivers = async () => {
    const res = await axiosInstance.get("/dropdown/rivers");
    return res.data;
};
export const fetchRoutes = async () => {
    const res = await axiosInstance.get("/dropdown/routes");
    return res.data;
};
export const fetchSnapshots = async () => {
    const res = await axiosInstance.get("/dropdown/snapshots");
    return res.data;
};
