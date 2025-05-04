// 📁 hooks/useDragAndDrop.js
import { useEffect, useState } from 'react';

export const createDragOverHandler = (setIsOver) => (e) => {
    e.preventDefault();
    setIsOver?.(true);
};

export const createDragLeaveHandler = (setIsOver) => () => {
    setIsOver?.(false);
};

export function useDragScroll(isDragging) {
    useEffect(() => {
        if (!isDragging) return;

        const scrollMargin = 50;
        const scrollSpeed = 15;

        const handleMouseMove = (e) => {
            if (e.clientY < scrollMargin) {
                window.scrollBy(0, -scrollSpeed);
            } else if (window.innerHeight - e.clientY < scrollMargin) {
                window.scrollBy(0, scrollSpeed);
            }
        };

        document.addEventListener('mousemove', handleMouseMove);
        return () => {
            document.removeEventListener('mousemove', handleMouseMove);
        };
    }, [isDragging]);
}

export function useDragAndDrop({ onDropSuccess }) {
    const [isDragging, setIsDragging] = useState(false);

    const handleDragStart = (e, entity, fromContext) => {
        if (!entity || !entity.entityType || !entity.id || !fromContext) {
            console.warn('❌ Invalid drag start payload');
            return;
        }

        const payload = {
            entityType: entity.entityType,
            entityId: entity.id,
            fromContext
        };

        e.dataTransfer.setData('application/json', JSON.stringify(payload));
        e.dataTransfer.effectAllowed = 'move';
        setIsDragging(true);

        console.log('📦 Drag started:', payload);
    };

    const handleDrop = async (e, dropTargetType, dropTargetId = null) => {
        e.preventDefault();
        setIsDragging(false);

        const raw = e.dataTransfer.getData('application/json');
        if (!raw) return;

        try {
            const { entityType, entityId, fromContext } = JSON.parse(raw);

            const payload = {
                EntityType: entityType,
                Id: entityId,
                DropTargetType: dropTargetType,
                DropTargetId: dropTargetId,
                FromContext: fromContext
            };

            const endpoint =
                dropTargetType === 'unassigned-dropzone'
                    ? '/api/cards/dropToUnassigned'
                    : '/api/cards/drop';

            const res = await fetch(endpoint, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            const result = await res.json();
            console.log(`✅ Drop successful to ${dropTargetType}:`, result);

            await onDropSuccess?.(entityId, dropTargetId, entityType);

        } catch (err) {
            console.error('❌ Drop failed:', err);
        }
    };

    return {
        handleDragStart,
        handleDrop,
        isDragging,
        handleDragOver: (e) => e.preventDefault(),
        handleDragLeave: () => { }
    };
}
