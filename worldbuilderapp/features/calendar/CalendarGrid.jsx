import React from 'react';
import CalendarDayCell from './CalendarDayCell';
import PlotPointCard from '../plotpoints/PlotPointCard';
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

    const renderSpanningCard = (pp) => {
        const startIdx = dayIndexMap[pp.startDateId];
        const endIdx = pp.endDateId ? dayIndexMap[pp.endDateId] : startIdx;
        if (startIdx === undefined || endIdx === undefined || startIdx > endIdx) return null;

        const spanStart = startIdx + 1;
        const spanLength = endIdx - startIdx + 1;

        return (
            <div
                key={pp.id}
                className="plotpoint-span"
                style={{
                    gridColumn: `${spanStart} / span ${spanLength}`,
                    zIndex: 2,
                }}
            >
                <PlotPointCard
                    plotPoint={pp}
                    span={spanLength > 1}
                    onContextMenu={onContextMenu}
                    onResizeEnd={onResizeEnd}
                />
            </div>
        );
    };

    return (
        <div
            className="calendar-grid"
            style={{ gridTemplateColumns: 'repeat(7, 1fr)' }}
        >
            {/* Day cells */}
            {calendarDays.map((day) => {
                const dayPlotPoints = plotPoints.filter((pp) => pp.startDateId === day.id);
                return (
                    <CalendarDayCell
                        key={day.id}
                        day={day}
                        onDropPlotPoint={onDropPlotPoint}
                        onContextMenu={onContextMenu}
                    >
                        {dayPlotPoints.map((pp) => (
                            <PlotPointCard
                                key={pp.id}
                                plotPoint={pp}
                                span={false}
                                onContextMenu={onContextMenu}
                                onResizeEnd={onResizeEnd}
                            />
                        ))}
                    </CalendarDayCell>
                );
            })}

            {/* Spanning cards rendered once, aligned to start day */}
            {plotPoints
                .filter((pp) =>
                    calendarDays.some((d) => d.id === pp.startDateId)
                )
                .map((pp) => renderSpanningCard(pp))}
        </div>
    );
}
