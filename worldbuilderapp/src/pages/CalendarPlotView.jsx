// Path: src/pages/CalendarPlotView.jsx

import React, { useState, useEffect } from "react";
import DraggablePlotPointCard from "../components/DraggablePlotPointCard";
import { fetchCalendarGrid } from "../api/calendarApi";
import { fetchPlotPoints } from "../api/plotPointApi";
import "../styles/CalendarPlotView.css";

export default function CalendarPlotView() {
    const [calendar, setCalendar] = useState([]);
    const [plotPoints, setPlotPoints] = useState([]);
    const [collapsedMonths, setCollapsedMonths] = useState({});

    useEffect(() => {
        const loadData = async () => {
            const cal = await fetchCalendarGrid();
            const points = await fetchPlotPoints();
            setCalendar(cal || []);
            setPlotPoints(points || []);
        };

        loadData();
        console.log("Calendar view mounted")
    }, []);


    const toggleMonth = (month) => {
        setCollapsedMonths(prev => ({
            ...prev,
            [month]: !prev[month]
        }));
    };

    const groupedByMonth = Array.isArray(calendar)
        ? calendar.reduce((acc, day) => {
            if (!acc[day.month]) acc[day.month] = [];
            acc[day.month].push(day);
            return acc;
        }, {})
        : {};

    if (calendar.length === 0 && plotPoints.length === 0) {
        return <div className="calendar-main">Loading timeline...</div>;
    }

    return (
        <div className="calendar-main">
            {Object.entries(groupedByMonth).map(([month, days]) => (
                <div key={month} className="month-block">
                    <div className="month-header sticky">
                        <button className="collapse-btn" onClick={() => toggleMonth(month)}>
                            {collapsedMonths[month] ? "▶" : "▼"} {month}
                        </button>
                    </div>

                    {!collapsedMonths[month] && (
                        <div className="calendar-grid">
                            {days.map(day => (
                                <div key={day.id} className="calendar-day">
                                    <div className="calendar-day-label">
                                        {day.day} {day.weekday}
                                    </div>

                                    {plotPoints
                                        .filter(pp => pp.calendarId === day.id)
                                        .map(pp => (
                                            <DraggablePlotPointCard
                                                key={pp.id}
                                                plotPoint={pp}
                                                onContextMenu={() => { /* implement if needed */ }}
                                            />
                                        ))}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
}
