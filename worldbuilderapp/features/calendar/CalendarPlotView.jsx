import React, { useEffect, useState } from 'react';
import CalendarDayCell from './CalendarDayCell';
import PlotPointCard from '../plotpoints/PlotPointCard';
import { fetchCalendarGrid } from './CalendarApi';
import { fetchPlotPoints, updatePlotPoint } from '../plotpoints/PlotPointApi';
import './CalendarPlotView.css';

export default function CalendarPlotView() {
    const [calendar, setCalendar] = useState([]);
    const [plotPoints, setPlotPoints] = useState([]);
    const [collapsedMonths, setCollapsedMonths] = useState({});

    useEffect(() => {
        async function loadData() {
            const [calendarData, plotPointData] = await Promise.all([
                fetchCalendarGrid(),
                fetchPlotPoints()
            ]);
            setCalendar(calendarData);
            setPlotPoints(plotPointData);
        }
        loadData();
    }, []);

    const calendarById = Object.fromEntries(calendar.map(d => [d.id, d]));
    const plotpointsByDay = {};

    plotPoints.forEach((pp) => {
        if (!pp.startDateId) return;

        const start = pp.startDateId;
        const end = pp.endDateId || start;
        const colorIndex = pp.id % 10;

        for (let dayId = start; dayId <= end; dayId++) {
            if (!calendarById[dayId]) continue;
            if (!plotpointsByDay[dayId]) plotpointsByDay[dayId] = [];

            plotpointsByDay[dayId].push({
                ...pp,
                isGhost: dayId !== start,
                colorIndex,
            });
        }
    });

    const groupedByMonth = {};
    calendar.forEach((day) => {
        if (!groupedByMonth[day.month]) groupedByMonth[day.month] = [];
        groupedByMonth[day.month].push(day);
    });

    const handleResize = async (plotPointId, direction, newDayId) => {
        const pp = plotPoints.find(p => p.id === plotPointId);
        if (!pp) return;
        const updated = { ...pp };
        if (direction === 'start') updated.startDateId = newDayId;
        if (direction === 'end') updated.endDateId = newDayId;
        const saved = await updatePlotPoint(plotPointId, updated);
        setPlotPoints(prev => prev.map(p => (p.id === plotPointId ? saved : p)));
    };

    const handleDropPlotPoint = async (plotPointId, dayId) => {
        const pp = plotPoints.find(p => p.id === plotPointId);
        if (!pp || pp.startDateId === dayId) return;

        const diff = (pp.endDateId || pp.startDateId) - pp.startDateId;
        const newStart = dayId;
        const newEnd = newStart + diff;

        const updated = await updatePlotPoint(plotPointId, {
            ...pp,
            startDateId: newStart,
            endDateId: diff > 0 ? newEnd : null,
        });

        setPlotPoints(prev => prev.map(p => (p.id === plotPointId ? updated : p)));
    };

    return (
        <div className="calendar-main">
            {Object.entries(groupedByMonth).map(([month, days]) => (
                <div key={month} className="month-block">
                    <div className="month-header sticky">
                        <button
                            className="collapse-btn"
                            onClick={() =>
                                setCollapsedMonths(p => ({ ...p, [month]: !p[month] }))
                            }
                        >
                            {collapsedMonths[month] ? '▶' : '▼'} {month}
                        </button>
                    </div>

                    {!collapsedMonths[month] && (
                        <div className="calendar-grid">
                            {days.map((day) => (
                                <CalendarDayCell
                                    key={day.id}
                                    day={day}
                                    weekday={calendarById[day.id]}
                                    onDropPlotPoint={handleDropPlotPoint}
                                >
                                    {(plotpointsByDay[day.id] || []).map((pp) => (
                                        <PlotPointCard
                                            key={`${pp.id}-${day.id}`}
                                            plotPoint={pp}
                                            isGhost={pp.isGhost}
                                            colorIndex={pp.colorIndex}
                                            onResizeEnd={handleResize}
                                            onContextMenu={(e) => {
                                                e.preventDefault();
                                                console.log('Context menu for:', pp);
                                            }}
                                        />
                                    ))}
                                </CalendarDayCell>
                            ))}
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
}
