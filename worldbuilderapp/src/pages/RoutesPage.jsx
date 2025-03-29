// src/pages/RoutePage.jsx
import React, { useEffect, useState } from "react";
import {
    fetchRoutes,
    createRoute,
    //updateRoute,
    deleteRoute,
} from '../api/RouteApi';
export default function RoutePage() {
    const [routes, setRoutes] = useState([]);
    const [newRoute, setNewRoute] = useState({ name: "", startLocationId: null, endLocationId: null });

    useEffect(() => {
        loadRoutes();
    }, []);

    const loadRoutes = async () => {
        const data = await fetchRoutes();
        setRoutes(data);
    };

    const handleCreate = async () => {
        await createRoute(newRoute);
        setNewRoute({ name: "", startLocationId: null, endLocationId: null });
        loadRoutes();
    };

    const handleDelete = async (id) => {
        await deleteRoute(id);
        loadRoutes();
    };

    return (
        <div className="app-container">
            <h2>Routes</h2>

            <div className="card">
                <input
                    type="text"
                    value={newRoute.name}
                    placeholder="Route Name"
                    onChange={(e) => setNewRoute({ ...newRoute, name: e.target.value })}
                />
                <input
                    type="number"
                    placeholder="Start Location ID"
                    value={newRoute.startLocationId || ""}
                    onChange={(e) => setNewRoute({ ...newRoute, startLocationId: parseInt(e.target.value) })}
                />
                <input
                    type="number"
                    placeholder="End Location ID"
                    value={newRoute.endLocationId || ""}
                    onChange={(e) => setNewRoute({ ...newRoute, endLocationId: parseInt(e.target.value) })}
                />
                <button onClick={handleCreate}>Add Route</button>
            </div>

            <ul>
                {routes.map((route) => (
                    <li key={route.id}>
                        {route.name} (From: {route.startLocationId}, To: {route.endLocationId})
                        <button onClick={() => handleDelete(route.id)}>❌</button>
                    </li>
                ))}
            </ul>
        </div>
    );
}
