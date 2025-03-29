//TODO
//CalendarPlotView.jsx
//Enable “Create Snapshot for Entity” via right - click on a plotpoint
// Support multiple plotpoints on the same day
// Add calendar highlight for “Jespen” (the extra day)
// Enable toggle: “Show PlotPoints for Current Snapshot Only”
// Implement vertical timeline view toggle
// Add context menu logic to day cells
// Add context menu logic to plotpoints
// Add context menu logic to month headers
// Add context menu logic to calendar header
// Add context menu logic to plotpoint cards
// Add context menu logic to snapshot cards
// Add context menu logic to relationship graph
// Add context menu logic to character cards
// Add context menu logic to location cards
// Add context menu logic to faction cards
// Add context menu logic to language cards
// Add context menu logic to event cards
// Add context menu logic to timeline cards
// Add context menu logic to snapshot selector
// Add context menu logic to character selector
// Add context menu logic to location selector
// Add context menu logic to faction selector
// Add context menu logic to language selector
// Add context menu logic to event selector
// Add context menu logic to timeline selector
// Add context menu logic to relationship graph selector


import React, { useState, useEffect, useCallback } from "react";
import DraggablePlotPointCard from "../components/DraggablePlotPointCard";
import { fetchCalendarGrid } from '../api/CalendarApi';
import { fetchPlotPoints } from '../api/PlotPointApi';
import "../styles/CalendarPlotView.css";

export default function CalendarPlotView() {
    const [calendar, setCalendar] = useState([]);
    const [plotPoints, setPlotPoints] = useState([]);
    const [collapsedMonths, setCollapsedMonths] = useState({});
    const [, setIsLoading] = useState(true);

    // ✅ loadData only defined once here:
    const loadData = useCallback(async (retry = 0) => {
        try {
            setIsLoading(true);
            const [calendarData, plotData] = await Promise.all([
                fetchCalendarGrid(),
                fetchPlotPoints()
            ]);

            if (!Array.isArray(calendarData)) {
                console.error("❌ Invalid calendar format", calendarData);
                setCalendar([]);
            } else {
                setCalendar(calendarData);
            }

            setPlotPoints(plotData || []);
        } catch (err) {
            console.error(`❌ Failed to load data. Attempt ${retry + 1}/5`, err);
            if (retry < 4) {
                setTimeout(() => loadData(retry + 1), 1000);
            }
        } finally {
            setIsLoading(false);
        }
    }, []);

    // ✅ Trigger load once after mount
    useEffect(() => {
        loadData();
    }, [loadData]);

    // ...rest of the component (grouping months, rendering days)

    // ----------------------
    // ✅ Collapse/Expand logic
    // ----------------------
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

    // ----------------------
    // ✅ Render
    // ----------------------
    return (
        <div className="calendar-main">
            {/* Optional Refresh Button for testing */}
            {/* <button onClick={forceRefresh}>🔁 Reload</button> */}

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
                                                onContextMenu={() => { }}
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
