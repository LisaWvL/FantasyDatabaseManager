// 📁 features/plotpoints/CalendarDayCell.jsx
import React, { useState } from 'react';
import { useDragAndDrop, createDragOverHandler, createDragLeaveHandler } from '../../hooks/useDragAndDrop';
import './CalendarDayCell.css';

export default function CalendarDayCell({ day, children, onDropEntity }) {
    const { handleDrop } = useDragAndDrop({
        onDropSuccess: async (entityId, entityType, dropTargetDayId) => {
            console.log('📥 Entity dropped into day', dropTargetDayId);
            await onDropEntity?.(entityId, entityType, dropTargetDayId);
        }
    });

    const handleDropWrapper = (e) => {
        e.preventDefault();
        handleDrop(e, 'calendar', day.id);
    };

    return (
        <div
            className="calendar-cell"
            data-dayid={day.id}
            onDragOver={(e) => e.preventDefault()}
            onDrop={handleDropWrapper}
        >
            <div className="calendar-header">
                <div className="calendar-day-number">{day.day}</div>
                <div className="calendar-weekday">{day.weekday}</div>
                <div className="calendar-meta">{day.name}</div>
            </div>
            <div className="calendar-body">
                {children}
            </div>
        </div>
    );
}



//// 📁 features/plotpoints/CalendarDayCell.jsx
//import React, { useState } from 'react';
//import { useDragAndDrop, createDragOverHandler, createDragLeaveHandler } from '../../hooks/useDragAndDrop';
//import './CalendarDayCell.css';

//export default function CalendarDayCell({ day, children, onRefreshAfterDrop, onDrop }) {
//    const [isOver, setIsOver] = useState(false);

//    const { handleDrop } = useDragAndDrop({
//        onDropSuccess: (entityId, dropTargetId) => {
//            console.log('📥 Entity dropped into day:', dropTargetId);
//            onRefreshAfterDrop?.({ id: entityId }, dropTargetId);
//        }
//    });

//    return (
//        <div
//            className={`calendar-cell ${isOver ? 'drag-over' : ''}`}
//            data-dayid={day.id}
//            data-date-id={day.id}
//            onDragOver={createDragOverHandler(setIsOver)}
//            onDragLeave={createDragLeaveHandler(setIsOver)}
//            onDrop={(e) => {
//                handleDrop(e, 'dashboard', day.id);
//                setIsOver(false);
//                onDrop?.(e);
//            }}
//        >
//            <div className="calendar-header">
//                <div className="calendar-day-number">{day.day}</div>
//                <div className="calendar-weekday">{day.weekday}</div>
//            </div>
//            <div className="calendar-body">
//                {children}
//            </div>
//        </div>
//    );
//}
