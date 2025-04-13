import React from 'react';
import './CalendarDayCell.css';
//import { fetchWeekdays } from './CalendarApi';

export default function CalendarDayCell({ day, weekday, children, onDropPlotPoint }) {
    const handleDragOver = (e) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
    };

    const handleDrop = (e) => {
        e.preventDefault();
        const plotPointId = parseInt(e.dataTransfer.getData('plotPointId'));
        console.log('📦 dropped on', day.id, 'plotPointId:', plotPointId);

        if (!isNaN(plotPointId)) {
            onDropPlotPoint(plotPointId, day.id);
        }
    };

    return (
        <div
            className="calendar-cell"
            data-dayid={day.id}
            onDragOver={handleDragOver}
            onDrop={handleDrop}
        >
            <div className="calendar-header">
                <div className="calendar-day-number">{day.day}</div>
                <div className="calendar-weekday">{weekday.weekday}</div>
                <div className="calendar-meta">{day.name}</div>
            </div>
            <div className="calendar-body">
                {children}
            </div>
        </div>
    );
}
