//TimelineStoryView.jsx in src/pages
//TODO this page shows my story on a timeline axis, with the main story being driven by Plotpoints and the writing progress by chapters that link to versions of entities
//the timelineView is two horizontal time scales, one for in story timeine (plotpoints) and synchronized below the book timeline
//there is a way to switch between showing chapter or plotpoints or both
//use timelineviewer component and timelineviewer.css
//add a link in the sidebare that brings the user to this page 


// src/pages/TimelineStoryView.jsx
import React, { useState } from "react";
import TimelineViewer from "../components/TimelineViewer.jsx";
import TimelineViewerModal from "../components/TimelineViewerModal.jsx";
import "../styles/TimelineViewer.css";


export default function TimelineStoryView() {
    const [viewMode, setViewMode] = useState("both"); // Options: 'both', 'plotpoints', 'chapters'

    const handleViewToggle = (mode) => {
        setViewMode(mode);
    };

    return (
        <div className="timeline-story-view">
            <h2>📖 Story Timeline</h2>
            <div className="timeline-controls">
                <label>Show:</label>
                <button onClick={() => handleViewToggle("both")}>PlotPoints + Chapters</button>
                <button onClick={() => handleViewToggle("plotpoints")}>PlotPoints Only</button>
                <button onClick={() => handleViewToggle("chapters")}>Chapters Only</button>
            </div>
            <TimelineViewer viewMode={viewMode} />
        </div>
    );
}
