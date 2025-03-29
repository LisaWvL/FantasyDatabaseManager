//TODO
//SnapshotSelector.jsx
// Enable timeline grouping(show Book > Act > Chapter visually)
// Add search and sort capabilities
// Add �Create New Snapshot� button
// Add �Delete Snapshot� button
// Add �Duplicate Snapshot� button
// Add �Edit Snapshot� button
// Add �Show All Related� button
// Add �Copy to Snapshot� button
// Add �Pin� button
// Add �Show PlotPoints for Current Snapshot Only� toggle



// src/components/SnapshotSelector.jsx
import React, { useEffect, useState } from 'react';
import { fetchSnapshots } from '../api/SnapshotApi';
import { useSnapshot } from '../context/snapshotContext';

const SnapshotSelector = () => {
    const [snapshots, setSnapshots] = useState([]);
    const { currentSnapshotId, setCurrentSnapshotId, setSnapshotName } = useSnapshot();

    useEffect(() => {
        const loadSnapshots = async () => {
            try {
                const data = await fetchSnapshots();
                setSnapshots(data);
            } catch (error) {
                console.error("Failed to fetch snapshots:", error);
            }
        };
        loadSnapshots();
    }, []);


    const handleChange = (e) => {
        const selectedId = parseInt(e.target.value);
        const selectedSnapshot = snapshots.find(s => s.id === selectedId);
        setCurrentSnapshotId(selectedId);
        setSnapshotName(selectedSnapshot?.snapshotName || 'Unknown');
    };

    return (
        <div className="snapshot-selector">
            <label>Select Snapshot:</label>
            <select onChange={handleChange} value={currentSnapshotId || ''}>
                <option value="">-- Choose Snapshot --</option>
                {snapshots.map(s => (
                    <option key={s.id} value={s.id}>{s.snapshotName}</option>
                ))}
            </select>
        </div>
    );
};

export default SnapshotSelector;
