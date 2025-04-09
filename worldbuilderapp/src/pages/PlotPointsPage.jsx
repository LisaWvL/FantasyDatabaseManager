// src/pages/PlotPointsPage.jsx
import React, { useEffect, useState } from 'react';
//import { fetchChapters } from '../api/ChapterApi';
import {
    fetchPlotPoints,
    deletePlotPoint,
    fetchForNewChapter
} from '../api/PlotPointApi';
import EntityTable from '../components/EntityTable';
import { useNavigate } from 'react-router-dom';

export default function PlotPointsPage() {
    const [plotPoints, setPlotPoints] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadPlotPoints();
    }, []);

    const loadPlotPoints = async () => {
        try {
            const data = await fetchPlotPoints();
            setPlotPoints(data);
        } catch (err) {
            console.error("Failed to load plot points", err);
        }
    };

    const handleEdit = (id) => {
        navigate(`/plotpoint/${id}`);
    };

    const handleChapter = async (id) => {
        try {
            const chapterVm = await fetchForNewChapter(id);
            navigate(`/plotpoint/${id}/chapter`, { state: { chapterVm } });
        } catch (err) {
            console.error("Failed to fetch new chapter", err);
        }
    };

    const handleDelete = async (id) => {
        if (confirm("Are you sure you want to delete this PlotPoint?")) {
            await deletePlotPoint(id);
            await loadPlotPoints();
        }
    };

    return (
        <div>
            <h2>Plot Points</h2>
            <EntityTable
                data={plotPoints}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onChapter={handleChapter}
            />
        </div>
    );
}
