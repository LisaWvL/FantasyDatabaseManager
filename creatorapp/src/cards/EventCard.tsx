import { useState } from 'react';
import useEntityRegistry from '../store/useEntityRegistry';
import { EntitySchemas } from '../store/EntitySchemas';
import type { Event } from '../types/entities';
import type { DisplayMode, CardContext } from './EntityCardTypes';
import { updateEvent } from '../features/events/EventApi';

export type EventCardProps = {
    entityType: 'Event';
    entityId: number;
    displayMode: DisplayMode;
    context?: CardContext;
    DroppedOn?: string;
    Payload?: string;
    isGhost?: boolean;
    isReversed?: boolean;
    colorIndex?: string;
    onResizeEnd?: (direction: 'start' | 'end', newDayId: number) => void;
};

export function EventCard({ entityId, displayMode, context }: EventCardProps) {
    const { get } = useEntityRegistry();
    const event = get('Event', entityId);
    const schema = EntitySchemas['Event'];

    const [showSummary, setShowSummary] = useState(true);
    const [showDetails, setShowDetails] = useState(true);

    if (!event) {
        return (
            <div className="event-card event-card--loading">
                Loading Event #{entityId}...
            </div>
        );
    }

    const renderField = (key: keyof Event) => {
        const value = event[key];
        return (
            <div key={String(key)} className="event-card__field">
                <span className="event-card__label">{String(key)}:</span>
                <span className="event-card__value">{String(value)}</span>
            </div>
        );
    };

    const handleDragStart = (e: React.DragEvent) => {
        const payload = JSON.stringify({
            entityType: 'Event',
            entityId,
            context,
        });
        e.dataTransfer.setData('application/json', payload);
        e.dataTransfer.setData('entityId', String(entityId));
        e.dataTransfer.setData('entityType', 'Event'); // e.g., 'event', 'event', etc.

        e.dataTransfer.effectAllowed = 'move';
    };


    const onDrop = async (e: React.DragEvent) => {
        const raw = e.dataTransfer.getData('application/json');
        if (!raw) return;
        const dragged = JSON.parse(raw);
        const updates: Partial<Event> = {};

        // CASE 1: Dropped into a calendar grid cell in Dashboard
        // → Update startDateId and endDateId to match the CalendarDayCell ID
        if (context === 'dashboard') {
            const calendarDayId = Number(e.currentTarget.getAttribute('data-calendar-day-id'));
            if (calendarDayId && dragged.entityType === 'Event') {
                updates.calendarId = calendarDayId;
            }
        }

        // CASE 2: Dropped into UnassignedSidebar while in the Dashboard context
        // → Clear startDateId and endDateId
        if (context === 'unassignedsidebar') {
            if (dragged.context === 'dashboard') {
                updates.calendarId = null;
            }
        }

        // CASE 3: Dropped onto a Chapter in ChapterEditor context
        //if (context === 'chaptereditor') {
        //    if (dragged.entityType === 'Chapter') {
        //        // TODO: Implement logic to add this chapterId to the plot point's list of associated chapters
        //        console.log('📌 Placeholder: Add Chapter', dragged.entityId, 'to Event', entityId);
        //    }
        //}

        try {
            if (Object.keys(updates).length > 0) {
                await updateEvent(entityId, updates);
                console.log(`✅ Event ${entityId} updated:`, updates);
            }
        } catch (error) {
            console.error(`❌ Failed to update Event ${entityId}`, error);
        }
    };

    const dragProps = ['compact', 'basic'].includes(displayMode)
        ? {
            draggable: true,
            handleDragStart,
            onDrop,
            onDragOver: (e: React.DragEvent) => e.preventDefault(),
        }
        : {};

    if (displayMode === 'compact') {
        return (
            <div
                {...dragProps}
                className="event-card event-card--compact"
            >
                {schema.fields.filter((f) => f.showInCompact).map((f) => renderField(f.key as keyof Event))}
            </div>
        );
    }

    if (displayMode === 'basic') {
        return (
            <div
                {...dragProps}
                className="event-card event-card--basic"
            >
                {/* HEADER */}
                <div className="event-card__section">
                    <div className="event-card__section-title">Header</div>
                    {schema.fields
                        .filter((f) => f.section === 'header')
                        .map((f) => renderField(f.key as keyof Event))}
                </div>

                {/* RELATION */}
                <div className="event-card__section">
                    <div className="event-card__section-title">Relation</div>
                    {schema.fields
                        .filter((f) => f.section === 'relation')
                        .map((f) => renderField(f.key as keyof Event))}
                </div>

                {/* SUMMARY (toggleable) */}
                <div className="event-card__section">
                    <div
                        className="event-card__section-title event-card__section-toggle"
                        onClick={() => setShowSummary(!showSummary)}
                    >
                        <span>{showSummary ? '▼' : '▶'} Summary</span>
                    </div>
                    {showSummary && (
                        <div className="event-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'summary')
                                .map((f) => renderField(f.key as keyof Event))}
                        </div>
                    )}
                </div>

                {/* DETAILS (toggleable) */}
                <div className="event-card__section">
                    <div
                        className="event-card__section-title event-card__section-toggle"
                        onClick={() => setShowDetails(!showDetails)}
                    >
                        <span>{showDetails ? '▼' : '▶'} Details</span>
                    </div>
                    {showDetails && (
                        <div className="event-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'details')
                                .map((f) => renderField(f.key as keyof Event))}
                        </div>
                    )}
                </div>
            </div>
        );
    }


    if (displayMode === 'full') {
        return (
            <div className="event-card event-card--full">
                <h2 className="event-card__title">Event #{entityId}</h2>
                {schema.fields.map((f) => renderField(f.key as keyof Event))}
            </div>
        );
    }

    return null;
}
