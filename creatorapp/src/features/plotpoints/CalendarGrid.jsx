// 📁 features/plotpoints/CalendarGrid.jsx
import React from 'react';
import './CalendarGrid.css';
import CalendarDayCell from './CalendarDayCell';
import Card from '../../components/Card';

export default function CalendarGrid({
    calendarDays,
    cards,
    onDropEntity,
    onResizeEnd,
    onContextMenu
}) {
    const cardsByDay = {};     // Start date → cards
    const ghostSpans = [];     // Spanning visual placeholders

    for (const card of cards) {
        const CardData = card.CardData ?? card.cardData ?? {};
        const entityType = CardData.EntityType ?? card.entityType;
        const id = CardData.Id;
        const startDateId = CardData.StartDateId;
        const endDateId = CardData.EndDateId ?? startDateId;

        if (!startDateId) continue;

        // Place card in its starting day
        if (!cardsByDay[startDateId]) cardsByDay[startDateId] = [];
        cardsByDay[startDateId].push({ ...card, entityType, id });

        // If card spans multiple days
        if (startDateId !== endDateId) {
            const min = Math.min(startDateId, endDateId);
            const max = Math.max(startDateId, endDateId);
            const isReversed = startDateId > endDateId;

            // Create ghost spans with more metadata
            for (let dayId = min + 1; dayId <= max; dayId++) {
                ghostSpans.push({
                    entityType,
                    id,
                    dayId,
                    isReversed,
                    color: CardData.Color ?? '#888',
                    title: CardData.Name || CardData.Title || `#${id}`,
                    isStart: dayId === min,
                    isEnd: dayId === max,
                    position: dayId === min + 1 ? 'start' : dayId === max ? 'end' : 'middle'
                });
            }
        }
    }

    return (
        <div 
            className="calendar-grid" 
            style={{ 
                gridTemplateColumns: `repeat(${calendarDays.length > 0 ? 7 : 1}, 1fr)`,
                gridAutoRows: 'minmax(160px, auto)'
            }}
        >
            {calendarDays.map(day => {
                const realCards = cardsByDay[day.id] ?? [];
                const ghostsToday = ghostSpans.filter(g => g.dayId === day.id);

                return (
                    <CalendarDayCell key={`cell-${day.id}`} day={day} onDropEntity={onDropEntity}>
                        {/* Main calendar cards */}
                        {realCards.map(card => (
                            <Card
                                key={`card-${card.entityType}-${card.id}`}
                                card={card}
                                mode="calendar"
                                onContextMenu={(e) => onContextMenu?.(e, card)}
                                onResizeEnd={onResizeEnd}
                            />
                        ))}

                        {/* Enhanced ghost visual spans */}
                        {ghostsToday.map(ghost => (
                            <div
                                key={`ghost-${ghost.entityType}-${ghost.id}-${ghost.dayId}`}
                                className={`card-ghost ${ghost.position} ${ghost.isReversed ? 'reversed' : ''}`}
                                style={{
                                    borderColor: ghost.color,
                                    backgroundColor: `${ghost.color}22`,
                                }}
                                title={`Continuation of: ${ghost.title}`}
                            >
                                <div className="ghost-content">
                                    {ghost.position === 'middle' && '⋯'}
                                    {ghost.position === 'start' && ghost.isReversed ? '◀' : ''}
                                    {ghost.position === 'end' && !ghost.isReversed ? '▶' : ''}
                                </div>
                            </div>
                        ))}
                    </CalendarDayCell>
                );
            })}
        </div>
    );
}
