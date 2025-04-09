// src/components/CharacterCard.jsx
import React, { useState, useEffect } from "react";
import { fetchFactionById } from "../api/FactionApi";
import { fetchLocationById } from "../api/LocationApi";
import { fetchLanguageById } from "../api/LanguageApi";
import { fetchFactions, fetchLocations, fetchLanguages } from "../api/DropdownApi"; // Assuming these are used for dropdowns
import { createCharacter, updateCharacter, deleteCharacter } from "../api/CharacterApi"; // Import APIs

export default function CharacterCard({ chapters, onSaveNewVersion, onDelete }) {
    const [showDetails, setShowDetails] = useState(false);
    const [showVersions, setShowVersions] = useState(false);
    const [faction, setFaction] = useState(null);
    const [location, setLocation] = useState(null);
    const [language, setLanguage] = useState(null);
    const [factionsList, setFactionsList] = useState([]);
    const [locationsList, setLocationsList] = useState([]);
    const [languagesList, setLanguagesList] = useState([]);
    const [isEditMode, setIsEditMode] = useState(false);

    const sortedChapters = [...chapters].sort((a, b) => {
        const bBook = b.BookNumber || 0;
        const aBook = a.BookNumber || 0;
        if (bBook !== aBook) return bBook - aBook;

        const bAct = b.ActNumber || 0;
        const aAct = a.ActNumber || 0;
        if (bAct !== aAct) return bAct - aAct;

        return (b.ChapterNumber || 0) - (a.ChapterNumber || 0);
    });

    const baseCharacter = sortedChapters[0];
    const versions = sortedChapters.slice(1);

    useEffect(() => {
        if (baseCharacter?.FactionId) fetchFactionById(baseCharacter.FactionId).then(setFaction);
        if (baseCharacter?.LocationId) fetchLocationById(baseCharacter.LocationId).then(setLocation);
        if (baseCharacter?.LanguageId) fetchLanguageById(baseCharacter.LanguageId).then(setLanguage);
        loadDropdowns();
    }, [baseCharacter]);

    const toggleDetails = () => setShowDetails(prev => !prev);
    const toggleVersions = () => setShowVersions(prev => !prev);

    const loadDropdowns = async () => {
        try {
            const [factionsData, locationsData, languagesData] = await Promise.all([
                fetchFactions(),
                fetchLocations(),
                fetchLanguages(),
            ]);
            setFactionsList(factionsData);
            setLocationsList(locationsData);
            setLanguagesList(languagesData);
        } catch (err) {
            console.error("❌ Failed to load dropdowns", err);
        }
    };

    // Handle creating a new version
    const handleCreateNewVersion = async () => {
        const newCharacter = {
            ...baseCharacter,
            id: undefined, // Set ID to undefined to indicate this is a new version (database will handle the ID)
            ChapterId: baseCharacter.ChapterId, // Retain ChapterId for the new version
            isBaseCharacter: false, // Mark this as a new version of the character
        };

        try {
            await createCharacter(newCharacter); // Save the new version
            setIsEditMode(false); // Exit edit mode after saving
            if (onSaveNewVersion) onSaveNewVersion(newCharacter); // Optionally call the parent function to handle saved version
        } catch (err) {
            console.error("❌ Failed to create character version", err);
        }
    };

    // Handle updating an existing character
    const handleUpdateCharacter = async () => {
        const updatedCharacter = {
            ...baseCharacter,
            // Include any changes made in edit mode here (e.g., changes to faction, location, language, etc.)
        };

        try {
            await updateCharacter(baseCharacter.Id, updatedCharacter); // Update the character via API
            setIsEditMode(false); // Exit edit mode after saving
        } catch (err) {
            console.error("❌ Failed to update character", err);
        }
    };

    // Handle deleting a character
    const handleDelete = async () => {
        if (window.confirm("Are you sure you want to delete this character?")) {
            try {
                await deleteCharacter(baseCharacter.Id); // Delete character
                if (onDelete) onDelete(baseCharacter.Id); // Optionally call the parent function to handle character deletion
            } catch (err) {
                console.error("❌ Failed to delete character", err);
            }
        }
    };

    // Group versions by chapter
    const groupedByChapter = versions.reduce((acc, version) => {
        const key = `${version.BookNumber}-${version.ActNumber}-${version.ChapterNumber}`;
        if (!acc[key]) acc[key] = [];
        acc[key].push(version);
        return acc;
    }, {});

    return (
        <div className="character-card">
            <div className="card-header">
                {versions.length > 0 && (
                    <button className="chapter-toggle" onClick={toggleVersions}>
                        {showVersions ? "▼" : "▶"}
                    </button>
                )}
                <h3>{baseCharacter.Name} {baseCharacter.Alias && `(${baseCharacter.Alias})`}</h3>
                <div className="card-actions">
                    <button onClick={() => setIsEditMode(true)}>✏️ Edit</button>
                    <button onClick={handleCreateNewVersion}>➕ Create New Version</button>
                    <button onClick={handleDelete}>🗑️ Delete</button>
                </div>
            </div>

            <div className="card-summary">
                <div className="attribute-box">
                    Role: <span>{baseCharacter.Role}</span>
                </div>
                <div className="attribute-box">
                    Magic: <span>{baseCharacter.Magic}</span>
                </div>
                <div className="attribute-box">
                    Personality: <span>{baseCharacter.Personality}</span>
                </div>

                <div className="attribute-box linked">
                    Faction:{" "}
                    {isEditMode ? (
                        <select
                            value={faction?.Id || ""}
                            onChange={(e) => setFaction(factionsList.find((f) => f.Id === e.target.value))}
                            onBlur={handleUpdateCharacter} // Trigger update on blur (when the user finishes editing)
                        >
                            {factionsList.map((faction) => (
                                <option key={faction.Id} value={faction.Id}>
                                    {faction.Name}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <span title={faction?.Description || ""}>{faction?.Name || baseCharacter.FactionName}</span>
                    )}
                </div>

                <div className="attribute-box linked">
                    Location:{" "}
                    {isEditMode ? (
                        <select
                            value={location?.Id || ""}
                            onChange={(e) => setLocation(locationsList.find((l) => l.Id === e.target.value))}
                            onBlur={handleUpdateCharacter} // Trigger update on blur (when the user finishes editing)
                        >
                            {locationsList.map((location) => (
                                <option key={location.Id} value={location.Id}>
                                    {location.Name}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <span title={location?.Description || ""}>{location?.Name || baseCharacter.LocationName}</span>
                    )}
                </div>

                <div className="attribute-box linked">
                    Language:{" "}
                    {isEditMode ? (
                        <select
                            value={language?.Id || ""}
                            onChange={(e) => setLanguage(languagesList.find((l) => l.Id === e.target.value))}
                            onBlur={handleUpdateCharacter} // Trigger update on blur (when the user finishes editing)
                        >
                            {languagesList.map((language) => (
                                <option key={language.Id} value={language.Id}>
                                    {language.Name}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <span title={language?.Description || ""}>{language?.Name || baseCharacter.LanguageName}</span>
                    )}
                </div>

                <button className="details-toggle" onClick={toggleDetails}>
                    {showDetails ? "Hide Details" : "Show More"}
                </button>
            </div>

            {showDetails && (
                <div className="card-details">
                    {[["Birth", `${baseCharacter.BirthDay} ${baseCharacter.BirthMonth} ${baseCharacter.BirthYear}`],
                    ["Gender", baseCharacter.Gender],
                    ["Height", `${baseCharacter.HeightCm} cm`],
                    ["Build", baseCharacter.Build],
                    ["Hair", baseCharacter.Hair],
                    ["Eyes", baseCharacter.Eyes],
                    ["Defining Features", baseCharacter.DefiningFeatures],
                    ["Social Status", baseCharacter.SocialStatus],
                    ["Occupation", baseCharacter.Occupation],
                    ["Motivation", baseCharacter.Motivation],
                    ["Desire", baseCharacter.Desire],
                    ["Fear", baseCharacter.Fear],
                    ["Weakness", baseCharacter.Weakness],
                    ["Flaw", baseCharacter.Flaw],
                    ["Misbelief", baseCharacter.Misbelief],].map(([label, value], i) => (
                        <div key={i} className="attribute-box">
                            {label}: <span>{value}</span>
                        </div>
                    ))}
                </div>
            )}
            {showVersions && (
                <div className="chapter-diff-container">
                    <h4>Version Differences</h4>
                    <div className="chapter-diff-list">
                        {Object.entries(groupedByChapter).map(([chapterKey, entries]) => {
                            const { BookNumber, ActNumber, ChapterNumber } = entries[0];
                            return (
                                <div key={chapterKey} className="chapter-group">
                                    <h5>📘 Book {BookNumber} • 🗂 Act {ActNumber} • 📄 Chapter {ChapterNumber}</h5>
                                    {entries.map((snap, i) => {
                                        const diff = Object.keys(snap).filter(
                                            (key) => snap[key] !== baseCharacter[key] && !["Id", "ChapterId"].includes(key)
                                        );
                                        return (
                                            <div key={i} className="chapter-card">
                                                {diff.map((key) => (
                                                    <p key={key}>
                                                        <strong>{key}:</strong> {snap[key]}
                                                    </p>
                                                ))}
                                            </div>
                                        );
                                    })}
                                </div>
                            );
                        })}
                    </div>
                </div>
            )}
        </div>
    );
}