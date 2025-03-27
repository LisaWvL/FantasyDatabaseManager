// src/components/CalendarDayCell.jsx
import React from "react";
import { useDrop } from "react-dnd";
import DraggablePlotPointCard from "./DraggablePlotPointCard";

import "../styles/CalendarPlotView.css";

export default function CalendarDayCell({ day, plotPoints, onDropPlotPoint, onContextMenu }) {
    const [, drop] = useDrop({
        accept: "plotpoint",
        drop: (item) => onDropPlotPoint(item.id, day.id),
    });

    return (
        <div
            className="calendar-cell"
            ref={drop}
            onContextMenu={(e) => onContextMenu(e, day, "calendar")}
        >
            <div className="calendar-header">
                <span>{day.day} {day.weekday}</span>
                <span>{day.month}</span>
            </div>

            {plotPoints.map(p => (
                <DraggablePlotPointCard
                    key={p.id}
                    plotPoint={p}
                    onContextMenu={onContextMenu}
                />
            ))}
        </div>
    );
}
