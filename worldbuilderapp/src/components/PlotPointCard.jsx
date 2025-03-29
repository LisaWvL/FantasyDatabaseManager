import React from "react";
import "../styles/CalendarPlotView.css";

export default function PlotPointCard({ plotPoint, onContextMenu }) {
    return (
        <div
            className="plotpoint-card"
            onContextMenu={(e) => onContextMenu(e, plotPoint, "card")}
        >
            <strong>{plotPoint.title}</strong>
            <p>{plotPoint.description}</p>
        </div>
    );
}
