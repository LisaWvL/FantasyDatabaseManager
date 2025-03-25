import React, { useState, useEffect } from "react";
import SnapshotSelector from "../components/SnapshotSelector.jsx";
import EntitySelector from "../components/EntitySelector.jsx";
import EntityTable from "../components/EntityTable.jsx";

export default function SnapshotEntityView() {
    const [snapshots, setSnapshots] = useState([]);
    const [selectedSnapshot, setSelectedSnapshot] = useState("");
    const [entityTypes] = useState(["Character", "Location", "Event", "Faction", "Artifact"]);
    const [selectedEntity, setSelectedEntity] = useState("");
    const [entities, setEntities] = useState([]);

    useEffect(() => {
        fetch("/api/snapshot")
            .then(res => res.json())
            .then(setSnapshots);
    }, []);

    useEffect(() => {
        if (selectedSnapshot && selectedEntity) {
            fetch(`/api/${selectedEntity.toLowerCase()}?snapshotId=${selectedSnapshot}`)
                .then(res => res.json())
                .then(setEntities);
        } else {
            setEntities([]);
        }
    }, [selectedSnapshot, selectedEntity]);

    const handleEdit = id => {
        alert(`Edit ${selectedEntity} with ID ${id}`);
        // Navigate to or open edit view
    };

    const handleDelete = async id => {
        if (confirm("Are you sure?")) {
            await fetch(`/api/${selectedEntity.toLowerCase()}/${id}`, { method: "DELETE" });
            setEntities(entities.filter(e => e.id !== id));
        }
    };

    const handleSnapshot = id => {
        window.location.href = `/api/${selectedEntity.toLowerCase()}/${id}/new-snapshot-page`;
    };

    return (
        <div className="container mt-4">
            <SnapshotSelector snapshots={snapshots} selectedSnapshot={selectedSnapshot} onChange={setSelectedSnapshot} />
            <EntitySelector entityTypes={entityTypes} selectedEntity={selectedEntity} onChange={setSelectedEntity} />
            <EntityTable data={entities} onEdit={handleEdit} onDelete={handleDelete} onSnapshot={handleSnapshot} />
        </div>
    );
}
