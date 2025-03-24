import React from "react";

export default function SnapshotSelector({ snapshots, selectedSnapshot, onChange }) {
    return (
        <div className="form-group mb-3">
            <label><strong>Select Snapshot:</strong></label>
            <select className="form-select" value={selectedSnapshot} onChange={e => onChange(e.target.value)}>
                <option value="">-- Select Snapshot --</option>
                {snapshots.map(s => (
                    <option key={s.id} value={s.id}>{s.snapshotName}</option>
                ))}
            </select>
        </div>
    );
}
