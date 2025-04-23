import { useState } from 'react';
import useEntityRegistry from '../store/useEntityRegistry';
import { EntitySchemas } from '../store/EntitySchemas';
import type { Chapter } from '../types/entities';
import type { DisplayMode, CardContext } from './EntityCardTypes';
import { updateChapter, fetchChapterById } from '../features/chapters/ChapterApi';

export type ChapterCardProps = {
    entityType: 'Chapter';
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
const ChapterCard: React.FC<ChapterCardProps> = ({
    entityId,
    context,
    displayMode,
    DroppedOn,
    Payload,
}) => {
        const { get } = useEntityRegistry();
        const chapter = fetchChapterById('Chapter', entityId);
        const schema = EntitySchemas['Chapter'];

        const [showSummary, setShowSummary] = useState(true);
        const [showDetails, setShowDetails] = useState(true);

        if (!chapter) {
            return (
                <div className="chapter-card chapter-card--loading">
                    Loading Chapter #{entityId}...
                </div>
            );
        }

        const renderField = (key: keyof Chapter) => {
            const value = chapter[key];
            return (
                <div key={String(key)} className="chapter-card__field">
                    <span className="chapter-card__label">{String(key)}:</span>
                    <span className="chapter-card__value">{String(value)}</span>
                </div>
            );
        };

        const handleDragStart = (e: React.DragEvent) => {
            const payload = JSON.stringify({
                entityType: 'Chapter',
                entityId,
                context,
            });
            e.dataTransfer.setData('application/json', payload);
            e.dataTransfer.setData('entityId', String(entityId));
            e.dataTransfer.setData('entityType', 'Chapter'); // e.g., 'chapter', 'chapter', etc.

            e.dataTransfer.effectAllowed = 'move';
        };


        const onDrop = async (e: React.DragEvent) => {
            const raw = e.dataTransfer.getData('application/json');
            if (!raw) return;
            const dragged = JSON.parse(raw);
            const updates: Partial<Chapter> = {};

            // CASE 1: Dropped into a calendar grid cell in Dashboard
            // → Update startDateId and endDateId to match the CalendarDayCell ID
            if (context === 'dashboard') {
                const calendarDayId = Number(e.currentTarget.getAttribute('data-calendar-day-id'));
                if (calendarDayId && dragged.entityType === 'Chapter') {
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
            //        console.log('📌 Placeholder: Add Chapter', dragged.entityId, 'to Chapter', entityId);
            //    }
            //}

            try {
                if (Object.keys(updates).length > 0) {
                    await updateChapter(entityId, updates);
                    console.log(`✅ Chapter ${entityId} updated:`, updates);
                }
            } catch (error) {
                console.error(`❌ Failed to update Chapter ${entityId}`, error);
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
                    className="chapter-card chapter-card--compact"
                >
                    {schema.fields.filter((f) => f.showInCompact).map((f) => renderField(f.key as keyof Chapter))}
                </div>
            );
        }

        if (displayMode === 'basic') {
            return (
                <div
                    {...dragProps}
                    className="chapter-card chapter-card--basic"
                >
                    {/* HEADER */}
                    <div className="chapter-card__section">
                        <div className="chapter-card__section-title">Header</div>
                        {schema.fields
                            .filter((f) => f.section === 'header')
                            .map((f) => renderField(f.key as keyof Chapter))}
                    </div>

                    {/* RELATION */}
                    <div className="chapter-card__section">
                        <div className="chapter-card__section-title">Relation</div>
                        {schema.fields
                            .filter((f) => f.section === 'relation')
                            .map((f) => renderField(f.key as keyof Chapter))}
                    </div>

                    {/* SUMMARY (toggleable) */}
                    <div className="chapter-card__section">
                        <div
                            className="chapter-card__section-title chapter-card__section-toggle"
                            onClick={() => setShowSummary(!showSummary)}
                        >
                            <span>{showSummary ? '▼' : '▶'} Summary</span>
                        </div>
                        {showSummary && (
                            <div className="chapter-card__section-body">
                                {schema.fields
                                    .filter((f) => f.section === 'summary')
                                    .map((f) => renderField(f.key as keyof Chapter))}
                            </div>
                        )}
                    </div>

                    {/* DETAILS (toggleable) */}
                    <div className="chapter-card__section">
                        <div
                            className="chapter-card__section-title chapter-card__section-toggle"
                            onClick={() => setShowDetails(!showDetails)}
                        >
                            <span>{showDetails ? '▼' : '▶'} Details</span>
                        </div>
                        {showDetails && (
                            <div className="chapter-card__section-body">
                                {schema.fields
                                    .filter((f) => f.section === 'details')
                                    .map((f) => renderField(f.key as keyof Chapter))}
                            </div>
                        )}
                    </div>
                </div>
            );
        }


        if (displayMode === 'full') {
            return (
                <div className="chapter-card chapter-card--full">
                    <h2 className="chapter-card__title">Chapter #{entityId}</h2>
                    {schema.fields.map((f) => renderField(f.key as keyof Chapter))}
                </div>
            );
        }

        return null;
    }

export default ChapterCard;