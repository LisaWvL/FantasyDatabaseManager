// src/components/TimelineViewerModal.jsx

import React, { useState } from "react";
import PlotPointModal from "./PlotPointModal";

export default function TimelineViewerModal({ onAddPlotPoint }) {
    const [isOpen, setIsOpen] = useState(false);

    const handleOpen = () => setIsOpen(true);
    const handleClose = () => setIsOpen(false);

    const handleSave = (newPlotPoint) => {
        if (onAddPlotPoint) {
            onAddPlotPoint(newPlotPoint);
        }
        setIsOpen(false);
    };

    return (
        <div>
            <button className="add-plotpoint-btn" onClick={handleOpen}>
                + New PlotPoint
            </button>

            {isOpen && (
                <div className="modal-overlay">
                    <PlotPointModal onClose={handleClose} onSave={handleSave} />
                </div>
            )}
        </div>
    );
}
