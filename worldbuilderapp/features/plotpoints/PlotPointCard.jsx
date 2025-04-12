import React, { useState, useEffect, useRef } from 'react';
import './PlotPointCard.css';
import '../calendar/CalendarDayCell.css';
import TooltipLink from '../entities/TooltipLink';
import { fetchPlotPointEntities } from './PlotPointApi';
import { entitySchemas } from '../entities/entitySchemas';
import { EntityFetcher } from '../entities/entityManager';

export default function PlotPointCard({
    plotPoint,
    onContextMenu,
    onResizeEnd,
    isGhost = false,
    colorIndex = 0
}) {
    const [showTooltip, setShowTooltip] = useState(false);
    const [relatedEntities, setRelatedEntities] = useState({});
    const cardRef = useRef(null);
    const [dragging, setDragging] = useState(null); // { direction: 'start' | 'end', originX: number }
    const draggingRef = useRef(null); // 👈 ADD THIS
    const [previewRange, setPreviewRange] = useState(null); // {start: dayId, end: dayId}



    const handleMouseDown = (e, direction) => {
        e.stopPropagation();
        e.preventDefault();
        setDragging({ direction, originX: e.clientX });
        draggingRef.current = { direction, originX: e.clientX };

        const onMouseMove = (moveEvent) => {
            const elements = document.elementsFromPoint(moveEvent.clientX, moveEvent.clientY);
            let previewDayId = null;

            for (const el of elements) {
                if (el.classList.contains('calendar-cell') && el instanceof HTMLElement) {
                    const datasetId = el.dataset?.dayid;
                    if (datasetId) {
                        previewDayId = parseInt(datasetId);
                        break;
                    }
                }
            }

            // Clear previous highlights
            document.querySelectorAll('.calendar-cell.preview-span').forEach(cell => {
                cell.classList.remove('preview-span');
            });

            if (!previewDayId) {
                setPreviewRange(null);
                return;
            }

            const currentDragging = draggingRef.current;
            const start = currentDragging.direction === 'start' ? previewDayId : plotPoint.startDateId;
            const end = currentDragging.direction === 'end' ? previewDayId : (plotPoint.endDateId || plotPoint.startDateId);

            setPreviewRange({ start: Math.min(start, end), end: Math.max(start, end) });

            for (let day = Math.min(start, end); day <= Math.max(start, end); day++) {
                const cell = document.querySelector(`.calendar-cell[data-dayid="${day}"]`);
                if (cell) {
                    cell.classList.add('preview-span');
                }
            }
        };

        const onMouseUp = async (upEvent) => {
            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);

            // Remove visual highlights
            document.querySelectorAll('.calendar-cell.preview-span').forEach(cell => {
                cell.classList.remove('preview-span');
            });

            const elements = document.elementsFromPoint(upEvent.clientX, upEvent.clientY);
            let newDayId = null;
            setPreviewRange(null);


            for (const el of elements) {
                if (el.classList.contains('calendar-cell') && el instanceof HTMLElement) {
                    const datasetId = el.dataset?.dayid;
                    if (datasetId) {
                        newDayId = parseInt(datasetId);
                        break;
                    }
                }
            }

            const currentDragging = draggingRef.current;
            setDragging(null);
            draggingRef.current = null;

            if (!isNaN(newDayId) && currentDragging) {
                await onResizeEnd?.(plotPoint.id, currentDragging.direction, newDayId);
            }
        };

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    };




    const handleContextMenu = (e) => {
        e.preventDefault();
        onContextMenu?.(e, plotPoint, 'plotpoint');
    };

    const handleDragStart = (e) => {
        console.log('🎯 drag start', plotPoint.id);
        if (isGhost) return;
        e.dataTransfer.setData('plotPointId', plotPoint.id.toString());
        e.dataTransfer.effectAllowed = 'move';
    };



    const handleClickOutside = (e) => {
        if (cardRef.current && !cardRef.current.contains(e.target)) {
            setShowTooltip(false);
        }
    };

    useEffect(() => {
        if (showTooltip) {
            document.addEventListener('mousedown', handleClickOutside);
        } else {
            document.removeEventListener('mousedown', handleClickOutside);
        }
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [showTooltip]);

    useEffect(() => {
        const load = async () => {
            const entities = await fetchPlotPointEntities(plotPoint.id);
            const chapterId = plotPoint.chapterId;

            const all = {};
            for (const [type, _schema] of Object.entries(entitySchemas)) {
                try {
                    const records = await EntityFetcher.fetchAll(type);
                    all[type] = records.filter((e) => e.chapterId === chapterId);
                } catch (err) {
                    console.warn(`⚠️ Failed to load ${type}`, err);
                }
            }

            setRelatedEntities({ ...entities, ...all });
        };

        if (showTooltip && Object.keys(relatedEntities).length === 0) {
            load();
        }
    }, [showTooltip, plotPoint.chapterId, plotPoint.id, relatedEntities]);

    return (
        <div
            ref={cardRef}
            className={[
                'plotpoint-card',
                `color-${colorIndex}`,
                isGhost ? 'ghost' : '',
                dragging ? 'resizing' : ''
            ].join(' ').trim()}
            style={{ left: `${plotPoint.startDateOffset}px`, width: `${plotPoint.duration}px` }}
            data-plotpointid={plotPoint.id}
            data-colorindex={colorIndex}
            draggable={!isGhost}
            onDragStart={handleDragStart}
            onContextMenu={handleContextMenu}
            onClick={() => !isGhost && setShowTooltip(!showTooltip)}
        >
            {/* 🟤 GHOST: Only show title */}
            {isGhost ? (
                <div className="plotpoint-title">{plotPoint.title}</div>
            ) : (
                <>
                    <div
                        className="resize-handle left"
                        onMouseDown={(e) => handleMouseDown(e, 'start')}
                        title="Drag to change start"
                    >
                        ◀
                    </div>

                    <div className="plotpoint-title">{plotPoint.title}</div>
                    <hr className="plotpoint-divider" />
                    <div className="plotpoint-subinfo">{plotPoint.description}</div>

                    {showTooltip && (
                        <div className="plotpoint-tooltip">
                            <strong>Start:</strong> {plotPoint.startDateName} <br />
                            <strong>End:</strong> {plotPoint.endDateName || 'Same Day'} <br />
                            {Object.entries(relatedEntities).map(([type, items]) =>
                                Array.isArray(items) && items.length > 0 ? (
                                    <div key={type} className="tooltip-entity-group">
                                        <strong>{type}:</strong>
                                        <br />
                                        {items.map((ent) => (
                                            <div key={ent.id}>
                                                <TooltipLink
                                                    entityType={type}
                                                    entityId={ent.id}
                                                    displayValue={
                                                        ent.name || ent.title || ent.alias || `[${type} #${ent.id}]`
                                                    }
                                                />
                                            </div>
                                        ))}
                                    </div>
                                ) : null
                            )}
                        </div>
                    )}

                    <div
                        className="resize-handle right"
                        onMouseDown={(e) => handleMouseDown(e, 'end')}
                        title="Drag to change end"
                    >
                        ▶
                        </div>
                        {previewRange && (
                            <div className="plotpoint-preview-tooltip">
                                Spanning days {previewRange.start} → {previewRange.end}
                            </div>
                        )}

                </>
            )}
        </div>
    );
}