//TODO
//CharacterEditModal.jsx
// Refactor to be reusable for other entities(Location, Event, etc.)
// Add dropdowns for Language, Location, Faction
// Pre - fill current values
// Save changes via updateCharacter
// Add "Create New Snapshot" button inside modal
// Add "Delete" button inside modal
// Add "Show Snapshots" button inside modal
// Add "Copy to Snapshot" button inside modal
// Add "Pin" button inside modal
// Add "Show All Related" button inside modal
// Add "Show All Snapshots" button inside modal
// Add "Show All Related Snapshots" button inside modal
// Add "Show PlotPoints for Current Snapshot Only" toggle inside modal
// Add "Show All Snapshots" toggle inside modal
// Add "Show All Related Snapshots" toggle inside modal
// Add "Show All Related Characters" toggle inside modal
// Add "Show All Related Locations" toggle inside modal
// Add "Show All Related Factions" toggle inside modal
// Add "Show All Related Events" toggle inside modal
// Add "Show All Related Items" toggle inside modal
// Add "Show All Related Rivers" toggle inside modal
// Add "Show All Related Routes" toggle inside modal
// Add "Show All Related Languages" toggle inside modal
// Add "Show All Related Eras" toggle inside modal
// Add "Show All Related PlotPoints" toggle inside modal
//add code to implement all te mentioned features



// src/components/CharacterEditModal.jsx
import React, { useState, useEffect } from 'react';
import '../styles/CharacterEditModal.css';

export default function CharacterEditModal({ character, onClose, onSave, factions, locations, languages }) {
    const [form, setForm] = useState({ ...character });

    useEffect(() => {
        setForm({ ...character });
    }, [character]);

    const handleChange = (field, value) => {
        setForm(prev => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        onSave(form);
    };

    return (
        <div className="modal-backdrop">
            <div className="modal">
                <h2>Edit Character</h2>

                <div className="form-grid">
                    <label>Personality:
                        <input value={form.personality || ''} onChange={e => handleChange('personality', e.target.value)} />
                    </label>
                    <label>Motivation:
                        <input value={form.motivation || ''} onChange={e => handleChange('motivation', e.target.value)} />
                    </label>
                    <label>Desire:
                        <input value={form.desire || ''} onChange={e => handleChange('desire', e.target.value)} />
                    </label>
                    <label>Fear:
                        <input value={form.fear || ''} onChange={e => handleChange('fear', e.target.value)} />
                    </label>
                    <label>Weakness:
                        <input value={form.weakness || ''} onChange={e => handleChange('weakness', e.target.value)} />
                    </label>
                    <label>Flaw:
                        <input value={form.flaw || ''} onChange={e => handleChange('flaw', e.target.value)} />
                    </label>
                    <label>Misbelief:
                        <input value={form.misbelief || ''} onChange={e => handleChange('misbelief', e.target.value)} />
                    </label>
                    <label>Faction:
                        <select value={form.factionId || ''} onChange={e => handleChange('factionId', parseInt(e.target.value))}>
                            <option value="">-- None --</option>
                            {factions.map(f => <option key={f.id} value={f.id}>{f.name}</option>)}
                        </select>
                    </label>
                    <label>Location:
                        <select value={form.locationId || ''} onChange={e => handleChange('locationId', parseInt(e.target.value))}>
                            <option value="">-- None --</option>
                            {locations.map(l => <option key={l.id} value={l.id}>{l.name}</option>)}
                        </select>
                    </label>
                    <label>Language:
                        <select value={form.languageId || ''} onChange={e => handleChange('languageId', parseInt(e.target.value))}>
                            <option value="">-- None --</option>
                            {languages.map(lang => <option key={lang.id} value={lang.id}>{lang.name}</option>)}
                        </select>
                    </label>
                </div>

                <div className="modal-actions">
                    <button onClick={handleSubmit}>Save</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
}
