// src/features/plotpoints/usePlotPointDragResize.js
import { useRef, useState } from 'react';

export function usePlotPointDragResize({ plotPoint, onResizeEnd }) {
    const [dragging, setDragging] = useState(null);
    const draggingRef = useRef(null);
    const [previewRange, setPreviewRange] = useState(null);

    const clearHighlights = () => {
        document.querySelectorAll('.calendar-cell.preview-span').forEach(cell =>
            cell.classList.remove('preview-span')
        );
    };

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

            clearHighlights();

            if (!previewDayId) {
                setPreviewRange(null);
                return;
            }

            const currentDragging = draggingRef.current;

            const fixedStart = plotPoint.startDateId;
            const fixedEnd = plotPoint.endDateId ?? plotPoint.startDateId;

            let rangeStart = fixedStart;
            let rangeEnd = fixedEnd;

            if (currentDragging.direction === 'start') {
                rangeStart = previewDayId;
            }
            if (currentDragging.direction === 'end') {
                rangeEnd = previewDayId;
            }

            const [min, max] = [Math.min(rangeStart, rangeEnd), Math.max(rangeStart, rangeEnd)];
            setPreviewRange({ start: min, end: max });

            for (let day = min; day <= max; day++) {
                const cell = document.querySelector(`.calendar-cell[data-dayid="${day}"]`);
                if (cell) cell.classList.add('preview-span');
            }
        };


        const onMouseUp = async (upEvent) => {
            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);

            clearHighlights();
            setPreviewRange(null);

            const elements = document.elementsFromPoint(upEvent.clientX, upEvent.clientY);
            let newDayId = null;

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

    return { dragging, previewRange, handleMouseDown };
}
