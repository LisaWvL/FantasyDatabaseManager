// src/pages/CharacterRelationshipPage.jsx
import React, { useEffect, useState } from 'react';
import {
    fetchCharacterRelationships,
    deleteCharacterRelationship,
    getNewSnapshotViewModel
} from '../api/CharacterRelationshipApi';
import EntityTable from '../components/EntityTable';

export default function CharacterRelationshipPage() {
    const [relationships, setRelationships] = useState([]);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await fetchCharacterRelationships();
            setRelationships(data);
        } catch (err) {
            console.error('Failed to load character relationships', err);
        }
    };

    const handleEdit = (id) => {
        alert(`✏️ Edit relationship ${id} – Open edit form here`);
    };

    const handleDelete = async (id) => {
        if (confirm('❌ Are you sure you want to delete this relationship?')) {
            await deleteCharacterRelationship(id);
            setRelationships(prev => prev.filter(r => r.id !== id));
        }
    };

    const handleSnapshot = async (id) => {
        const vm = await getNewSnapshotViewModel(id);
        console.log('New snapshot view model:', vm);
        alert(`Open snapshot form for relationship ${id}`);
    };

    return (
        <div>
            <h2>Character Relationships</h2>
            <EntityTable
                data={relationships}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onSnapshot={handleSnapshot}
            />
        </div>
    );
}
