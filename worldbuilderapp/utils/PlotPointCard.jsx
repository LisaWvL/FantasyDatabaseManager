import { useRef, useState, useEffect } from 'react';
import './PlotPointCard.css';
import TooltipLink from '../utils/TooltipLink';
import { usePlotPointDragResize } from '../hooks/usePlotPointDragResize';
import { usePlotPointEntities } from '../hooks/usePlotPointEntities';

export default function PlotPointCard({
    plotPoint,
    onContextMenu,
    onResizeEnd,
    isGhost = false,
    colorIndex = 0,
}) {
    const cardRef = useRef(null);
    const [showTooltip, setShowTooltip] = useState(false);
    const relatedEntities = {
        Chapter: [],
        ...usePlotPointEntities(plotPoint, showTooltip)
    };

    const { dragging, previewRange, handleMouseDown } = usePlotPointDragResize({
        plotPoint,
        onResizeEnd,
    });

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

    const handleDragStart = (e) => {
        if (isGhost) return;
        const idStr = plotPoint.id.toString();
        e.dataTransfer.setData('plotPointId', idStr); // ✅ for calendar drop
        e.dataTransfer.setData('entityId', idStr);    // ✅ for sidebar drop
        e.dataTransfer.effectAllowed = 'move';
    };

    // ✅ Extract chapter title from relatedEntities if loaded
    const chapterTitle = relatedEntities?.Chapter?.[0]?.chapterTitle || null;

    return (
        <div
            ref={cardRef}
            className={[
                'plotpoint-card',
                `color-${colorIndex}`,
                isGhost ? 'ghost' : '',
                dragging ? 'resizing' : '',
                plotPoint.isReversed ? 'reversed' : '',
            ].join(' ')}
            draggable={!isGhost}
            onDragStart={handleDragStart}
            onContextMenu={(e) => onContextMenu?.(e, plotPoint, 'plotpoint')}
            onClick={() => !isGhost && setShowTooltip(!showTooltip)}
        >
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

                    {chapterTitle && (
                        <div className="plotpoint-subinfo">{chapterTitle}</div>
                    )}

                    {showTooltip && (
                        <div className="plotpoint-tooltip">
                            <strong>Start:</strong> {plotPoint.startDateName} <br />
                            <strong>End:</strong> {plotPoint.endDateName || 'Same Day'} <br />
                            {Object.entries(relatedEntities).map(([type, items]) =>
                                Array.isArray(items) && items.length > 0 ? (
                                    <div key={type} className="tooltip-entity-group">
                                        <strong>{type}:</strong><br />
                                        {items.map((ent) => (
                                            <div key={ent.id}>
                                                <TooltipLink
                                                    entityType={type}
                                                    entityId={ent.id}
                                                    displayValue={ent.name || ent.title || ent.alias || `[${type} #${ent.id}]`}
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



//import { useRef, useState, useEffect } from 'react';
//import './PlotPointCard.css';
//import TooltipLink from '../entities/TooltipLink';
//import { usePlotPointDragResize } from './usePlotPointDragResize';
//import { usePlotPointEntities } from './usePlotPointEntities';

//export default function PlotPointCard({
//    plotPoint,
//    onContextMenu,
//    onResizeEnd,
//    isGhost = false,
//    colorIndex = 0,
//}) {
//    const cardRef = useRef(null);
//    const [showTooltip, setShowTooltip] = useState(false);
//    const relatedEntities = usePlotPointEntities(plotPoint, showTooltip);
//    const { dragging, previewRange, handleMouseDown } = usePlotPointDragResize({
//        plotPoint,
//        onResizeEnd,
//    });

//    const handleClickOutside = (e) => {
//        if (cardRef.current && !cardRef.current.contains(e.target)) {
//            setShowTooltip(false);
//        }
//    };

//    useEffect(() => {
//        if (showTooltip) {
//            document.addEventListener('mousedown', handleClickOutside);
//        } else {
//            document.removeEventListener('mousedown', handleClickOutside);
//        }
//        return () => {
//            document.removeEventListener('mousedown', handleClickOutside);
//        };
//    }, [showTooltip]);

//    const handleDragStart = (e) => {
//        if (isGhost) return;
//        e.dataTransfer.setData('plotPointId', plotPoint.id.toString());
//        e.dataTransfer.effectAllowed = 'move';
//    };

//    return (
//        <div
//            ref={cardRef}
//            className={[
//                'plotpoint-card',
//                `color-${colorIndex}`,
//                isGhost ? 'ghost' : '',
//                dragging ? 'resizing' : '',
//                plotPoint.isReversed ? 'reversed' : '', // ✅ Add reversed class
//            ].join(' ')}
//            draggable={!isGhost}
//            onDragStart={handleDragStart}
//            onContextMenu={(e) => {
//                e.preventDefault();
//                onContextMenu?.(e, plotPoint, 'plotpoint');
//            }}
//            onClick={() => !isGhost && setShowTooltip(!showTooltip)}
//        >
//            {isGhost ? (
//                <div className="plotpoint-title">{plotPoint.title}</div>
//            ) : (
//                <>
//                    <div
//                        className="resize-handle left"
//                        onMouseDown={(e) => handleMouseDown(e, 'start')}
//                        title="Drag to change start"
//                    >
//                        ◀
//                    </div>

//                    <div className="plotpoint-title">{plotPoint.title}</div>
//                    <hr className="plotpoint-divider" />

//                    {showTooltip && (
//                        <div className="plotpoint-tooltip">
//                            <strong>Start:</strong> {plotPoint.startDateName} <br />
//                            <strong>End:</strong> {plotPoint.endDateName || 'Same Day'} <br />
//                            {Object.entries(relatedEntities).map(([type, items]) =>
//                                Array.isArray(items) && items.length > 0 ? (
//                                    <div key={type} className="tooltip-entity-group">
//                                        <strong>{type}:</strong><br />
//                                        {items.map((ent) => (
//                                            <div key={ent.id}>
//                                                <TooltipLink
//                                                    entityType={type}
//                                                    entityId={ent.id}
//                                                    displayValue={ent.name || ent.title || ent.alias || `[${type} #${ent.id}]`}
//                                                />
//                                            </div>
//                                        ))}
//                                    </div>
//                                ) : null
//                            )}
//                        </div>
//                    )}

//                    <div
//                        className="resize-handle right"
//                        onMouseDown={(e) => handleMouseDown(e, 'end')}
//                        title="Drag to change end"
//                    >
//                        ▶
//                    </div>

//                    {previewRange && (
//                        <div className="plotpoint-preview-tooltip">
//                            Spanning days {previewRange.start} → {previewRange.end}
//                        </div>
//                    )}
//                </>
//            )}
//        </div>
//    );
//}





