// 📁 src/components/Card.jsx
import { useState, useEffect, useRef } from 'react';
import { entitySchemas } from '../../utils/entitySchemas';
import { EntityFetcher } from '../../utils/EntityManager';
import { fetchFlatEntity } from '../../src/api/api'; // adjust path if needed
import TooltipLink from '../../utils/TooltipLink';
import useEntityRegistry from '../../utils/EntityRegistry';
import ContextMenu from '../../utils/ContextMenu';
import ConfirmDialog from '../../utils/ConfirmDialog';
import { usePlotPointDragResize } from '../../hooks/usePlotPointDragResize';
import './Card.css';

const typeColors = {
    character: '#a368ff',
    chapter: '#ff8a65',
    era: '#ffb74d',
    scene: '#4fc3f7',
    faction: '#ffb74d',
    item: '#81c784',
    event: '#f06292',
    location: '#64b5f6',
    language: '#7986cb',
    plotpoint: '#9575cd',
    relationship: '#ffd54f',
    default: '#888'
};

export default function Card({
    entity,
    entityType,
    displayMode = 'full',
    onFieldUpdate,
    onDelete,
    onCreateNewVersion,
    onDragStart,
    onDragEnd,
    onContextMenu,
    draggable = true,
    isGhost = false,
    isReversed = false,
    //colorIndex = 0,
    onResizeEnd = null, // ✅ make optional with default
}) {
    const schema = entitySchemas[entityType];
    const [localEntity, setLocalEntity] = useState({ ...entity });
    const [isEditMode, setIsEditMode] = useState(false);
    const [dropdownData, setDropdownData] = useState({});
    const [contextMenuPos, setContextMenuPos] = useState(null);
    const [confirmDelete, setConfirmDelete] = useState(false);
    const isFirstRender = useRef(true);

    const colorBase = typeColors[entityType.toLowerCase()] || typeColors.default;
    const backgroundTint = displayMode === 'calendar' ? `${colorBase}44` : 'transparent';

    const isCalendar = displayMode === 'calendar';

    // Always call the hook, even if it's not used right now
    const {
        handleMouseDown: resizeMouseDown,
        previewRange,
        dragging
    } = usePlotPointDragResize({
        plotPoint: entity,
        onResizeEnd
    });

    const handleMouseDown = displayMode === 'calendar' && onResizeEnd ? resizeMouseDown : undefined;



    useEffect(() => {
        const registry = useEntityRegistry.getState(); // ✅ direct registry access (or use a prop if you already have it)
        const fkFields = schema.fields.filter(f => f.type === 'fk' || f.type === 'multiFk');

        const load = async () => {
            const data = {};

            for (const f of fkFields) {
                const fkType = f.fkType;
                const cached = registry.cache?.[fkType];

                if (cached && Object.keys(cached).length > 0) {
                    data[f.key] = Object.values(cached);
                    continue;
                }

                try {
                    // Try standard EntityManager fetch
                    const opts = await EntityFetcher.fetchAll(fkType);
                    data[f.key] = opts;

                    // Optional: also write to registry if you want to hot-cache
                    registry.cache[fkType] = Object.fromEntries(opts.map(i => [i.id, i]));
                } catch (err) {
                    console.warn(`🔁 Fallback to flat fetch for ${fkType}`);
                    try {
                        const flat = await fetchFlatEntity(fkType);
                        data[f.key] = flat;
                    } catch (flatErr) {
                        console.error(`❌ Dropdown flat fetch failed for ${fkType}`, flatErr);
                    }
                }
            }

            setDropdownData(data);
        };

        load();
    }, [schema]);


    const handleChange = (field, value) => {
        const updated = { ...localEntity, [field.key]: value };
        setLocalEntity(updated);
        onFieldUpdate?.(field.key, value);
    };

    const handleBlur = () => {
        if (!isFirstRender.current) {
            setIsEditMode(false);
        }
        isFirstRender.current = false;
    };

    const handleContext = (e) => {
        e.preventDefault();
        setContextMenuPos({ x: e.pageX, y: e.pageY });
        onContextMenu?.(e, entity);
    };

    const renderField = (field) => {
        const value = localEntity[field.key];
        if (isEditMode) {
            const options = dropdownData[field.key] || [];
            if (field.type === 'fk') {
                return (
                    <div className="field" key={field.key}>
                        <label>{field.label}</label>
                        <select
                            value={value || ''}
                            onChange={(e) => handleChange(field, parseInt(e.target.value))}
                            onBlur={handleBlur}
                        >
                            <option value=''>Select...</option>
                            {options.map(opt => (
                                <option key={opt.id} value={opt.id}>{opt.name || opt.title || opt.alias}</option>
                            ))}
                        </select>
                    </div>
                );
            } else if (field.type === 'multiFk') {
                return (
                    <div className="field" key={field.key}>
                        <label>{field.label}</label>
                        <select
                            multiple
                            value={value || []}
                            onChange={(e) => handleChange(field, Array.from(e.target.selectedOptions, o => parseInt(o.value)))}
                            onBlur={handleBlur}
                        >
                            {options.map(opt => (
                                <option key={opt.id} value={opt.id}>{opt.name || opt.title || opt.alias}</option>
                            ))}
                        </select>
                    </div>
                );
            } else {
                return (
                    <div className="field" key={field.key}>
                        <label>{field.label}</label>
                        <textarea
                            value={value || ''}
                            onChange={(e) => handleChange(field, e.target.value)}
                            onBlur={handleBlur}
                        />
                    </div>
                );
            }
        } else {
            if (field.type === 'fk') {
                return (
                    <div className="field" key={field.key}>
                        <TooltipLink
                            entityType={field.fkType}
                            entityId={value}
                            displayValue={entity[`${field.key.replace(/Id$/, '')}Name`] || `(${field.label})`}
                        />
                    </div>
                );
            } else if (field.type === 'multiFk') {
                return (
                    <div className="field" key={field.key}>
                        <span>{field.label}:</span>
                        {(value || []).map(id => (
                            <TooltipLink
                                key={id}
                                entityType={field.fkType}
                                entityId={id}
                                displayValue={`#${id}`}
                            />
                        ))}
                    </div>
                );
            } else {
                return (
                    <div className="field" key={field.key}>
                        <label>{field.label}</label>
                        <span>{value}</span>
                    </div>
                );
            }
        }
    };

    const renderSection = (section) => {
        const fields = schema.fields.filter(f => f.section === section);
        if (!fields.length) return null;
        return (
            <div className={`section ${section}`}>{fields.map(renderField)}</div>
        );
    };

    return (
        <>
            <div
                className={[
                    'entity-card',
                    entityType.toLowerCase(),
                    displayMode,
                    isGhost ? 'ghost' : '',
                    isReversed ? 'reversed' : '',
                    dragging ? 'resizing' : ''
                ].join(' ')}
                style={{
                    backgroundColor: backgroundTint,
                    borderColor: colorBase
                }}
                draggable={draggable}
                onDragStart={(e) => {
                    e.dataTransfer.setData('entityId', entity.id);
                    e.dataTransfer.setData('entityType', entityType);
                    onDragStart?.(e);
                }}
                onDragEnd={onDragEnd}
                onContextMenu={handleContext}
            >
                {isCalendar && !isGhost && (
                    <>
                        <div
                            className="resize-handle left"
                            onMouseDown={(e) => handleMouseDown?.(e, 'start')}
                            title="Drag to change start"
                        >
                            ◀
                        </div>
                        <div className="resize-handle right"
                            onMouseDown={(e) => handleMouseDown?.(e, 'end')}
                            title="Drag to change end"
                        >
                            ▶
                        </div>
                    </>
                )}

                <div className="card-header">
                    <div className="card-title">
                        <h3>{schema.primaryDisplay(localEntity)}</h3>
                    </div>
                </div>

                {['header', 'relation'].includes(displayMode) && renderSection('header')}
                {['full', 'interactive'].includes(displayMode) && renderSection('relation')}
                {displayMode === 'full' && (
                    <>
                        {renderSection('summary')}
                        {renderSection('details')}
                    </>
                )}
                {displayMode === 'compact' && (
                    <div className="compact-label">{localEntity.name || localEntity.title || `#${entity.id}`}</div>
                )}
            </div>

            {previewRange && isCalendar && (
                <div className="plotpoint-preview-tooltip">
                    Spanning days {previewRange.start} → {previewRange.end}
                </div>
            )}

            {contextMenuPos && (
                <ContextMenu
                    x={contextMenuPos.x}
                    y={contextMenuPos.y}
                    onClose={() => setContextMenuPos(null)}
                    onClone={() => {
                        onCreateNewVersion?.(entityType, entity);
                        setContextMenuPos(null);
                    }}
                    onEdit={() => {
                        setIsEditMode(true);
                        setContextMenuPos(null);
                    }}
                    onDelete={() => {
                        setConfirmDelete(true);
                        setContextMenuPos(null);
                    }}
                />
            )}

            {confirmDelete && (
                <ConfirmDialog
                    title={`Delete ${schema.primaryDisplay(entity)}?`}
                    message="This action cannot be undone."
                    onCancel={() => setConfirmDelete(false)}
                    onConfirm={() => {
                        onDelete?.(entityType, entity.id);
                        setConfirmDelete(false);
                    }}
                />
            )}
        </>
    );
}
