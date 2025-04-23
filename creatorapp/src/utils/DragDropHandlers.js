// 📁 src/hooks/DragAndDropHandler.js
import { useEffect } from 'react';
//import useEntityRegistry from '../store/useEntityRegistry';

/**
 * Enables smooth scrolling when dragging near the top/bottom of the screen.
 */
export function useDragScroll(isDragging) {
    useEffect(() => {
        if (!isDragging) return;

        const scrollMargin = 50;
        const scrollSpeed = 10;

        const handleMouseMove = (e) => {
            if (e.clientY < scrollMargin) {
                window.scrollBy(0, -scrollSpeed);
            } else if (window.innerHeight - e.clientY < scrollMargin) {
                window.scrollBy(0, scrollSpeed);
            }
        };

        const handleWheel = (e) => {
            window.scrollBy({
                top: e.deltaY,
                behavior: 'smooth',
            });
        };

        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('wheel', handleWheel, { passive: false });

        return () => {
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('wheel', handleWheel);
        };
    }, [isDragging]);
}

/**
 * Core logic hook for drag and drop behavior.
 */
export function useDragAndDrop({ handleUpdateEntity }) {
    //const registry = useEntityRegistry.getState();

    const handleDragStart = (e, entity, context = 'default', extra = {}) => {
        if (!entity || !entity.entityType || !entity.id) {
            console.warn('❌ Invalid drag entity:', entity);
            return;
        }

        e.dataTransfer.setData('entityType', entity.entityType);
        e.dataTransfer.setData('entityId', entity.id.toString());
        e.dataTransfer.setData('dragSourceContext', context);

        Object.entries(extra).forEach(([key, value]) => {
            e.dataTransfer.setData(key, value);
        });

        e.dataTransfer.effectAllowed = 'move';
    };

    const handleDrop = async (e, dropTarget, contextData = {}) => {
        e.preventDefault();

        const entityType = e.dataTransfer.getData('entityType');
        const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
        const dragSourceContext = e.dataTransfer.getData('dragSourceContext');

        if (!entityType || !entityId) {
            console.warn('❌ Missing drop data');
            return;
        }

        const entity = { id: entityId, entityType };
        const payload = {};
        let updateTargetType = entityType;
        let updateTargetId = entityId;

        // Specific drop rules
        if (dropTarget === 'POV-dropzone' && entityType === 'Character') {
            const chapterId = e.currentTarget.closest('[data-chapter-id]')?.dataset?.chapterId;
            if (!chapterId) {
                console.warn('❌ No chapterId found for POV drop');
                return;
            }
            updateTargetType = 'Chapter';
            updateTargetId = parseInt(chapterId);
            payload.povCharacterId = entity.id;
        } else if (dropTarget === 'context-dropzone' || dropTarget === 'unassigned-sidebar') {
            const chapterId = dropTarget === 'unassigned-sidebar'
                ? null
                : parseInt(e.currentTarget.closest('[data-chapter-id]')?.dataset?.chapterId || '');
            payload.chapterId = chapterId;
        } else if (dropTarget === 'relationship-zone') {
            // 🔮 Future zone: If the drop occurs into a character relationship area
            const relatedId = e.dataTransfer.getData('relatedEntityId');
            if (relatedId) {
                payload.relatedCharacterId = parseInt(relatedId);
            }
        }

        try {
            await registry.update(updateTargetType, updateTargetId, payload);
            handleUpdateEntity?.({ entity, payload, updateTargetType, updateTargetId, dropTarget, dragSourceContext, contextData });
        } catch (err) {
            console.error(`❌ Failed to update entity after drop`, err);
        }
    };

    return {
        handleDragStart,
        handleDrop,
    };
}

/**
 * Allows setting visual highlight state when dragging over a zone
 */
export const createDragOverHandler = (setIsOver) => (e) => {
    e.preventDefault();
    setIsOver?.(true);
};

/**
 * Clears highlight state when dragging leaves a zone
 */
export const createDragLeaveHandler = (setIsOver) => () => {
    setIsOver?.(false);
};

// 🔮 FUTURE UTILITY: You may want to expose a drop context parser
// export function parseDragMetadata(e) {
//     return {
//         entityType: e.dataTransfer.getData('entityType'),
//         entityId: parseInt(e.dataTransfer.getData('entityId'), 10),
//         sourceContext: e.dataTransfer.getData('dragSourceContext'),
//         intent: e.dataTransfer.getData('dropIntent'),
//     };
// }

// 🔮 FUTURE UTILITY: Batch drop for multi-selection
// export function handleBatchDrop(entities, target, context) {
//     entities.forEach(entity => handleDropForEntity(entity, target, context));
// }
