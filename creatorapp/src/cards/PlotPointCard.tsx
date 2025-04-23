// 📁 src/cards/PlotPointCard.tsx
import { useState } from 'react';
import { EntitySchemas } from '../store/EntitySchemas';
import type { PlotPoint } from '../types/entities';
import type { DisplayMode, CardContext } from './EntityCardTypes';
import { updatePlotPoint, fetchPlotPointById } from '../features/plotpoints/PlotPointApi';
import './PlotPointCard.css';

export type PlotPointCardProps = {
    entityType: 'PlotPoint';
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

export function PlotPointCard({
    entityId,
    displayMode,
    context,
    Payload,
    isGhost,
    isReversed,
    colorIndex

}: PlotPointCardProps) {
    const plotPoint = fetchPlotPointById(entityId);
    const schema = EntitySchemas['PlotPoint'];

    const [showSummary, setShowSummary] = useState(true);
    const [showDetails, setShowDetails] = useState(true);

    if (!plotPoint) {
        return (
            <div className="plotpoint-card plotpoint-card--loading">
                Loading PlotPoint #{entityId}...
            </div>
        );
    }

    const renderField = (key: keyof PlotPoint) => {
        const value = plotPoint[key];
        return (
            <div key={String(key)} className="plotpoint-card__field">
                <span className="plotpoint-card__label">{String(key)}:</span>
                <span className="plotpoint-card__value">{String(value)}</span>
            </div>
        );
    };

    const handleDragStart = (e: React.DragEvent) => {
        const payload = JSON.stringify({
            entityType: 'PlotPoint',
            entityId,
            context,
        });
        e.dataTransfer.setData('application/json', payload);
        e.dataTransfer.setData('entityId', String(entityId));
        e.dataTransfer.setData('entityType', 'PlotPoint');
        e.dataTransfer.effectAllowed = 'move';
    };

    const handleDrop = async () => {
        const updates: Partial<PlotPoint> = {};

        if (["dashboard", "context-dropzone", "timeline"].includes(context || '')) {
            if (Payload) {
                const dayId = parseInt(Payload);
                updates.startDateId = dayId;
                updates.endDateId = dayId;
            }
        }

        if (context === 'unassignedsidebar') {
            updates.startDateId = null;
            updates.endDateId = null;
        }

        try {
            if (Object.keys(updates).length > 0) {
                // TODO: Update plot point's date assignment
                await updatePlotPoint(entityId, updates);
                console.log(`✅ PlotPoint ${entityId} updated:`, updates);
            }
        } catch (error) {
            console.error(`❌ Failed to update PlotPoint ${entityId}`, error);
        }
    };

    const dragProps = ['compact', 'basic'].includes(displayMode)
        ? {
            draggable: true,
            onDragStart: handleDragStart,
            onDrop: handleDrop,
            onDragOver: (e: React.DragEvent) => e.preventDefault(),
        }
        : {};

    const colorClass = colorIndex ? `color-${entityId % 10}` : '';

    const cardClass = [
        'plotpoint-card',
        `plotpoint-card--${displayMode}`,
        isGhost ? 'ghost' : '',
        isReversed ? 'reversed' : '',
        colorClass
    ].join(' ').trim();

    if (isGhost) {
        return <div className={cardClass} />;
    }

    if (displayMode === 'compact') {
        return (
            <div {...dragProps} className={cardClass}>
                {schema.fields.filter((f) => f.showInCompact).map((f) => renderField(f.key as keyof PlotPoint))}
            </div>
        );
    }

    if (displayMode === 'basic') {
        return (
            <div {...dragProps} className={cardClass}>
                <div className="plotpoint-card__section">
                    <div className="plotpoint-card__section-title">Header</div>
                    {schema.fields.filter((f) => f.section === 'header').map((f) => renderField(f.key as keyof PlotPoint))}
                </div>

                <div className="plotpoint-card__section">
                    <div className="plotpoint-card__section-title">Relation</div>
                    {schema.fields.filter((f) => f.section === 'relation').map((f) => renderField(f.key as keyof PlotPoint))}
                </div>

                <div className="plotpoint-card__section">
                    <div className="plotpoint-card__section-title plotpoint-card__section-toggle" onClick={() => setShowSummary(!showSummary)}>
                        <span>{showSummary ? '▼' : '▶'} Summary</span>
                    </div>
                    {showSummary && (
                        <div className="plotpoint-card__section-body">
                            {schema.fields.filter((f) => f.section === 'summary').map((f) => renderField(f.key as keyof PlotPoint))}
                        </div>
                    )}
                </div>

                <div className="plotpoint-card__section">
                    <div className="plotpoint-card__section-title plotpoint-card__section-toggle" onClick={() => setShowDetails(!showDetails)}>
                        <span>{showDetails ? '▼' : '▶'} Details</span>
                    </div>
                    {showDetails && (
                        <div className="plotpoint-card__section-body">
                            {schema.fields.filter((f) => f.section === 'details').map((f) => renderField(f.key as keyof PlotPoint))}
                        </div>
                    )}
                </div>
            </div>
        );
    }

    if (displayMode === 'full') {
        return (
            <div className={cardClass}>
                <h2 className="plotpoint-card__title">PlotPoint #{entityId}</h2>
                {schema.fields.map((f) => renderField(f.key as keyof PlotPoint))}
            </div>
        );
    }

    return null;
}