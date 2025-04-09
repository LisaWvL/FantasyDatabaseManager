/* eslint-disable react-refresh/only-export-components */
//TODO - The Plotpoint Card contains data regarding all entities versioned by chapter, that are related to the PlotPoint.
//The cell of the CalendarGrid represents where in the timeline this Plotpoint happens. Dragging the card updates the date

//TODO - The DraggablePlotPointCard component allows plot points to be dragged and dropped
//The component uses the useDrag hook from the react-dnd library to enable drag functionality
//The plot point data is passed to the component as props
//The component renders a div element with the plot point title
//The div element is wrapped in the drag ref to enable drag functionality
//The component also handles right-click context menu events
//The component is styled with CSS classes from the CalendarPlotView.css file
//The component is exported as the default export

//TODO - Add the following code to the CalendarPlotView.css file:
// src/styles/CalendarPlotView.css
// .plotpoint-card {
//     background-color: #f8f9fa;
//     border: 1px solid #ced4da;
//     border-radius: 4px;
//     padding: 8px;
//     margin: 4px;
//     cursor: move;
// }

//TODO - The CSS code styles the plot point card with a light background color, border, padding, and margin
//The cursor property is set to move to indicate that the card is draggable
//The CSS code is used to style the DraggablePlotPointCard component


// src/components/DraggablePlotPointCard.jsx
/* eslint-disable react-refresh/only-export-components */
import React from 'react';
import { useDrag } from 'react-dnd';
import '../styles/CalendarPlotView.css';

const DraggablePlotPointCard = ({ plotPoint, onContextMenu }) => {
    const [{ isDragging }, drag] = useDrag(() => ({
        type: 'PLOT_POINT',
        item: { id: plotPoint.id },
        collect: (monitor) => ({
            isDragging: monitor.isDragging()
        })
    }), [plotPoint]);

    const handleContextMenu = (e) => {
        e.preventDefault();
        if (onContextMenu) {
            onContextMenu(e, plotPoint, 'plotpoint');
        }
    };

    return (
        <div
            ref={drag}
            className="plotpoint-card"
            onContextMenu={handleContextMenu}
            style={{ opacity: isDragging ? 0.5 : 1 }}
        >
            <div className="plotpoint-title">{plotPoint.title}</div>
            <div className="plotpoint-subinfo">
                {plotPoint.startDateName && <span>{plotPoint.startDateName}</span>}
                {plotPoint.endDateName && plotPoint.endDateName !== plotPoint.startDateName && (
                    <span> → {plotPoint.endDateName}</span>
                )}
            </div>
        </div>
    );
};

export default DraggablePlotPointCard;
