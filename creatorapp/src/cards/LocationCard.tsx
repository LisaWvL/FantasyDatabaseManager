import { useState } from 'react';
import useEntityRegistry from '../store/useEntityRegistry';
import { EntitySchemas } from '../store/EntitySchemas';
import type { Location } from '../types/entities';
import type { DisplayMode, CardContext } from './EntityCardTypes';
import { updateLocation } from '../features/locations/LocationApi';

export type LocationCardProps = {
    entityType: 'Location';
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

export function LocationCard({ entityId, displayMode, context }: LocationCardProps) {
    const { get } = useEntityRegistry();
    const location = get('Location', entityId);
    const schema = EntitySchemas['Location'];

    const [showSummary, setShowSummary] = useState(true);
    const [showDetails, setShowDetails] = useState(true);

    if (!location) {
        return (
            <div className="location-card location-card--loading">
                Loading Location #{entityId}...
            </div>
        );
    }

    const renderField = (key: keyof Location) => {
        const value = location[key];
        return (
            <div key={String(key)} className="location-card__field">
                <span className="location-card__label">{String(key)}:</span>
                <span className="location-card__value">{String(value)}</span>
            </div>
        );
    };

    const handleDragStart = (e: React.DragEvent) => {
        const payload = JSON.stringify({
            entityType: 'Location',
            entityId,
            context,
        });
        e.dataTransfer.setData('application/json', payload);
        e.dataTransfer.setData('entityId', String(entityId));
        e.dataTransfer.setData('entityType', 'Location'); // e.g., 'location', 'event', etc.

        e.dataTransfer.effectAllowed = 'move';
    };


    const onDrop = async (e: React.DragEvent) => {
        const raw = e.dataTransfer.getData('application/json');
        if (!raw) return;
        const dragged = JSON.parse(raw);
        const updates: Partial<Location> = {};

        // CASE 1: Dropped into a calendar grid cell in Dashboard
        // → Update startDateId and endDateId to match the CalendarDayCell ID
        if (context === 'dashboard') {
            const calendarDayId = Number(e.currentTarget.getAttribute('data-calendar-day-id'));
            if (calendarDayId && dragged.entityType === 'Location') {
                //updates.startDateId = calendarDayId;
                //updates.endDateId = calendarDayId;
            }
        }

        // CASE 2: Dropped into UnassignedSidebar while in the Dashboard context
        // → Clear startDateId and endDateId
        if (context === 'unassignedsidebar') {
            if (dragged.context === 'dashboard') {
                //updates.startDateId = null;
                //updates.endDateId = null;
            }
        }

        // CASE 3: Dropped onto a Chapter in ChapterEditor context
        //if (context === 'chaptereditor') {
        //    if (dragged.entityType === 'Chapter') {
        //        // TODO: Implement logic to add this chapterId to the plot point's list of associated chapters
        //        console.log('📌 Placeholder: Add Chapter', dragged.entityId, 'to Location', entityId);
        //    }
        //}

        try {
            if (Object.keys(updates).length > 0) {
                await updateLocation(entityId, updates);
                console.log(`✅ Location ${entityId} updated:`, updates);
            }
        } catch (error) {
            console.error(`❌ Failed to update Location ${entityId}`, error);
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
                className="location-card location-card--compact"
            >
                {schema.fields.filter((f) => f.showInCompact).map((f) => renderField(f.key as keyof Location))}
            </div>
        );
    }

    if (displayMode === 'basic') {
        return (
            <div
                {...dragProps}
                className="location-card location-card--basic"
            >
                {/* HEADER */}
                <div className="location-card__section">
                    <div className="location-card__section-title">Header</div>
                    {schema.fields
                        .filter((f) => f.section === 'header')
                        .map((f) => renderField(f.key as keyof Location))}
                </div>

                {/* RELATION */}
                <div className="location-card__section">
                    <div className="location-card__section-title">Relation</div>
                    {schema.fields
                        .filter((f) => f.section === 'relation')
                        .map((f) => renderField(f.key as keyof Location))}
                </div>

                {/* SUMMARY (toggleable) */}
                <div className="location-card__section">
                    <div
                        className="location-card__section-title location-card__section-toggle"
                        onClick={() => setShowSummary(!showSummary)}
                    >
                        <span>{showSummary ? '▼' : '▶'} Summary</span>
                    </div>
                    {showSummary && (
                        <div className="location-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'summary')
                                .map((f) => renderField(f.key as keyof Location))}
                        </div>
                    )}
                </div>

                {/* DETAILS (toggleable) */}
                <div className="location-card__section">
                    <div
                        className="location-card__section-title location-card__section-toggle"
                        onClick={() => setShowDetails(!showDetails)}
                    >
                        <span>{showDetails ? '▼' : '▶'} Details</span>
                    </div>
                    {showDetails && (
                        <div className="location-card__section-body">
                            {schema.fields
                                .filter((f) => f.section === 'details')
                                .map((f) => renderField(f.key as keyof Location))}
                        </div>
                    )}
                </div>
            </div>
        );
    }


    if (displayMode === 'full') {
        return (
            <div className="location-card location-card--full">
                <h2 className="location-card__title">Location #{entityId}</h2>
                {schema.fields.map((f) => renderField(f.key as keyof Location))}
            </div>
        );
    }

    return null;
}
