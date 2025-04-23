// 📁 CalendarGrid.jsx
import React from 'react';
import CalendarDayCell from './CalendarDayCell';
import './CalendarGrid.css';

export default function CalendarGrid({ calendarDays, cards, onContextMenu, onUpdateCard, onCardDrop, collapsedMonths = {}, setCollapsedMonths }) {
    const calendarMap = Object.fromEntries(calendarDays.map(d => [d.id, d]));
    const groupedCards = groupCardsByDate(cards, calendarMap);

    return (
        <div className="calendar-grid">
            {calendarDays.map(day => {
                const dayCards = groupedCards[day.id] || [];
                const isCollapsed = collapsedMonths[day.month] || false;

                return (
                    <CalendarDayCell
                        key={day.id}
                        date={day}
                        cards={isCollapsed ? [] : dayCards}
                        onContextMenu={onContextMenu}
                        onUpdateCard={onUpdateCard}
                        onCardDrop={onCardDrop}
                    />
                );
            })}
        </div>
    );
}

function groupCardsByDate(cards, calendarMap) {
    const map = {};

    for (const card of cards) {
        const start = card.CardData?.startDateId;
        const end = card.CardData?.endDateId;

        if (!start) continue;

        if (!map[start]) map[start] = [];
        map[start].push({ ...card, isGhost: false });

        if (card.Styling?.isSpanning && start !== end && calendarMap[start] && calendarMap[end]) {
            const ghostRange = getDateRange(calendarMap, start, end);
            ghostRange.forEach(id => {
                if (!map[id]) map[id] = [];
                map[id].push({ ...card, isGhost: true });
            });
        }
    }

    return map;
}

function getDateRange(dateMap, startId, endId) {
    const ids = Object.keys(dateMap).map(Number).sort((a, b) => a - b);
    const startIndex = ids.indexOf(startId);
    const endIndex = ids.indexOf(endId);
    if (startIndex === -1 || endIndex === -1) return [];

    const range = ids.slice(Math.min(startIndex, endIndex) + 1, Math.max(startIndex, endIndex));
    return range;
}



//import DateDayCell from './DateDayCell';
//import Card from '../../components/Card';
//import './DateGrid.css';

//export default function DateGrid({
//    dateDays,
//    plotPoints,
//    onDropPlotPoint,
//    onContextMenu,
//    onResizeEnd
//}) {
//    const dayIndexMap = {};
//    dateDays.forEach((d, i) => {
//        dayIndexMap[d.id] = i;
//    });

//    function handlePlotPointDragStart(e, pp) {
//        e.dataTransfer.setData('entityId', pp.id.toString());
//        e.dataTransfer.setData('entityType', 'PlotPoint');
//        e.dataTransfer.effectAllowed = 'move';
//    }

//    const renderSpanningCard = (pp) => {
//        const startIdx = dayIndexMap[pp.startDateId];
//        const endIdx = pp.endDateId ? dayIndexMap[pp.endDateId] : startIdx;

//        if (startIdx === undefined || endIdx === undefined || startIdx > endIdx) {
//            console.warn(`Skipping span render for PlotPoint ${pp.id} due to invalid start/end`, {
//                startIdx, endIdx, pp
//            });
//            return null;
//        }

//        const spanStart = startIdx + 1;
//        const spanLength = endIdx - startIdx + 1;

//        console.log(`📏 Rendering span for PlotPoint ${pp.id}: ${spanStart} → ${spanLength}`);

//        return (
//            <div
//                key={`span-${pp.id}`}
//                className="plotpoint-span"
//                style={{
//                    gridColumn: `${spanStart} / span ${spanLength}`,
//                    zIndex: 2,
//                }}
//            >
//                <Card
//                    key={`card-${pp.id}`}
//                    entity={pp}
//                    entityType="PlotPoint"
//                    displayMode="date"
//                    isReversed={pp.isReversed}
//                    isGhost={false}
//                    colorIndex={pp.colorIndex}
//                    onContextMenu={(e) => onContextMenu?.(e, pp, 'PlotPoint')}
//                    onResizeEnd={(direction, newDayId) => onResizeEnd?.(pp.id, direction, newDayId)}
//                    onDragStart={(e) => handlePlotPointDragStart(e, pp)}
//                    onDragEnd={() => { }}
//                    draggable
//                />
//            </div>
//        );
//    };

//    const plotpointsByStartDay = plotPoints.reduce((acc, pp) => {
//        if (!pp.startDateId) return acc;
//        if (!acc[pp.startDateId]) acc[pp.startDateId] = [];
//        acc[pp.startDateId].push(pp);
//        return acc;
//    }, {});

//    return (
//        <div className="date-grid" style={{ gridTemplateColumns: 'repeat(7, 1fr)' }}>
//            {dateDays.map((day) => {
//                const dayPlotPoints = plotpointsByStartDay[day.id] || [];
//                return (
//                    <DateDayCell
//                        key={day.id}
//                        day={day}
//                        weekday={day.weekday}
//                        month={day.month}
//                        year={day.year}
//                        onDropPlotPoint={onDropPlotPoint}
//                        onContextMenu={onContextMenu}
//                    >
//                        {dayPlotPoints.map((pp) => (
//                            <Card
//                                key={`pp-${pp.id}-${day.id}`}
//                                entity={pp}
//                                entityType="PlotPoint"
//                                displayMode="date"
//                                isGhost={false}
//                                isReversed={pp.isReversed}
//                                colorIndex={pp.colorIndex}
//                                onContextMenu={(e) => onContextMenu?.(e, pp, 'PlotPoint')}
//                                onResizeEnd={(direction, newDayId) => onResizeEnd?.(pp.id, direction, newDayId)}
//                                onDragStart={(e) => handlePlotPointDragStart(e, pp)}
//                                onDragEnd={() => { }}
//                                draggable
//                            />
//                        ))}
//                    </DateDayCell>
//                );
//            })}

//            {/* Render only spanning cards once, visually positioned using CSS grid */}
//            {plotPoints
//                .filter((pp) => pp.startDateId && pp.endDateId && pp.endDateId !== pp.startDateId)
//                .map((pp) => renderSpanningCard(pp))}
//        </div>
//    );
//}
