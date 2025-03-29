import React from "react";
import CalendarDayCell from "./CalendarDayCell";
import "../styles/CalendarPlotView.css";

export default function CalendarGrid({ calendarDays, plotPoints, onDropPlotPoint, onContextMenu }) {
    return (
        <div className="calendar-grid">
            {calendarDays.map(day => {
                const dayPlotPoints = plotPoints.filter(pp => pp.startDateId === day.id);
                return (
                    <CalendarDayCell
                        key={day.id}
                        day={day}
                        plotPoints={dayPlotPoints}
                        onDropPlotPoint={onDropPlotPoint}
                        onContextMenu={onContextMenu}
                    />
                );
            })}
        </div>
    );
}
