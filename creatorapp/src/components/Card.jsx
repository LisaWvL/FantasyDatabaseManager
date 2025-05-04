// 📁 src/components/Card.jsx
import React, { useState } from 'react';
import './Card.css';
import { useDragAndDrop } from '../hooks/useDragAndDrop';
import useResize from '../hooks/useResize';

export default function Card({ card, mode = 'basic', onClick, onContextMenu, onResizeEnd }) {
    const [expanded, setExpanded] = useState(false);
    const [resizing, setResizing] = useState(false);

    const { handleDragStart } = useDragAndDrop({});

    const CardData = card.CardData ?? card.cardData ?? {};
    const DisplayMode = card.DisplayMode ?? card.displayMode ?? mode;
    const Styling = card.Styling ?? card.styling ?? {};

    const {
        color = '#888',
        isGhost = false,
        isReversed = false
    } = Styling;

    const entityType = CardData.EntityType ?? CardData.entityType ?? card.EntityType ?? card.entityType;
    const entityId = CardData.Id;
    const startDateId = CardData.StartDateId;
    const endDateId = CardData.EndDateId ?? startDateId;
    const isSpanning = startDateId !== endDateId;

    const draggable = (DisplayMode === 'compact' || DisplayMode === 'basic' || DisplayMode === 'calendar') && !resizing;

    const { dragging, previewRange, handleMouseDown } = useResize({
        entity: { id: entityId, startDateId, endDateId },
        onResizeEnd: async (entityId, direction, newDayId) => {
            console.log('🎯 Resize ended:', { entityId, direction, newDayId });
            setResizing(false);
            await onResizeEnd?.(entityId, direction, newDayId);
        }
    });



    const cardClass = [
        'entity-card',
        DisplayMode === 'compact' ? 'entity-card-compact' : '',
        DisplayMode === 'basic' ? 'entity-card-basic' : '',
        DisplayMode === 'full' ? 'entity-card-full' : '',
        DisplayMode === 'calendar' ? 'entity-card-calendar' : '',
        isGhost ? 'ghost' : '',
        isReversed ? 'reversed' : '',
        dragging ? 'resizing' : ''
    ].join(' ').trim();

    const formatLabel = (key) =>
        key.replace(/^header/i, '').replace(/^summary/i, '').replace(/^detail/i, '')
            .replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase()).trim();

    const renderField = (key, value) => (
        <div key={key} className="attribute-box">
            <div className="label">{formatLabel(key)}</div>
            <div className="value">{value ?? '—'}</div>
        </div>
    );

    const headerFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('header'));
    const summaryFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('summary'));
    const detailFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('detail'));

    return (
        <div
            className={cardClass}
            style={{ borderColor: color }}
            onClick={onClick}
            draggable={draggable}
            onDragStart={(e) => {
                if (!resizing) {
                    console.log('📦 Drag Start:', { entityType, entityId });
                    handleDragStart(e, { entityType, id: entityId }, 'calendar');
                }
            }}
            onContextMenu={onContextMenu}
        >
            {DisplayMode === 'calendar' && (
                <div className="card-section calendar-section">
                    <div className="card-title">
                        {CardData?.Name || CardData?.Title || CardData?.ChapterTitle || `#${entityId}`}
                    </div>
                    {headerFields.map(([k, v]) => renderField(k, v))}

                    <hr className="divider" />
                    <div className="resize-handles">
                        <div
                            className="resize-handle left"
                            onMouseDown={(e) => {
                                setResizing(true);
                                handleMouseDown(e, 'start');
                            }}
                            title="Drag to change start"
                        >◀</div>
                        <div
                            className="resize-handle right"
                            onMouseDown={(e) => {
                                setResizing(true);
                                handleMouseDown(e, 'end');
                            }}
                            title="Drag to change end"
                        >▶</div>
                    </div>

                    {previewRange && (
                        <div className="preview-tooltip">
                            Spanning days {previewRange.start} → {previewRange.end}
                        </div>
                    )}
                </div>
            )}

            {DisplayMode === 'compact' && (
                <div className="card-section">
                    <h3>{CardData?.Name || CardData?.Title || CardData?.ChapterTitle || `#${entityId}`}</h3>
                    {headerFields.map(([k, v]) => renderField(k, v))}
                </div>
            )}

            {DisplayMode === 'basic' && (
                <>
                    <div className="card-section">
                        <h5 className="section-title">Header</h5>
                        {headerFields.map(([k, v]) => renderField(k, v))}
                    </div>
                    <div className="card-section">
                        <h5 className="section-title">Summary</h5>
                        {summaryFields.map(([k, v]) => renderField(k, v))}
                    </div>
                </>
            )}

            {DisplayMode === 'full' && (
                <>
                    <div className="card-section">
                        <h5 className="section-title">Header</h5>
                        {headerFields.map(([k, v]) => renderField(k, v))}
                    </div>
                    <div className="card-section">
                        <h5 className="section-title">Summary</h5>
                        {summaryFields.map(([k, v]) => renderField(k, v))}
                    </div>

                    {expanded && (
                        <div className="card-section">
                            <h5 className="section-title">Details</h5>
                            {detailFields.map(([k, v]) => renderField(k, v))}
                        </div>
                    )}

                    {detailFields.length > 0 && (
                        <button className="details-toggle" onClick={() => setExpanded(!expanded)}>
                            {expanded ? 'Hide Details' : 'Show Details'}
                        </button>
                    )}
                </>
            )}
        </div>
    );
}


//import React, { useState } from 'react';
//import './Card.css';
//import { useDragAndDrop } from '../hooks/useDragAndDrop';
//import useResize from '../hooks/useResize';

//export default function Card({ card, mode = 'basic', onClick, onUpdate, onContextMenu }) {
//    const [expanded, setExpanded] = useState(false);
//    const { handleDragStart } = useDragAndDrop({});
//    const [resizing, setResizing] = useState(false);


//    const CardData = card.CardData ?? card.cardData ?? {};
//    const DisplayMode = card.DisplayMode ?? card.displayMode ?? mode;
//    const Styling = card.Styling ?? card.styling ?? {};

//    const {
//        color = '#888',
//        isGhost = false,
//        isReversed = false,
//    } = Styling;

//    const entityType = CardData.EntityType ?? card.EntityType ?? card.entityType;
//    const entityId = CardData.Id;
//    const startDateId = CardData.StartDateId;
//    const endDateId = CardData.EndDateId ?? CardData.StartDateId;

//    const draggable = DisplayMode === 'compact' || DisplayMode === 'basic' || DisplayMode === 'calendar';

//    // Setup resize hook
//    const { dragging, previewRange, handleMouseDown } = useResize({
//        entity: { id: entityId, startDateId, endDateId },
//        onResizeEnd: async (entityId, direction, newDayId) => {
//            console.log('🎯 Resize ended:', { entityId, direction, newDayId });
//            // Here you should trigger a real API call or local state update
//            await onUpdate?.({ entityId, direction, newDayId });
//        }
//    });

//    const cardClass = [
//        'entity-card',
//        DisplayMode === 'compact' ? 'entity-card-compact' : '',
//        DisplayMode === 'basic' ? 'entity-card-basic' : '',
//        DisplayMode === 'full' ? 'entity-card-full' : '',
//        DisplayMode === 'calendar' ? 'entity-card-calendar' : '',
//        isGhost ? 'ghost' : '',
//        isReversed ? 'reversed' : ''
//    ].join(' ').trim();

//    const formatLabel = (key) =>
//        key
//            .replace(/^header/i, '')
//            .replace(/^summary/i, '')
//            .replace(/^detail/i, '')
//            .replace(/([A-Z])/g, ' $1')
//            .replace(/^./, str => str.toUpperCase())
//            .trim();

//    const renderField = (key, value) => (
//        <div key={key} className="attribute-box">
//            <div className="label">{formatLabel(key)}</div>
//            <div className="value">{value ?? '—'}</div>
//        </div>
//    );

//    const headerFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('header'));
//    const summaryFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('summary'));
//    const detailFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('detail'));

//    const isSpanning = startDateId !== endDateId;

//    return (
//        <div
//            className={cardClass}
//            style={{ borderColor: color }}
//            onClick={onClick}
//            draggable={!resizing} // Disable dragging while resizing
//            onDragStart={(e) => {
//                if (!resizing) handleDragStart(e, { entityType, id: entityId }, 'calendar');
//            }}
//            onContextMenu={onContextMenu}
//        >
//            <div className="card-header">
//                <h3>{entityType}</h3>
//            </div>

//            {DisplayMode === 'calendar' && (
//                <div className="card-section calendar-section">
//                    <div className="card-title">
//                        {CardData?.Name || CardData?.Title || CardData?.ChapterTitle || `#${entityId}`}
//                    </div>

//                    <hr className="divider" />

//                    {headerFields.map(([k, v]) => renderField(k, v))}

//                    {isSpanning && (
//                        <div className="resize-handles">
//                            <div
//                                className="resize-handle left"
//                                onMouseDown={(e) => handleMouseDown(e, 'start')}
//                                title="Drag to change start"
//                            >
//                                ◀
//                            </div>
//                            <div
//                                className="resize-handle right"
//                                onMouseDown={(e) => handleMouseDown(e, 'end')}
//                                title="Drag to change end"
//                            >
//                                ▶
//                            </div>
//                        </div>
//                    )}

//                    {previewRange && (
//                        <div className="preview-tooltip">
//                            Spanning days {previewRange.start} → {previewRange.end}
//                        </div>
//                    )}
//                </div>
//            )}

//            {DisplayMode === 'compact' && (
//                <div className="card-section">
//                    <h3>{CardData?.Name || CardData?.Title || CardData?.ChapterTitle || `#${entityId}`}</h3>
//                    {headerFields.map(([k, v]) => renderField(k, v))}
//                </div>
//            )}

//            {DisplayMode === 'basic' && (
//                <>
//                    <div className="card-section">
//                        <h5 className="section-title">Header</h5>
//                        {headerFields.map(([k, v]) => renderField(k, v))}
//                    </div>
//                    <div className="card-section">
//                        <h5 className="section-title">Summary</h5>
//                        {summaryFields.map(([k, v]) => renderField(k, v))}
//                    </div>
//                </>
//            )}

//            {DisplayMode === 'full' && (
//                <>
//                    <div className="card-section">
//                        <h5 className="section-title">Header</h5>
//                        {headerFields.map(([k, v]) => renderField(k, v))}
//                    </div>
//                    <div className="card-section">
//                        <h5 className="section-title">Summary</h5>
//                        {summaryFields.map(([k, v]) => renderField(k, v))}
//                    </div>

//                    {expanded && (
//                        <div className="card-section">
//                            <h5 className="section-title">Details</h5>
//                            {detailFields.map(([k, v]) => renderField(k, v))}
//                        </div>
//                    )}

//                    {detailFields.length > 0 && (
//                        <button className="details-toggle" onClick={() => setExpanded(!expanded)}>
//                            {expanded ? 'Hide Details' : 'Show Details'}
//                        </button>
//                    )}
//                </>
//            )}
//        </div>
//    );
//}

