// 📁 features/plotpoints/Dashboard.jsx
import React, { useEffect, useState, useMemo } from 'react';
import CalendarGrid from './CalendarGrid';
import { getDashboardGrid } from './PlotPointApi';
import { useOutletContext } from 'react-router-dom';
import './Dashboard.css';
import './CalendarGrid.css';
import './CalendarDayCell.css';
import { useDragAndDrop } from '../../hooks/useDragAndDrop';

export default function Dashboard() {
    const [calendarGrid, setCalendarGrid] = useState([]);
    const { cards, setCards } = useOutletContext();

    useEffect(() => {
        async function loadCalendar() {
            try {
                const { calendarGrid } = await getDashboardGrid();
                setCalendarGrid(calendarGrid);
            } catch (error) {
                console.error('🚨 Calendar load failed:', error);
            }
        }
        loadCalendar();
    }, []);

    const calendarById = useMemo(
        () => Object.fromEntries(calendarGrid.map(day => [day.id, day])),
        [calendarGrid]
    );

    const refreshCards = async () => {
        try {
            const response = await fetch('/api/cards/getDashboardCards');
            const { cards: updated } = await response.json();
            setCards(updated);
        } catch (err) {
            console.error('❌ Failed to refresh cards:', err);
        }
    };

    // 📦 Drag hook
    const { handleDrop } = useDragAndDrop({
        onDropSuccess: async (entityId, dropTargetDayId, entityType) => {
            console.log('📦 Drop success:', { entityId, dropTargetDayId, entityType });
            await refreshCards();
        }
    });

    // 📏 Resize handler
    const handleResizeEnd = async (entityId, direction, newDayId) => {
        const card = cards.find(c => c.cardData?.Id === entityId);
        if (!card) return;

        const cardData = card.cardData;
        const entityType = card.entityType ?? cardData?.EntityType;

        let start = cardData.StartDateId;
        let end = cardData.EndDateId ?? start;

        if (direction === 'start') start = newDayId;
        else if (direction === 'end') end = newDayId;

        try {
            const response = await fetch('/api/cards/updateDateRange', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    EntityType: entityType,
                    Id: entityId,
                    StartDateId: start,
                    EndDateId: end
                })
            });

            const result = await response.json();
            console.log('✅ Resize success:', result);
            await refreshCards();
        } catch (err) {
            console.error('❌ Resize save failed:', err);
        }
    };

    // 📥 Drop handler
    const handleDropEntity = async (entityId, entityType, dayId) => {
        const card = cards.find(c => c.cardData?.Id === entityId && c.entityType === entityType);
        if (!card) return;

        const cardData = card.cardData;
        const currentSpan = {
            StartDateId: dayId,
            EndDateId: cardData.EndDateId ?? dayId,
        };

        // Preserve duration
        const duration = (cardData.EndDateId ?? cardData.StartDateId) - cardData.StartDateId;
        currentSpan.EndDateId = dayId + duration;

        // Optimistic update
        const optimisticCards = cards.map(c => 
            c.cardData?.Id === entityId ? {
                ...c,
                cardData: {
                    ...c.cardData,
                    StartDateId: currentSpan.StartDateId,
                    EndDateId: currentSpan.EndDateId
                }
            } : c
        );
        setCards(optimisticCards);

        try {
            const response = await fetch('/api/cards/updateDateRange', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    EntityType: entityType,
                    Id: entityId,
                    StartDateId: currentSpan.StartDateId,
                    EndDateId: currentSpan.EndDateId
                })
            });

            const result = await response.json();
            console.log('📥 Card dropped and updated:', result);
            await refreshCards(); // Only refresh if needed to sync with other changes
        } catch (err) {
            console.error('❌ Failed to drop entity:', err);
            // Revert optimistic update on failure
            setCards(cards);
        }
    };

    return (
        <CalendarGrid
            calendarDays={calendarGrid}
            cards={cards}
            onDropEntity={handleDropEntity}
            onResizeEnd={handleResizeEnd}
            onContextMenu={(entityType, id) => {
                console.log('📜 Context menu opened for:', entityType, id);
            }}
        />
    );
}