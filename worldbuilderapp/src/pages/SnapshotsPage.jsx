// src/pages/SnapshotPage.jsx
import React, { useEffect, useState } from "react";
import {
    fetchSnapshots,
    createSnapshot,
    //updateSnapshot,
    deleteSnapshot,
    duplicateSnapshot,
} from '../api/SnapshotApi';

export default function SnapshotPage() {
    const [snapshots, setSnapshots] = useState([]);
    const [newSnapshot, setNewSnapshot] = useState({ book: "", act: "", chapter: "", snapshotName: "" });

    useEffect(() => {
        loadSnapshots();
    }, []);

    const loadSnapshots = async () => {
        try {
            const data = await fetchSnapshots();
            setSnapshots(data);
        } catch (error) {
            console.error("Failed to load snapshots", error);
        }
    };

    const handleCreate = async () => {
        try {
            await createSnapshot(newSnapshot);
            setNewSnapshot({ book: "", act: "", chapter: "", snapshotName: "" });
            await loadSnapshots();
        } catch (error) {
            console.error("Failed to create snapshot", error);
        }
    };

    const handleDelete = async (id) => {
        if (confirm("Delete this snapshot?")) {
            await deleteSnapshot(id);
            await loadSnapshots();
        }
    };

    const handleDuplicate = async (id) => {
        const copy = await duplicateSnapshot(id);
        await createSnapshot(copy);
        await loadSnapshots();
    };

    return (
        <div className="calendar-layout">
            <div className="main-content">
                <h2>Snapshots</h2>

                <div className="snapshot-form">
                    <input
                        type="text"
                        placeholder="Book"
                        value={newSnapshot.book}
                        onChange={(e) => setNewSnapshot({ ...newSnapshot, book: e.target.value })}
                    />
                    <input
                        type="text"
                        placeholder="Act"
                        value={newSnapshot.act}
                        onChange={(e) => setNewSnapshot({ ...newSnapshot, act: e.target.value })}
                    />
                    <input
                        type="text"
                        placeholder="Chapter"
                        value={newSnapshot.chapter}
                        onChange={(e) => setNewSnapshot({ ...newSnapshot, chapter: e.target.value })}
                    />
                    <input
                        type="text"
                        placeholder="Snapshot Name"
                        value={newSnapshot.snapshotName}
                        onChange={(e) => setNewSnapshot({ ...newSnapshot, snapshotName: e.target.value })}
                    />
                    <button onClick={handleCreate}>Create</button>
                </div>

                <table className="table table-dark table-hover mt-4">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Book</th>
                            <th>Act</th>
                            <th>Chapter</th>
                            <th>Snapshot</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {snapshots.map((s) => (
                            <tr key={s.id}>
                                <td>{s.id}</td>
                                <td>{s.book}</td>
                                <td>{s.act}</td>
                                <td>{s.chapter}</td>
                                <td>{s.snapshotName}</td>
                                <td>
                                    <button onClick={() => handleDelete(s.id)}>🗑️</button>
                                    <button onClick={() => handleDuplicate(s.id)}>🧬 Duplicate</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
