// src/pages/FactionPage.jsx
import React, { useEffect, useState } from 'react';
import {
    fetchFactions,
    deleteFaction,
    fetchNewChapter
} from '../api/FactionApi';

export default function FactionPage() {
    const [factions, setFactions] = useState([]);

    useEffect(() => {
        loadFactions();
    }, []);

    const loadFactions = async () => {
        try {
            const data = await fetchFactions();
            setFactions(data);
        } catch (error) {
            console.error('Failed to load factions:', error);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this faction?')) {
            await deleteFaction(id);
            loadFactions();
        }
    };

    const handleChapter = async (id) => {
        const chapter = await fetchNewChapter(id);
        console.log('New chapter prepared:', chapter);
        // Navigate or display chapter editor here
    };

    return (
        <div className="entity-page">
            <h2>Factions</h2>
            <table className="table table-dark table-striped">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Leader</th>
                        <th>HQ Location</th>
                        <th>Chapter</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {factions.map(f => (
                        <tr key={f.id}>
                            <td>{f.name}</td>
                            <td>{f.leaderName}</td>
                            <td>{f.hqLocationName}</td>
                            <td>{f.chapterName}</td>
                            <td>
                                <button className="btn btn-sm btn-warning me-2" onClick={() => handleChapter(f.id)}>Chapter</button>
                                <button className="btn btn-sm btn-danger" onClick={() => handleDelete(f.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
