import { useState } from 'react';
import useEntityRegistry from '../store/useEntityRegistry';
import { EntitySchemas } from '../store/EntitySchemas';
import type { Era } from '../types/entities';
import type { DisplayMode, CardContext } from './EntityCardTypes';
import { updateEra } from '../features/eras/EraApi';

export type EraCardProps = {
    entityType: 'Era';
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

export function EraCard({ entityId, displayMode, context }: EraCardProps) {
    const { get } = useEntityRegistry();
    const era = get('Era', entityId);
    const schema = EntitySchemas['Era'];

    const [showSummary, setShowSummary] = useState(true);
    const [showDetails, setShowDetails] = useState(true);

    if (!era) {
        return (
            <div className="era-card era-card--loading">
                Loading Era #{entityId}...
            </div>
        );
    }

    const renderField = (key: keyof Era) => {
        const value = era[key];
        return (
            <div key={String(key)} className="era-card__field">
                <span className="era-card__label">{String(key)}:</span>
                <span className="era-card__value">{String(value)}</span>
            </div>
        );
    };

    const handleDragStart = (e: React.DragEvent) => {
        const payload = JSON.stringify({
            entityType: 'Era',
            entityId,
            context,
        });
        e.dataTransfer.setData('application/json', payload);
        e.dataTransfer.setData('entityId', String(entityId));
        e.dataTransfer.setData('entityType', 'Era'); // e.g., 'era', 'event', etc.

        e.dataTransfer.effectAllowed = 'move';
    };


    const onDrop = async (e: React.DragEvent) => {
        const raw = e.dataTransfer.getData('application/json');
        if (!raw) return;
        const dragged = JSON.parse(raw);
        const updates: Partial<Era> = {};

        // CASE 1: Dropped into a calendar grid cell in Dashboard
        // → Update startDateId and endDateId to match the CalendarDayCell ID
        if (context === 'dashboard') {
            const calendarDayId = Number(e.currentTarget.getAttribute('data-calendar-day-id'));
            if (calendarDayId && dragged.entityType === 'Era') {
                updates.startDateId = calendarDayId;
                updates.endDateId = calendarDayId;
            }
        }

        // CASE 2: Dropped into UnassignedSidebar while in the Dashboard context
        // → Clear startDateId and endDateId
        if (context === 'unassignedsidebar') {
            if (dragged.context === 'dashboard') {
                updates.startDateId = null;
                updates.endDateId = null;
            }
        }

        // CASE 3: Dropped onto a Chapter in ChapterEditor context
        //if (context === 'chaptereditor') {
        //    if (dragged.entityType === 'Chapter') {
        //        // TODO: Implement logic to add this chapterId to the plot point's list of associated chapters
        //        console.log('📌 Placeholder: Add Chapter', dragged.entityId, 'to Era', entityId);
        //    }
        //}

        try {
            if (Object.keys(updates).length > 0) {
                await updateEra(entityId, updates);
                console.log(`✅ Era ${entityId} updated:`, updates);
            }
        } catch (error) {
            console.error(`❌ Failed to update Era ${entityId}`, error);
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
                className="era-card era-card--compact"
            >
                {schema.fields.filter((f) => f.showInCompact).map((f) => renderField(f.key as keyof Era))}
            </div>
        );
    }

    if (displayMode === 'basic') {
        return (
            <div
                {...dragProps}
                className="era-card era-card--basic"
            >
                {/* HEADER */}
                <div className="era-card__section">
                    <div className="era-card__section-title">Header</div>
                    {schema.fields
                        .filter((f) => f.section === 'header')
                        .map((f) => renderField(f.key as keyof Era))}
                </div>

                {/* RELATION */}
                <div className="era-card__section">
                    <div className="era-card__section-title">Relation</div>
                    {schema.fields
                        .filter((f) => f.section === 'relation')
                        .map((f) => renderField(f.key as keyof Era))}
                </div>

                {/* SUMMARY (toggleable) */}
                <div className="era-card__section">
                    <div
                        className="era-card__section-title era-card__section-toggle"
                        onClick={() => setShowSummary(!showSummary)}
                    >
                        <span>{showSummary ? '▼' : '▶'} Summary</span>
                    </div>
                    {showSummary && (
                        <div className="era-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'summary')
                                .map((f) => renderField(f.key as keyof Era))}
                        </div>
                    )}
                </div>

                {/* DETAILS (toggleable) */}
                <div className="era-card__section">
                    <div
                        className="era-card__section-title era-card__section-toggle"
                        onClick={() => setShowDetails(!showDetails)}
                    >
                        <span>{showDetails ? '▼' : '▶'} Details</span>
                    </div>
                    {showDetails && (
                        <div className="era-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'details')
                                .map((f) => renderField(f.key as keyof Era))}
                        </div>
                    )}
                </div>
            </div>
        );
    }


    if (displayMode === 'full') {
        return (
            <div className="era-card era-card--full">
                <h2 className="era-card__title">Era #{entityId}</h2>
                {schema.fields.map((f) => renderField(f.key as keyof Era))}
            </div>
        );
    }

    return null;
}
