import React, { useState, useEffect } from "react";
import ChapterSelector from "../components/ChapterSelector.jsx";
import EntitySelector from "../components/EntitySelector.jsx";
import EntityTable from "../components/EntityTable.jsx";
import { fetchChapters } from '../api/ChapterApi.js'; // ✅ use wrapper

export default function ChapterEntityView() {
    const [chapters, setChapters] = useState([]);
    const [selectedChapter, setSelectedChapter] = useState("");
    const [entityTypes] = useState(["Character", "Location", "Event", "Faction", "Item"]);
    const [selectedEntity, setSelectedEntity] = useState("");
    const [entities, setEntities] = useState([]);

    useEffect(() => {
        fetchChapters()
            .then(setChapters)
            .catch((err) => console.error("Chapter load failed", err));
    }, []);

    useEffect(() => {
        if (selectedChapter && selectedEntity) {
            fetch(`/api/${selectedEntity.toLowerCase()}?chapterId=${selectedChapter}`)
                .then(res => res.json())
                .then(setEntities)
                .catch(err => console.error(`Failed to fetch ${selectedEntity}:`, err));
        } else {
            setEntities([]);
        }
    }, [selectedChapter, selectedEntity]);

    const handleEdit = id => {
        alert(`Edit ${selectedEntity} with ID ${id}`);
    };

    const handleDelete = async id => {
        if (confirm("Are you sure?")) {
            await fetch(`/api/${selectedEntity.toLowerCase()}/${id}`, { method: "DELETE" });
            setEntities(entities.filter(e => e.id !== id));
        }
    };

    const handleChapter = id => {
        window.location.href = `/api/${selectedEntity.toLowerCase()}/${id}/new-chapter-page`;
    };

    return (
        <div className="container mt-4">
            <ChapterSelector chapters={chapters} selectedChapter={selectedChapter} onChange={setSelectedChapter} />
            <EntitySelector entityTypes={entityTypes} selectedEntity={selectedEntity} onChange={setSelectedEntity} />
            <EntityTable data={entities} onEdit={handleEdit} onDelete={handleDelete} onChapter={handleChapter} />
        </div>
    );
}
