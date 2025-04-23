import React from 'react';
import CalendarDayCell from './CalendarDayCell';
import Card from '../../src/components/Card';
import './CalendarGrid.css';

export default function CalendarGrid({
    calendarDays,
    plotPoints,
    onDropPlotPoint,
    onContextMenu,
    onResizeEnd
}) {
    const dayIndexMap = {};
    calendarDays.forEach((d, i) => {
        dayIndexMap[d.id] = i;
    });

    function handlePlotPointDragStart(e, pp) {
        e.dataTransfer.setData('entityId', pp.id.toString());
        e.dataTransfer.setData('entityType', 'PlotPoint');
        e.dataTransfer.effectAllowed = 'move';
    }

    const renderSpanningCard = (pp) => {
        const startIdx = dayIndexMap[pp.startDateId];
        const endIdx = pp.endDateId ? dayIndexMap[pp.endDateId] : startIdx;

        if (startIdx === undefined || endIdx === undefined || startIdx > endIdx) {
            console.warn(`Skipping span render for PlotPoint ${pp.id} due to invalid start/end`, {
                startIdx, endIdx, pp
            });
            return null;
        }

        const spanStart = startIdx + 1;
        const spanLength = endIdx - startIdx + 1;

        console.log(`📏 Rendering span for PlotPoint ${pp.id}: ${spanStart} → ${spanLength}`);

        return (
            <div
                key={`span-${pp.id}`}
                className="plotpoint-span"
                style={{
                    gridColumn: `${spanStart} / span ${spanLength}`,
                    zIndex: 2,
                }}
            >
                <Card
                    key={`card-${pp.id}`}
                    entity={pp}
                    entityType="PlotPoint"
                    displayMode="calendar"
                    isReversed={pp.isReversed}
                    isGhost={false}
                    colorIndex={pp.colorIndex}
                    onContextMenu={(e) => onContextMenu?.(e, pp, 'PlotPoint')}
                    onResizeEnd={(direction, newDayId) => onResizeEnd?.(pp.id, direction, newDayId)}
                    onDragStart={(e) => handlePlotPointDragStart(e, pp)}
                    onDragEnd={() => { }}
                    draggable
                />
            </div>
        );
    };

    const plotpointsByStartDay = plotPoints.reduce((acc, pp) => {
        if (!pp.startDateId) return acc;
        if (!acc[pp.startDateId]) acc[pp.startDateId] = [];
        acc[pp.startDateId].push(pp);
        return acc;
    }, {});

    return (
        <div className="calendar-grid" style={{ gridTemplateColumns: 'repeat(7, 1fr)' }}>
            {calendarDays.map((day) => {
                const dayPlotPoints = plotpointsByStartDay[day.id] || [];
                return (
                    <CalendarDayCell
                        key={day.id}
                        day={day}
                        weekday={day.weekday}
                        month={day.month}
                        year={day.year}
                        onDropPlotPoint={onDropPlotPoint}
                        onContextMenu={onContextMenu}
                    >
                        {dayPlotPoints.map((pp) => (
                            <Card
                                key={`pp-${pp.id}-${day.id}`}
                                entity={pp}
                                entityType="PlotPoint"
                                displayMode="calendar"
                                isGhost={false}
                                isReversed={pp.isReversed}
                                colorIndex={pp.colorIndex}
                                onContextMenu={(e) => onContextMenu?.(e, pp, 'PlotPoint')}
                                onResizeEnd={(direction, newDayId) => onResizeEnd?.(pp.id, direction, newDayId)}
                                onDragStart={(e) => handlePlotPointDragStart(e, pp)}
                                onDragEnd={() => { }}
                                draggable
                            />
                        ))}
                    </CalendarDayCell>
                );
            })}

            {/* Render only spanning cards once, visually positioned using CSS grid */}
            {plotPoints
                .filter((pp) => pp.startDateId && pp.endDateId && pp.endDateId !== pp.startDateId)
                .map((pp) => renderSpanningCard(pp))}
        </div>
    );
}
