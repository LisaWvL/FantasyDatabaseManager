// 📁 src/components/UnassignedSidebar.jsx
import './UnassignedSidebar.css';
import { useState } from 'react';
import {
    useDragAndDrop,
    useDragScroll,
    createDragOverHandler,
    createDragLeaveHandler,
} from '../../utils/dragDropHandlers';
import useEntityRegistry from '../../utils/EntityRegistry';
import { entitySchemas } from '../../utils/entitySchemas';

export default function UnassignedSidebar({
    isSidebarOpen,
    setIsSidebarOpen,
    onContextMenu,
    renderItem,
    entityType,
    items,
    isUnassigned: externalIsUnassigned,
    onDropToUnassigned,
}) {
    const [isOverSidebar, setIsOverSidebar] = useState(false);
    const [selectedIds, setSelectedIds] = useState(new Set());
    const [lastSelectedId, setLastSelectedId] = useState(null);

    const registry = useEntityRegistry.getState();
    //const schema = entitySchemas[entityType];

    const { handleDrop } = useDragAndDrop({
        handleUpdateEntity: ({ entity, dragSourceContext }) => {
            const schema = entitySchemas[entity.entityType];
            const nullableFields = [];

            if (dragSourceContext === 'calendar' && entity.entityType === 'PlotPoint') {
                nullableFields.push('startDateId', 'endDateId');
            }

            const chapterField = schema.fields.find(f => f.fkType === 'Chapter')?.key;
            if (chapterField) nullableFields.push(chapterField);

            nullableFields.forEach(field => {
                registry.setFieldToNull(entity.entityType, entity.id, field);
            });

            onDropToUnassigned?.(entity.id);
        },
    });

    useDragScroll(isOverSidebar);

    const toggleSelection = (e, item) => {
        if (e.shiftKey && lastSelectedId !== null) {
            const currentIndex = items.findIndex(i => i.id === item.id);
            const lastIndex = items.findIndex(i => i.id === lastSelectedId);
            const [start, end] = [Math.min(currentIndex, lastIndex), Math.max(currentIndex, lastIndex)];
            const newIds = items.slice(start, end + 1).map(i => i.id);
            setSelectedIds(new Set([...selectedIds, ...newIds]));
        } else {
            const newSet = new Set(selectedIds);
            newSet.has(item.id) ? newSet.delete(item.id) : newSet.add(item.id);
            setSelectedIds(newSet);
            setLastSelectedId(item.id);
        }
    };

    const isUnassigned = externalIsUnassigned || createUnassignedChecker(entityType);

    return (
        <div className={`unassigned-sidebar-wrapper ${isSidebarOpen ? 'open' : 'collapsed'}`}>
            <div
                className={`unassigned-sidebar ${isSidebarOpen ? 'open' : 'collapsed'} ${isOverSidebar ? 'highlight-drop' : ''}`}
                onDragOver={createDragOverHandler(setIsOverSidebar)}
                onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
                onDrop={(e) => {
                    handleDrop(e, 'unassigned-sidebar');
                    setIsOverSidebar(false);
                }}
            >
                <button
                    className="unassigned-sidebar-toggle"
                    onClick={() => setIsSidebarOpen?.(!isSidebarOpen)}
                >
                    MORE
                </button>

                <h3>Unassigned {capitalize(entityType)}s</h3>
                {items.filter(isUnassigned).map(item => (
                    <div
                        key={item.id}
                        className={`unassigned-item ${selectedIds.has(item.id) ? 'selected' : ''}`}
                        onClick={(e) => toggleSelection(e, item)}
                        onContextMenu={(e) => {
                            e.preventDefault();
                            onContextMenu?.(e, item, entityType);
                        }}
                    >
                        {renderItem(item)}
                    </div>
                ))}
            </div>
        </div>
    );
}

function capitalize(str) {
    return str.charAt(0).toUpperCase?.() + str.slice(1);
}

function createUnassignedChecker(entityType) {
    const schema = entitySchemas[entityType];
    if (!schema) return () => false;
    const chapterField = schema.fields.find(f => f.fkType === 'Chapter')?.key;
    return (item) => chapterField ? !item[chapterField] : false;
}

// 🔮 FUTURE: Add button for bulk actions on selected cards
// 🔮 FUTURE: Add filtering/searching within the unassigned items list
// 🔮 FUTURE: Export button to dump selected IDs to clipboard for mass actions
