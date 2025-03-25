// src/api/axiosInstance.js
import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7210/api', // ✅ your backend port
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});

export default axiosInstance;
