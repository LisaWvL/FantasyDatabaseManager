//This is the Character card, it displays one character version per character,
//multiple versions of a character can exist, but should be sorted into different chapers
// and acts, so that the same character can be in different chapters and acts
//clicking edit sets the card into edit mode, leaving it triggers save on blur
//creating a new version of the character creates a new character with the same values as the current one as its own card which is set in edit mode
//the user can then edit the attributes and save them, which creates a new character in the database
//deleting a character deletes the character from the database
//the user can update the ChapterId of the Character, by dragging and dropping the card into a different section of the page

import React, { useState, useEffect } from "react";
import { fetchFactionById } from "../features/factions/FactionApi";
import { fetchLocationById } from "../features/locations/LocationApi";
import { fetchLanguageById } from "../features/languages/LanguageApi";
import { fetchFactions, fetchLocations, fetchLanguages } from "../api/DropdownApi";
import TooltipLink from "../utils/TooltipLink";

import "../styles/CharacterCard.css";
import "../styles/TooltipLink.css";

export default function CharacterCard({
    character,
    onCreateNewVersion,
    onDelete,
    onUpdate
}) {
    const [isEditMode, setIsEditMode] = useState(character.isNew || character.isEditMode || false);
    const [localCharacter, setLocalCharacter] = useState({ ...character });

    const [faction, setFaction] = useState(null);
    const [location, setLocation] = useState(null);
    const [language, setLanguage] = useState(null);

    const [factions, setFactions] = useState([]);
    const [locations, setLocations] = useState([]);
    const [languages, setLanguages] = useState([]);

    useEffect(() => {
        fetchFactions().then(setFactions);
        fetchLocations().then(setLocations);
        fetchLanguages().then(setLanguages);

        if (character.factionId) fetchFactionById(character.factionId).then(setFaction);
        if (character.locationId) fetchLocationById(character.locationId).then(setLocation);
        if (character.languageId) fetchLanguageById(character.languageId).then(setLanguage);
    }, [character]);

    const handleBlur = () => {
        if (isEditMode) {
            onUpdate(localCharacter);
            setIsEditMode(false);
        }
    };

    useEffect(() => {
        const handleKeyDown = (e) => {
            if (e.key === "Escape") {
                setLocalCharacter({ ...character }); // revert
                setIsEditMode(false);
            }
        };

        if (isEditMode) {
            window.addEventListener("keydown", handleKeyDown);
        }

        return () => {
            window.removeEventListener("keydown", handleKeyDown);
        };
    }, [isEditMode, character]);

    const handleChange = (field, value) => {
        setLocalCharacter(prev => ({ ...prev, [field]: value }));
    };

    return (
        <div className="character-card" draggable onDragStart={(e) => e.dataTransfer.setData("characterId", character.id)}>

            <div className="card-header">
                <div className="card-actions">
                    <button onClick={() => onCreateNewVersion(character)}>➕</button>
                    <button onClick={() => setIsEditMode(true)}>✏️</button>
                    <button onClick={() => onDelete(character)}>🗑️</button>
                </div>
                <hr />
                <div>
                    <h3>{localCharacter.name}{localCharacter.alias && ` (${localCharacter.alias})`}</h3>
                    <h4>{localCharacter.role} • {localCharacter.magic}</h4>
                </div>
            </div>

            <div className="card-summary">
                {/* Faction Dropdown or Tooltip */}
                {isEditMode ? (
                    <div className="attribute-box">
                        <span className="label">Faction:</span>
                        <select
                            value={localCharacter.factionId || ""}
                            onChange={(e) => handleChange("factionId", parseInt(e.target.value))}
                            onBlur={handleBlur}
                        >
                            <option value="">Select...</option>
                            {factions.map(f => (
                                <option key={f.id} value={f.id}>
                                    {f.name}{f.alias ? ` (${f.alias})` : ""}
                                </option>
                            ))}
                        </select>
                    </div>
                ) : (
                    <TooltipLink
                        entityType="Faction"
                        entityId={localCharacter.factionId}
                        displayValue={
                            faction ? `${faction.name}${faction.alias ? ` (${faction.alias})` : ""}` : "(Faction)"
                        }
                    />
                )}
                {/* Location Dropdown or Tooltip */}
                {isEditMode ? (
                    <div className="attribute-box">
                        <span className="label">Location:</span>
                        <select
                            value={localCharacter.locationId || ""}
                            onChange={(e) => handleChange("locationId", parseInt(e.target.value))}
                            onBlur={handleBlur}
                        >
                            <option value="">Select...</option>
                            {locations.map(l => (
                                <option key={l.id} value={l.id}>{l.name} ({l.type})</option>
                            ))}
                        </select>
                    </div>
                ) : (
                    <TooltipLink
                        entityType="Location"
                        entityId={localCharacter.locationId}
                        displayValue={
                            location ? `${location.name}${location.type ? ` (${location.type})` : ""}` : "(Location)"
                        }
                    />
                )}
                {/* Language Dropdown or Tooltip */}
                {isEditMode ? (
                    <div className="attribute-box">
                        <span className="label">Language:</span>
                        <select
                            value={localCharacter.languageId || ""}
                            onChange={(e) => handleChange("languageId", parseInt(e.target.value))}
                            onBlur={handleBlur}
                        >
                            <option value="">Select...</option>
                            {languages.map(l => (
                                <option key={l.id} value={l.id}>{l.name} ({l.type})</option>
                            ))}
                        </select>
                    </div>
                ) : (
                    <TooltipLink
                        entityType="Language"
                        entityId={localCharacter.languageId}
                        displayValue={
                            language ? `${language.name}${language.type ? ` (${language.type})` : ""}` : "(Language)"
                        }
                    />
                )}
                {/* Editable textarea fields */}
                {["personality", "motivation", "desire", "fear", "weakness", "flaw", "misbelief"].map(field => (
                    <div key={field} className="attribute-box">
                        <span className="label">{field.charAt(0).toUpperCase() + field.slice(1)}:</span>
                        {isEditMode ? (
                            <textarea
                                className="edit-field"
                                value={localCharacter[field] || ""}
                                onChange={(e) => handleChange(field, e.target.value)}
                                onBlur={handleBlur}
                            />
                        ) : (
                            <span className="value">{localCharacter[field]}</span>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
}
