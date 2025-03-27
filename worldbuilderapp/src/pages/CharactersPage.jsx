import React, { useEffect, useState } from "react";
import {
    fetchCharacters,
    deleteCharacter,
    duplicateCharacter,
} from "../api/characterApi";
import "../styles/App.css";

export default function CharacterPage() {
    const [characters, setCharacters] = useState([]);

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
        console.log("New snapshot entity created:", result);
        await loadCharacters();
    };

    return (
        <div className="main-content">
            <h2>Characters</h2>
            {characters.length === 0 ? (
                <p>No characters found.</p>
            ) : (
                <table className="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Faction</th>
                            <th>Language</th>
                            <th>Location</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {characters.map((char) => (
                            <tr key={char.id}>
                                <td>{char.name}</td>
                                <td>{char.factionName}</td>
                                <td>{char.languageName}</td>
                                <td>{char.locationName}</td>
                                <td>
                                    <button className="btn btn-sm btn-warning me-1" onClick={() => handleDuplicate(char.id)}>Snapshot</button>
                                    <button className="btn btn-sm btn-danger" onClick={() => handleDelete(char.id)}>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}
