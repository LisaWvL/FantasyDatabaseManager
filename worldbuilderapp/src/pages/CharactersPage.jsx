""// src/pages/CharacterPage.jsx
import React, { useEffect, useState } from "react";
import {
    fetchCharacters,
    deleteCharacter,
    duplicateCharacter,
    updateCharacter,
    // ✅ Only import the actual API function, not handleEdit
} from '../api/CharacterApi';
import CharacterEditModal from "../modals/CharacterEditModal.jsx";

import {
    fetchFactions,
    fetchLocations,
    fetchLanguages
} from '../api/DropdownApi.js'; // 👈 Add this line

import CharacterCard from "../components/CharacterCard";
import "../styles/App.css";

export default function CharacterPage() {
    const [characters, setCharacters] = useState([]);

    // 🔁 Load characters once on mount
    useEffect(() => {
        loadCharacters();
    }, []);

    const loadCharacters = async () => {
        try {
            const data = await fetchCharacters();
            setCharacters(data);
        } catch (err) {
            console.error("❌ Failed to load characters", err);
        }
    };

    const handleDelete = async (id) => {
        if (confirm("Delete this character?")) {
            await deleteCharacter(id);
            await loadCharacters();
        }
    };

    const handleDuplicate = async (id) => {
        const result = await duplicateCharacter(id);
        console.log("📸 New snapshot entity created:", result);
        await loadCharacters();
    };

    // Inside CharacterPage.jsx
    const [editingCharacter, setEditingCharacter] = useState(null);

    const handleEdit = (char) => {
        setEditingCharacter(char);
    };

    const handleModalClose = () => {
        setEditingCharacter(null);
    };

    const handleModalSave = async (updatedChar) => {
        await updateCharacter(updatedChar.id, updatedChar);
        await loadCharacters();
        setEditingCharacter(null);
    };


    // 🧠 Group snapshots by Name
    const groupedSnapshots = characters.reduce((acc, char) => {
        if (!char.name) return acc;
        if (!acc[char.name]) acc[char.name] = [];
        acc[char.name].push(char);
        return acc;
    }, {});


    const [factions, setFactions] = useState([]);
    const [locations, setLocations] = useState([]);
    const [languages, setLanguages] = useState([]);

    useEffect(() => {
        loadCharacters();
        loadDropdowns();
    }, []);

    const loadDropdowns = async () => {
        try {
            const [factionsData, locationsData, languagesData] = await Promise.all([
                fetchFactions(),
                fetchLocations(),
                fetchLanguages()
            ]);
            setFactions(factionsData);
            setLocations(locationsData);
            setLanguages(languagesData);
        } catch (err) {
            console.error("❌ Failed to load dropdowns", err);
        }
    };

    return (
        <div className="character-grid">
            {Object.entries(groupedSnapshots).map(([name, versions]) => (
                <React.Fragment key={name}>
                    <CharacterCard
                        character={versions[0]} // any version to use as anchor
                        snapshots={versions}
                        onEdit={() => handleEdit(versions[0])}
                        onDelete={() => handleDelete(versions[0].id)}
                        onCreateSnapshot={() => handleDuplicate(versions[0].id)}
                    />
                    {editingCharacter?.id === versions[0].id && (
                        <CharacterEditModal
                            character={editingCharacter}
                            onClose={handleModalClose}
                            onSave={handleModalSave}
                            factions={factions}
                            locations={locations}
                            languages={languages}
                        />
                    )}
                </React.Fragment>
            ))}
        </div>
    );
}
