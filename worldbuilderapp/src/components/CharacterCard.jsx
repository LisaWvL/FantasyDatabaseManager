//TODO
//CharacterCard.jsx
// Context menu options: “Show All Related”, “Pin”, “Copy to Snapshot”
// Refactor into generic EntityCard with props for different entity types
// Add snapshot creation functionality
// Add snapshot comparison functionality
// Add snapshot diff display
// Add snapshot deletion functionality
// Add snapshot pinning functionality


// src/components/CharacterCard.jsx
import React, { useState } from 'react';
import '../styles/CharacterCard.css';


export default function CharacterCard({ snapshots, onEdit, onDelete, onCreateSnapshot }) {
    const [showDetails, setShowDetails] = useState(false);
    const [showSnapshots, setShowSnapshots] = useState(false);

    // The base snapshot is the newest one (highest Book, Act, Chapter)
    const sortedSnapshots = [...snapshots].sort((a, b) => {
        if (b.book !== a.book) return b.book - a.book;
        if (b.act !== a.act) return b.act - a.act;
        return b.chapter - a.chapter;
    });

    const baseSnapshot = sortedSnapshots[0];
    const extraSnapshots = sortedSnapshots.slice(1);

    const toggleDetails = () => setShowDetails(prev => !prev);
    const toggleSnapshots = () => setShowSnapshots(prev => !prev);

    return (
        <div className={`character-card ${showSnapshots ? 'with-snapshots' : ''}`}>
            <div className="card-header">
                {extraSnapshots.length > 0 && (
                    <button className="snapshot-toggle" onClick={toggleSnapshots}>
                        {showSnapshots ? '▼' : '▶'}
                    </button>
                )}
                <h3>{baseSnapshot.name}</h3>
                <div className="card-actions">
                    <button onClick={() => onCreateSnapshot(baseSnapshot)}>📸</button>
                    <button onClick={() => onEdit(baseSnapshot)}>✏️</button>
                    <button onClick={() => onDelete(baseSnapshot)}>🗑️</button>
                </div>
            </div>

            <div className="card-summary">
                <div className="attribute-box"><span className="label">Role: </span><span className="value">{baseSnapshot.role}</span></div>
                <div className="attribute-box"><span className="label">Faction: </span><span className="value">{baseSnapshot.factionName}</span></div>
                <div className="attribute-box"><span className="label">Location: </span><span className="value">{baseSnapshot.locationName}</span></div>
                <div className="attribute-box"><span className="label">Personality: </span><span className="value">{baseSnapshot.personality}</span></div>
                <div className="attribute-box"><span className="label">Motivation: </span><span className="value">{baseSnapshot.motivation}</span></div>
                <div className="attribute-box"><span className="label">Desire: </span><span className="value">{baseSnapshot.desire}</span></div>
                <div className="attribute-box"><span className="label">Fear: </span><span className="value">{baseSnapshot.fear}</span></div>
                <div className="attribute-box"><span className="label">Weakness: </span><span className="value">{baseSnapshot.weakness}</span></div>
                <div className="attribute-box"><span className="label">Flaw: </span><span className="value">{baseSnapshot.flaw}</span></div>
                <div className="attribute-box"><span className="label">Misbelief: </span><span className="value">{baseSnapshot.misbelief}</span></div>

                <button className="details-toggle" onClick={toggleDetails}>
                    {showDetails ? 'Hide Details' : 'Show More'}
                </button>
            </div>

            {showDetails && (
                <div className="card-details">
                    <div className="attribute-box"><span className="label">Alias: </span><span className="value">{baseSnapshot.alias}</span></div>
                    <div className="attribute-box"><span className="label">Birth: </span><span className="value">{baseSnapshot.birthDay} {baseSnapshot.birthMonth} {baseSnapshot.birthYear}</span></div>
                    <div className="attribute-box"><span className="label">Gender: </span><span className="value">{baseSnapshot.gender}</span></div>
                    <div className="attribute-box"><span className="label">Height: </span><span className="value">{baseSnapshot.heightCm} cm</span></div>
                    <div className="attribute-box"><span className="label">Build: </span><span className="value">{baseSnapshot.build}</span></div>
                    <div className="attribute-box"><span className="label">Hair: </span><span className="value">{baseSnapshot.hair}</span></div>
                    <div className="attribute-box"><span className="label">Eyes: </span><span className="value">{baseSnapshot.eyes}</span></div>
                    <div className="attribute-box"><span className="label">Defining Features: </span><span className="value">{baseSnapshot.definingFeatures}</span></div>
                    <div className="attribute-box"><span className="label">Magic: </span><span className="value">{baseSnapshot.magic}</span></div>
                    <div className="attribute-box"><span className="label">Social Status: </span><span className="value">{baseSnapshot.socialStatus}</span></div>
                    <div className="attribute-box"><span className="label">Occupation: </span><span className="value">{baseSnapshot.occupation}</span></div>
                    <div className="attribute-box"><span className="label">Language: </span><span className="value">{baseSnapshot.languageName}</span></div>
                </div>
            )}


            {showSnapshots && extraSnapshots.length > 0 && (
                <div className="snapshot-diff-container">
                    <h4>Snapshot diffs</h4>
                    <div className="snapshot-diff-list">
                        {extraSnapshots.map((snap, index) => {
                            const diffKeys = Object.keys(snap).filter(
                                key => snap[key] !== baseSnapshot[key] && !['id', 'snapshotId'].includes(key)
                            );

                            return (
                                <div key={index} className="snapshot-card">
                                    <h5>Version: Book {snap.book}, Act {snap.act}, Chapter {snap.chapter}</h5>
                                    {diffKeys.map((key) => (
                                        <p key={key}>
                                            <strong>{key}:</strong> {snap[key]}
                                        </p>
                                    ))}
                                </div>
                            );
                        })}
                    </div>
                </div>
            )}
        </div>
    );
}
