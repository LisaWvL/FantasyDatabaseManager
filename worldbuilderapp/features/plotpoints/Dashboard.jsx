import React, { useEffect, useState } from 'react';
import CalendarDayCell from './CalendarDayCell';
import PlotPointCard from './PlotPointCard';
import PlotPointModal from './PlotPointModal';
import DashboardLayout from './DashboardLayout';

import { fetchCalendarGrid } from './CalendarApi';
import { fetchPlotPoints, updatePlotPoint } from './PlotPointApi';
import './Dashboard.css';

export default function Dashboard() {
    const [calendar, setCalendar] = useState([]);
    const [plotPoints, setPlotPoints] = useState([]);
    const [collapsedMonths, setCollapsedMonths] = useState({});
    const [showModal, setShowModal] = useState(false);
    const [newPlotPointId, setNewPlotPointId] = useState(null);
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [isOverSidebar, setIsOverSidebar] = useState(false);

    useEffect(() => {
        async function loadInitialData() {
            const [calendarData, plotPointData] = await Promise.all([
                fetchCalendarGrid(),
                fetchPlotPoints(),
            ]);
            setCalendar(calendarData);
            setPlotPoints(plotPointData);
        }
        loadInitialData();
    }, []);

    const calendarById = Object.fromEntries(calendar.map(day => [day.id, day]));

    const groupedByMonth = calendar.reduce((acc, day) => {
        if (!acc[day.month]) acc[day.month] = [];
        acc[day.month].push(day);
        return acc;
    }, {});

    const plotpointsByDay = plotPoints.reduce((acc, pp) => {
        if (!pp.startDateId) return acc;

        const isReversed = pp.endDateId && pp.endDateId < pp.startDateId;
        const start = isReversed ? pp.endDateId : pp.startDateId;
        const end = isReversed ? pp.startDateId : (pp.endDateId || pp.startDateId);
        const colorIndex = pp.id % 10;

        for (let dayId = start; dayId <= end; dayId++) {
            if (!calendarById[dayId]) continue;
            if (!acc[dayId]) acc[dayId] = [];
            acc[dayId].push({
                ...pp,
                isGhost: dayId !== (isReversed ? pp.endDateId : pp.startDateId),
                colorIndex,
                isReversed,
            });
        }

        return acc;
    }, {});

    const handleResize = async (plotPointId, direction, newDayId) => {
        const pp = plotPoints.find(p => p.id === plotPointId);
        if (!pp) return;

        const updated = {
            ...pp,
            startDateId: direction === 'start' ? newDayId : pp.startDateId,
            endDateId: direction === 'end' ? newDayId : (pp.endDateId ?? pp.startDateId),
        };

        const saved = await updatePlotPoint(plotPointId, updated);
        setPlotPoints(prev => prev.map(p => (p.id === plotPointId ? saved : p)));
    };

    const handleDropPlotPoint = async (plotPointId, dayId) => {
        const pp = plotPoints.find(p => p.id === plotPointId);
        if (!pp || pp.startDateId === dayId) return;

        const originalStart = pp.startDateId;
        const originalEnd = pp.endDateId ?? pp.startDateId;
        const isReversed = originalEnd < originalStart;

        const duration = Math.abs(originalEnd - originalStart);
        let newStart, newEnd;

        if (pp.startDateId === null && pp.endDateId === null) {
            newStart = newEnd = dayId;
        } else if (isReversed) {
            newEnd = dayId;
            newStart = newEnd + duration;
        } else {
            newStart = dayId;
            newEnd = newStart + duration;
        }

        const updated = { ...pp, startDateId: newStart, endDateId: newEnd };
        const saved = await updatePlotPoint(plotPointId, updated);
        setPlotPoints(prev => prev.map(p => (p.id === plotPointId ? saved : p)));
        setNewPlotPointId(null);
    };

    const handleDropToSidebar = async (plotPointId) => {
        const pp = plotPoints.find(p => p.id === plotPointId);
        if (!pp) return;

        const updated = await updatePlotPoint(plotPointId, {
            ...pp,
            startDateId: null,
            endDateId: null,
        });

        setPlotPoints(prev => prev.map(p => (p.id === plotPointId ? updated : p)));
    };

    const handleNewPlotPoint = (created) => {
        setPlotPoints(prev => [...prev, created]);
        setNewPlotPointId(created.id);
        setShowModal(false);
    };

    const sidebarContent = (
        <>
            <h3>Unplaced PlotPoints</h3>
            {plotPoints
                .filter(p => p.startDateId === null && p.endDateId === null)
                .map(p => (
                    <PlotPointCard
                        key={p.id}
                        plotPoint={p}
                        isGhost={false}
                        colorIndex={p.id % 10}
                        onResizeEnd={handleResize}
                        onContextMenu={(e) => {
                            e.preventDefault();
                            console.log('Context menu:', p);
                        }}
                    />
                ))}
        </>
    );

    const headerContent = (
        <button className="add-button" onClick={() => setShowModal(true)}>
            ＋ Add PlotPoint
        </button>
    );

    const calendarContent = (
        <>
            {Object.entries(groupedByMonth).map(([month, days]) => (
                <div key={month} className="month-block">
                    <div className="month-header sticky">
                        <button
                            className="collapse-btn"
                            onClick={() =>
                                setCollapsedMonths(prev => ({ ...prev, [month]: !prev[month] }))
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
                                    {(plotpointsByDay[day.id] || []).map(pp => (
                                        <PlotPointCard
                                            key={`${pp.id}-${day.id}`}
                                            plotPoint={pp}
                                            isGhost={pp.isGhost}
                                            colorIndex={pp.colorIndex}
                                            onResizeEnd={handleResize}
                                            onContextMenu={(e) => {
                                                e.preventDefault();
                                                console.log('Context menu:', pp);
                                            }}
                                        />
                                    ))}
                                    {newPlotPointId &&
                                        newPlotPointId ===
                                        (plotpointsByDay[day.id] || []).find(p => p.id === newPlotPointId)?.id && (
                                            <div className="new-plotpoint-hint">📌 Drop here</div>
                                        )}
                                </CalendarDayCell>
                            ))}
                        </div>
                    )}
                </div>
            ))}
        </>
    );

    return (
        <>
            <DashboardLayout
                headerContent={headerContent}
                sidebarContent={sidebarContent}
                isSidebarOpen={isSidebarOpen}
                setIsSidebarOpen={setIsSidebarOpen}
                isOverSidebar={isOverSidebar}
                onSidebarDragOver={(e) => {
                    e.preventDefault();
                    setIsOverSidebar(true);
                }}
                onSidebarDragLeave={() => setIsOverSidebar(false)}
                onSidebarDrop={(e) => {
                    const plotPointId = parseInt(e.dataTransfer.getData('plotPointId'));
                    handleDropToSidebar(plotPointId);
                    setIsOverSidebar(false);
                }}
            >
                {calendarContent}
            </DashboardLayout>

            {showModal && (
                <PlotPointModal
                    onClose={() => setShowModal(false)}
                    onSave={handleNewPlotPoint}
                />
            )}
        </>
    );
}
