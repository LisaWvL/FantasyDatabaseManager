// src/pages/CharacterPage.jsx
import React, { useEffect, useState } from "react";
import '../styles/CharacterPage.css';

import {
    fetchCharacters,
    deleteCharacter,
    updateCharacter,
} from '../api/CharacterApi';

import {
    fetchFactions,
    fetchLocations,
    fetchLanguages
} from '../api/DropdownApi';

import { fetchChapters } from '../api/ChapterApi';

import CharacterCard from "../components/CharacterCard";
import CharacterEditModal from "../modals/CharacterEditModal";
import "../styles/App.css";

export default function CharacterPage() {
    const [characters, setCharacters] = useState([]);
    const [chapters, setChapters] = useState([]);
    const [editingCharacter, setEditingCharacter] = useState(null);
    const [factions, setFactions] = useState([]);
    const [locations, setLocations] = useState([]);
    const [languages, setLanguages] = useState([]);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const [characterData, chapterData, factionsData, locationsData, languagesData] = await Promise.all([
                fetchCharacters(),
                fetchChapters(),
                fetchFactions(),
                fetchLocations(),
                fetchLanguages()
            ]);

            setCharacters(characterData);
            setChapters(chapterData);
            setFactions(factionsData);
            setLocations(locationsData);
            setLanguages(languagesData);
        } catch (err) {
            console.error("❌ Failed to load data:", err);
        }
    };

    const handleDelete = async (id) => {
        if (confirm("Delete this character?")) {
            await deleteCharacter(id);
            await loadData();
        }
    };

    const handleEdit = (char) => {
        setEditingCharacter(char);
    };

    const handleModalClose = () => {
        setEditingCharacter(null);
    };

    const handleModalSave = async (updatedChar) => {
        await updateCharacter(updatedChar.id, updatedChar);
        await loadData();
        setEditingCharacter(null);
    };

    // 🧠 Group characters by chapter ID
    const charactersByChapter = characters.reduce((acc, char) => {
        if (!char.chapterId) return acc;
        if (!acc[char.chapterId]) acc[char.chapterId] = [];
        acc[char.chapterId].push(char);
        return acc;
    }, {});

    // 📘 Filter chapters that have visible content
    const relevantChapters = chapters.filter(c =>
        c.chapterTitle || c.chapterText || c.chapterSummary || c.toDo
    );

    return (
        <div className="character-grid">
            {relevantChapters.map(chap => (
                <section key={chap.id} className="chapter-section">
                    <h2 className="chapter-title">
                        Book {chap.bookNumber} • Act {chap.actNumber} • Chapter {chap.chapterNumber}: {chap.chapterTitle}
                    </h2>
                    <div className="chapter-card-list">
                        {(charactersByChapter[chap.id] || []).map(char => (
                            <React.Fragment key={char.id}>
                                <CharacterCard
                                    chapters={(charactersByChapter[chap.id] || []).filter(c => c.name === char.name)}
                                    onEdit={() => handleEdit(char)}
                                    onDelete={() => handleDelete(char.id)}
                                />
                                {editingCharacter?.id === char.id && (
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
                </section>
            ))}
        </div>
    );
}
