// src/pages/LocationPage.jsx
import React, { useEffect, useState } from 'react';
import {
    fetchLocations,
    //createLocation,
    //updateLocation,
    deleteLocation,
    fetchLocationSnapshot
} from '../api/locationApi';
import EntityTable from '../components/EntityTable';

export default function LocationPage() {
    const [locations, setLocations] = useState([]);

    const load = async () => {
        try {
            const data = await fetchLocations();
            setLocations(data);
        } catch (err) {
            console.error('Failed to load locations:', err);
        }
    };

    useEffect(() => {
        load();
    }, []);

    const handleEdit = (id) => {
        alert(`Edit location ${id}`);
        // TODO: open form or modal
    };

    const handleDelete = async (id) => {
        if (confirm('Delete this location?')) {
            await deleteLocation(id);
            await load();
        }
    };

    const handleSnapshot = async (id) => {
        const snapshotDraft = await fetchLocationSnapshot(id);
        console.log('New location snapshot draft:', snapshotDraft);
        // TODO: Open snapshot editing view
    };

    return (
        <div className="app-container">
            <h2>Locations</h2>
            <EntityTable
                data={locations}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onSnapshot={handleSnapshot}
            />
        </div>
    );
}
