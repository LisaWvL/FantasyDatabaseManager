// 📁 hooks/useResize.js
import { useState, useCallback, useEffect } from 'react';

export default function useResize({ entity, onResizeEnd }) {
    const [dragging, setDragging] = useState(false);
    const [direction, setDirection] = useState(null);
    const [previewRange, setPreviewRange] = useState(null);
    const [startPos, setStartPos] = useState(null);

    const handleMouseDown = useCallback((e, resizeDirection) => {
        e.preventDefault();
        setDragging(true);
        setDirection(resizeDirection);
        setStartPos({ x: e.clientX, y: e.clientY });
        
        // Initial preview
        setPreviewRange({
            start: entity.startDateId,
            end: entity.endDateId ?? entity.startDateId
        });

        // Add global mouse handlers
        const handleMouseMove = (e) => {
            const dayElement = findClosestDayElement(e.target);
            if (!dayElement) return;

            const newDayId = parseInt(dayElement.dataset.dayid);
            if (isNaN(newDayId)) return;

            let newStart = entity.startDateId;
            let newEnd = entity.endDateId ?? entity.startDateId;

            if (direction === 'start') {
                newStart = newDayId;
                // Ensure start doesn't go beyond end
                if (newStart > newEnd) {
                    [newStart, newEnd] = [newEnd, newStart];
                    setDirection('end');
                }
            } else {
                newEnd = newDayId;
                // Ensure end doesn't go before start
                if (newEnd < newStart) {
                    [newStart, newEnd] = [newEnd, newStart];
                    setDirection('start');
                }
            }

            setPreviewRange({ start: newStart, end: newEnd });
        };

        const handleMouseUp = (e) => {
            setDragging(false);
            const dayElement = findClosestDayElement(e.target);
            
            if (dayElement) {
                const newDayId = parseInt(dayElement.dataset.dayid);
                if (!isNaN(newDayId)) {
                    onResizeEnd?.(entity.id, direction, newDayId);
                }
            }

            cleanup();
        };

        const cleanup = () => {
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
            setPreviewRange(null);
            setDirection(null);
            setStartPos(null);
        };

        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);
    }, [entity, onResizeEnd]);

    // Helper to find the closest calendar day element
    const findClosestDayElement = (element) => {
        while (element && !element.dataset?.dayid) {
            element = element.parentElement;
        }
        return element;
    };

    return {
        dragging,
        direction,
        previewRange,
        handleMouseDown
    };
}
