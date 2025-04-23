// 📁 CalendarDayCell.jsx
import React from 'react';
import UnassignedSidebar from '../sidebars/UnassignedSidebar';
import Card from '../../components/Card';
import '../../components/Card.css';

import { createDragOverHandler, createDragLeaveHandler, useDragAndDrop } from '../../utils/DragDropHandlers';
import './CalendarDayCell.css';

export default function CalendarDayCell({ date, cards, onContextMenu, onUpdateCard, onCardDrop }) {
    const { handleDrop } = useDragAndDrop({
        handleUpdateEntity: async ({ entity, dragSourceContext }) => {
            await onCardDrop({
                entityId: entity.id,
                entityType: entity.entityType,
                dropTargetType: 'dashboard',
                dropTargetId: date.id,
                dragSourceContext: dragSourceContext
            });
        }
    });

    return (
        <div
            className="calendar-cell"
            data-date-id={date.id}
            onDragOver={createDragOverHandler()}
            onDragLeave={createDragLeaveHandler()}
            onDrop={(e) => handleDrop(e, 'dashboard')}
        >
            <div className="calendar-header">{date.month}
                <div className="calendar-day-number">{date.day}</div>
                <div className="calendar-weekday">{date.weekday}</div>
            </div>


            <div className="calendar-body">
                {cards.map(card => (
                    <Card
                        //check if Id or id
                        key={card.CardData.Id + (card.isGhost ? '-ghost' : '')}
                        card={card}
                        onContextMenu={onContextMenu}
                        onUpdate={onUpdateCard}
                        mode="compact"
                        isGhost={card.isGhost}
                    />
                ))}
            </div>
        </div>
    );
}



//import './CalendarDayCell.css';

//export default function DateDayCell({
//    day = {},
//    weekday = {},
//    month = {},
//    year = {},
//    children,
//    onDropPlotPoint
//}) {
//    const handleDragOver = (e) => {
//        e.preventDefault();
//        e.dataTransfer.dropEffect = 'move';
//    };

//    const handleDrop = (e) => {
//        e.preventDefault();
//        const plotPointId = parseInt(e.dataTransfer.getData('plotPointId'));
//        console.log('📦 dropped on', day?.id, 'plotPointId:', plotPointId);

//        if (!isNaN(plotPointId) && day?.id) {
//            onDropPlotPoint(plotPointId, day.id);
//        }
//    };

//    return (
//        <div
//            className="date-cell"
//            data-dayid={day?.id}
//            data-month={month?.id}
//            data-year={year?.id}
//            data-weekday={weekday?.id}
//            onDragOver={handleDragOver}
//            onDrop={handleDrop}
//        >
//            <div className="date-header">
//                <div className="date-day-number">{day?.day ?? '?'}</div>
//                <div className="date-weekday">{weekday?.weekday ?? ''}</div>
//                <div className="date-meta">{day?.name ?? ''}</div>
//            </div>
//            <div className="date-body">
//                {children}
//            </div>
//        </div>
//    );
//}
