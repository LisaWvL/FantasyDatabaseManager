// Path: src/pages/EraPage.jsx
import React, { useEffect, useState } from "react";
import {
    fetchEras,
    createEra,
  //  updateEra,
    deleteEra,
    fetchNewSnapshotEra
} from "../api/eraApi";

export default function EraPage() {
    const [eras, setEras] = useState([]);
    const [newEra, setNewEra] = useState({ name: "", description: "" });

    useEffect(() => {
        loadEras();
    }, []);

    const loadEras = async () => {
        try {
            const data = await fetchEras();
            setEras(data);
        } catch (err) {
            console.error(err);
        }
    };

    const handleCreate = async () => {
        if (!newEra.name) return;
        await createEra(newEra);
        setNewEra({ name: "", description: "" });
        loadEras();
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Delete this era?")) return;
        await deleteEra(id);
        loadEras();
    };

    return (
        <div className="entity-page">
            <h2>Eras</h2>

            <div className="entity-form">
                <input
                    type="text"
                    placeholder="Era name"
                    value={newEra.name}
                    onChange={(e) => setNewEra({ ...newEra, name: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="Description"
                    value={newEra.description}
                    onChange={(e) => setNewEra({ ...newEra, description: e.target.value })}
                />
                <button onClick={handleCreate}>Create Era</button>
            </div>

            <div className="entity-table">
                <table>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                            <th>Snapshot</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {eras.map((era) => (
                            <tr key={era.id}>
                                <td>{era.name}</td>
                                <td>{era.description}</td>
                                <td>{era.snapshotName}</td>
                                <td>
                                    <button onClick={() => fetchNewSnapshotEra(era.id)}>Snapshot</button>
                                    <button onClick={() => handleDelete(era.id)}>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
