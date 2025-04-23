// 📁 components/Card.jsx
import React, { useState } from 'react';
import './Card.css';

export default function Card({ card, mode = 'basic', onClick }) {
    const [expanded, setExpanded] = useState(false);

    const CardData = card.CardData ?? card.cardData ?? {};
    const DisplayMode = card.DisplayMode ?? card.displayMode ?? mode;
    const Styling = card.Styling ?? card.styling ?? {};

    const {
        color = '#888',
        isGhost = false,
        isReversed = false,
    } = Styling;


    const cardClass = [
        'card',
        DisplayMode === 'compact' ? 'card--compact' : '',
        DisplayMode === 'basic' ? 'card--basic' : '',
        DisplayMode === 'full' ? 'card--full' : '',
        isGhost ? 'card--ghost' : '',
        isReversed ? 'card--reversed' : ''
    ].join(' ');

    const renderField = (key, value) => (
        <div key={key} className="field">
            <span className="label">{formatLabel(key)}</span>
            <span className="value">{value ?? '—'}</span>
        </div>
    );

    const formatLabel = (key) =>
        key
            .replace(/^header/i, '')
            .replace(/^summary/i, '')
            .replace(/^detail/i, '')
            .replace(/([A-Z])/g, ' $1')
            .replace(/^./, str => str.toUpperCase())
            .trim();

    const headerFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('header'));
    const summaryFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('summary'));
    const detailFields = Object.entries(CardData).filter(([k]) => k.toLowerCase().startsWith('detail'));

    return (
        <div
            className={cardClass}
            style={{ borderColor: color }}
            onClick={onClick}
        >
            <div className="header">
                <span className="title">
                    {CardData?.Name || CardData?.Title || CardData?.ChapterTitle || `#${CardData?.Id ?? CardData?.id ?? card.id}`}
                </span>
            </div>

            {DisplayMode === 'compact' && (
                <div className="body compact-body">
                    {headerFields.map(([k, v]) => renderField(k, v))}
                </div>
            )}

            {DisplayMode === 'basic' && (
                <div className="body basic-body">
                    {headerFields.map(([k, v]) => renderField(k, v))}
                    {summaryFields.map(([k, v]) => renderField(k, v))}
                </div>
            )}

            {DisplayMode === 'full' && (
                <div className="body full-body">
                    {headerFields.map(([k, v]) => renderField(k, v))}
                    {summaryFields.map(([k, v]) => renderField(k, v))}

                    {expanded && (
                        <div className="detail-section">
                            {detailFields.map(([k, v]) => renderField(k, v))}
                        </div>
                    )}

                    {detailFields.length > 0 && (
                        <button className="toggle-edit" onClick={() => setExpanded(!expanded)}>
                            {expanded ? 'Hide Details' : 'Show Details'}
                        </button>
                    )}
                </div>
            )}
        </div>
    );
}







//import { useState, useEffect, useRef } from 'react';
//import { entitySchemas } from '../store/EntitySchemas';
//import {
//    EntityFetcher,
//    EntityUpdater,
//    EntityDeleter,
//    EntityCreator
//} from '../store/EntityManager';
//import { fetchFlatEntity } from '../api/api'; // adjust path if needed
//import TooltipLink from '../utils/TooltipLink';
//import ContextMenu from '../utils/ContextMenu';
//import ConfirmDialog from '../utils/ConfirmDialog';
//import { usePlotPointDragResize } from '../hooks/usePlotPointDragResize';
//import './Card.css';

//const cardColors = {
//    character: '#a368ff',
//    chapter: '#ff8a65',
//    era: '#ffb74d',
//    scene: '#4fc3f7',
//    faction: '#ffb74d',
//    item: '#81c784',
//    event: '#f06292',
//    location: '#64b5f6',
//    language: '#7986cb',
//    plotpoint: '#9575cd',
//    relationship: '#ffd54f',
//    river: '#64b5f6',
//    route: '#7986cb',
//    default: '#888'
//};

//export default function Card({
//    entity,
//    entityType,
//    displayMode = 'full',
//    onFieldUpdate,
//    onDelete,
//    onCreateNewVersion,
//    onDragStart,
//    onDragEnd,
//    onContextMenu,
//    draggable = true,
//    isGhost = false,
//    isReversed = false,
//    onResizeEnd = null,
//}) {
//    const schema = entitySchemas[entityType];
//    const [localEntity, setLocalEntity] = useState({ ...entity });
//    const [isEditMode, setIsEditMode] = useState(false);
//    const [dropdownData, setDropdownData] = useState({});
//    const [contextMenuPos, setContextMenuPos] = useState(null);
//    const [confirmDelete, setConfirmDelete] = useState(false);
//    const isFirstRender = useRef(true);

//    const colorBase = cardColors[entityType.toLowerCase()] || cardColors.default;
//    const backgroundTint = displayMode === 'date' ? `${colorBase}44` : 'transparent';
//    const isDate = displayMode === 'date';

//    const {
//        handleMouseDown: resizeMouseDown,
//        previewRange,
//        dragging
//    } = usePlotPointDragResize({
//        plotPoint: entity,
//        onResizeEnd
//    });

//    const handleMouseDown = isDate && onResizeEnd ? resizeMouseDown : undefined;

//    useEffect(() => {
//        const loadDropdownData = async () => {
//            const result = {};
//            const fkFields = schema.fields.filter(f => f.type === 'fk' || f.type === 'multiFk');

//            for (const field of fkFields) {
//                const fkType = field.fkType;
//                try {
//                    result[field.key] = await EntityFetcher.fetchAll(fkType);
//                } catch (err) {
//                    console.warn(`🔁 Fallback to flat fetch for ${fkType}`, err);
//                    try {
//                        const flat = await fetchFlatEntity(fkType);
//                        result[field.key] = flat;
//                    } catch (flatErr) {
//                        console.error(`❌ Dropdown flat fetch failed for ${fkType}`, flatErr);
//                        result[field.key] = [];
//                    }
//                }
//            }

//            setDropdownData(result);
//        };

//        loadDropdownData();
//    }, [schema]);

//    const handleChange = (field, value) => {
//        const updated = { ...localEntity, [field.key]: value };
//        setLocalEntity(updated);
//        onFieldUpdate?.(field.key, value);
//    };

//    const handleBlur = () => {
//        if (!isFirstRender.current) {
//            setIsEditMode(false);
//        }
//        isFirstRender.current = false;
//    };

//    const handleContext = (e) => {
//        e.preventDefault();
//        setContextMenuPos({ x: e.pageX, y: e.pageY });
//        onContextMenu?.(e, entity);
//    };

//    const renderField = (field) => {
//        const value = localEntity[field.key];
//        const options = dropdownData[field.key] || [];

//        if (isEditMode) {
//            if (field.type === 'fk') {
//                return (
//                    <div className="field" key={field.key}>
//                        <label>{field.label}</label>
//                        <select
//                            value={value || ''}
//                            onChange={(e) => handleChange(field, parseInt(e.target.value))}
//                            onBlur={handleBlur}
//                        >
//                            <option value=''>Select...</option>
//                            {options.map(opt => (
//                                <option key={opt.id} value={opt.id}>
//                                    {opt.name || opt.title || opt.alias}
//                                </option>
//                            ))}
//                        </select>
//                    </div>
//                );
//            } else if (field.type === 'multiFk') {
//                return (
//                    <div className="field" key={field.key}>
//                        <label>{field.label}</label>
//                        <select
//                            multiple
//                            value={value || []}
//                            onChange={(e) =>
//                                handleChange(
//                                    field,
//                                    Array.from(e.target.selectedOptions, o => parseInt(o.value))
//                                )
//                            }
//                            onBlur={handleBlur}
//                        >
//                            {options.map(opt => (
//                                <option key={opt.id} value={opt.id}>
//                                    {opt.name || opt.title || opt.alias}
//                                </option>
//                            ))}
//                        </select>
//                    </div>
//                );
//            } else {
//                return (
//                    <div className="field" key={field.key}>
//                        <label>{field.label}</label>
//                        <textarea
//                            value={value || ''}
//                            onChange={(e) => handleChange(field, e.target.value)}
//                            onBlur={handleBlur}
//                        />
//                    </div>
//                );
//            }
//        } else {
//            if (field.type === 'fk') {
//                return (
//                    <div className="field" key={field.key}>
//                        <TooltipLink
//                            entityType={field.fkType}
//                            entityId={value}
//                            displayValue={
//                                entity[`${field.key.replace(/Id$/, '')}Name`] || `(${field.label})`
//                            }
//                        />
//                    </div>
//                );
//            } else if (field.type === 'multiFk') {
//                return (
//                    <div className="field" key={field.key}>
//                        <span>{field.label}:</span>
//                        {(value || []).map(id => (
//                            <TooltipLink
//                                key={id}
//                                entityType={field.fkType}
//                                entityId={id}
//                                displayValue={`#${id}`}
//                            />
//                        ))}
//                    </div>
//                );
//            } else {
//                return (
//                    <div className="field" key={field.key}>
//                        <label>{field.label}</label>
//                        <span>{value}</span>
//                    </div>
//                );
//            }
//        }
//    };

//    const renderSection = (section) => {
//        const fields = schema.fields.filter(f => f.section === section);
//        if (!fields.length) return null;
//        return <div className={`section ${section}`}>{fields.map(renderField)}</div>;
//    };

//    return (
//        <>
//            <div
//                className={[
//                    'entity-card',
//                    entityType.toLowerCase(),
//                    displayMode,
//                    isGhost ? 'ghost' : '',
//                    isReversed ? 'reversed' : '',
//                    dragging ? 'resizing' : ''
//                ].join(' ')}
//                style={{
//                    backgroundColor: backgroundTint,
//                    borderColor: colorBase
//                }}
//                draggable={draggable}
//                onDragStart={(e) => {
//                    e.dataTransfer.setData('entityId', entity.id);
//                    e.dataTransfer.setData('entityType', entityType);
//                    onDragStart?.(e);
//                }}
//                onDragEnd={onDragEnd}
//                onContextMenu={handleContext}
//            >
//                {isDate && !isGhost && (
//                    <>
//                        <div
//                            className="resize-handle left"
//                            onMouseDown={(e) => handleMouseDown?.(e, 'start')}
//                            title="Drag to change start"
//                        >
//                            ◀
//                        </div>
//                        <div
//                            className="resize-handle right"
//                            onMouseDown={(e) => handleMouseDown?.(e, 'end')}
//                            title="Drag to change end"
//                        >
//                            ▶
//                        </div>
//                    </>
//                )}

//                <div className="card-header">
//                    <div className="card-title">
//                        <h3>{schema.primaryDisplay(localEntity)}</h3>
//                    </div>
//                </div>

//                {['header', 'relation'].includes(displayMode) && renderSection('header')}
//                {['full', 'interactive'].includes(displayMode) && renderSection('relation')}
//                {displayMode === 'full' && (
//                    <>
//                        {renderSection('summary')}
//                        {renderSection('details')}
//                    </>
//                )}
//                {displayMode === 'compact' && (
//                    <div className="compact-label">
//                        {localEntity.name || localEntity.title || `#${entity.id}`}
//                    </div>
//                )}
//            </div>

//            {previewRange && isDate && (
//                <div className="plotpoint-preview-tooltip">
//                    Spanning days {previewRange.start} → {previewRange.end}
//                </div>
//            )}

//            {contextMenuPos && (
//                <ContextMenu
//                    x={contextMenuPos.x}
//                    y={contextMenuPos.y}
//                    onClose={() => setContextMenuPos(null)}
//                    onClone={() => {
//                        onCreateNewVersion?.(entityType, entity);
//                        setContextMenuPos(null);
//                    }}
//                    onEdit={() => {
//                        setIsEditMode(true);
//                        setContextMenuPos(null);
//                    }}
//                    onDelete={() => {
//                        setConfirmDelete(true);
//                        setContextMenuPos(null);
//                    }}
//                />
//            )}

//            {confirmDelete && (
//                <ConfirmDialog
//                    title={`Delete ${schema.primaryDisplay(entity)}?`}
//                    message="This action cannot be undone."
//                    onCancel={() => setConfirmDelete(false)}
//                    onConfirm={() => {
//                        onDelete?.(entityType, entity.id);
//                        setConfirmDelete(false);
//                    }}
//                />
//            )}
//        </>
//    );
//}
