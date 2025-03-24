import React from "react";

export default function EntitySelector({ entityTypes, selectedEntity, onChange }) {
    return (
        <div className="form-group mb-3">
            <label><strong>Select Entity Type:</strong></label>
            <select className="form-select" value={selectedEntity} onChange={e => onChange(e.target.value)}>
                <option value="">-- Select Entity --</option>
                {entityTypes.map(e => (
                    <option key={e} value={e}>{e}</option>
                ))}
            </select>
        </div>
    );
}
