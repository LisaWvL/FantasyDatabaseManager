// src/components/DraggablePlotPointCard.jsx
import React from "react";
import { useDrag } from "react-dnd";
import "../styles/CalendarPlotView.css";

export const ItemTypes = {
    PLOTPOINT: "plotpoint",
};

export default function DraggablePlotPointCard({ plotPoint, onContextMenu }) {
    const [{ isDragging }, drag] = useDrag(() => ({
        type: ItemTypes.PLOTPOINT,
        item: { id: plotPoint.id, calendarId: plotPoint.calendarId },
        collect: (monitor) => ({
            isDragging: monitor.isDragging(),
        }),
    }));

    const handleContextMenu = (e) => {
        e.preventDefault();
        onContextMenu(e, plotPoint, "plotpoint");
    };

    return (
        <div
            ref={drag}
            className="plotpoint-card"
            style={{
                opacity: isDragging ? 0.5 : 1,
                cursor: "move",
            }}
            onContextMenu={handleContextMenu}
        >
            {plotPoint.title}
        </div>
    );
}
