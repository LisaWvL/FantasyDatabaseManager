console.log("Hello World!")// src/api/axiosInstance.js
import axios from "axios";

export default axios.create({
    baseURL: "https://localhost:63752/api/",
    headers: {
        "Content-Type": "application/json"
    },
    withCredentials: true
});
