//TODO: Implement the CalendarDayCell component.
//This component will be used to display the plot points for a specific day in the calendar view.
//The component should accept the following props:
//1. day: an object representing the day to display, with properties for day, weekday, and month.
//2. plotPoints: an array of plot point objects to display for the day.
//3. onDropPlotPoint: a function to handle dropping a plot point onto the day cell.
//4. onContextMenu: a function to handle right-click context menu events on the day cell.
//The component should render the day information in the header of the cell, and then map over the plot points to render a DraggablePlotPointCard for each one.
//The DraggablePlotPointCard component should be draggable and display the plot point information.
//The CalendarDayCell component should use the useDrop hook from react-dnd to enable dropping plot points onto the cell.
//The onDropPlotPoint function should be called with the plot point id and the day id when a plot point is dropped onto the cell.
//The onContextMenu function should be called with the event, day, and a context string when a right-click event occurs on the cell.
//The context string should indicate that the event occurred in the calendar view.
//The component should have appropriate styles to display the day cell and plot point cards.
//The component should be exported as the default export.
//The component should be placed in the components directory.
//The component should be named CalendarDayCell.
//The component should be implemented in a file named CalendarDayCell.jsx.
//The component should import React from the 'react' module.
//The component should import the useDrop hook from the 'react-dnd' module.
//The component should import the DraggablePlotPointCard component from './DraggablePlotPointCard'.
//The component should import the '../styles/CalendarPlotView.css' stylesheet.
//The component should be a functional component.
//The component should accept the props day, plotPoints, onDropPlotPoint, and onContextMenu.
//The component should render a div element with the class 'calendar-cell'.
//The div element should be a drop target for plot points using the useDrop hook.
//The div element should call the onDropPlotPoint function with the plot point id and day id when a plot point is dropped onto the cell.
//The div element should call the onContextMenu function with the event, day, and 'calendar' context string when a right-click event occurs on the cell.
//The component should render a div element with the class 'calendar-header' containing the day information.
//The day information should include the day number and weekday name.
//The day information should include the month name.
//The component should map over the plotPoints array and render a DraggablePlotPointCard component for each plot point.
//The DraggablePlotPointCard component should be passed the plot point object as a prop.
//The DraggablePlotPointCard component should be passed the onContextMenu function as a prop.
//The DraggablePlotPointCard component should have a key attribute set to the plot point id.
//The component should export the CalendarDayCell component as the default export.
//The component should be implemented in the CalendarDayCell.jsx file.
//The component should be styled using the CalendarPlotView.css stylesheet.
//The component should display the day information in the header of the cell.
//The component should display the plot points as draggable cards within the cell.
//The component should handle dropping plot points onto the cell and calling the onDropPlotPoint function.
//The component should handle right-click events on the cell and call the onContextMenu function.
//The component should be free of linter errors.
//The component should be free of console warnings.
//The component should be free of console logs.






// src/components/CalendarDayCell.jsx
import React from "react";
import { useDrop } from "react-dnd";
import DraggablePlotPointCard from "./DraggablePlotPointCard";
import "../styles/CalendarPlotView.css";

export default function CalendarDayCell({ day, plotPoints, onDropPlotPoint, onContextMenu }) {
    const [, drop] = useDrop({
        accept: "plotpoint",
        drop: (item) => onDropPlotPoint(item.id, day.id),
    });

    return (
        <div
            className="calendar-cell"
            ref={drop}
            onContextMenu={(e) => onContextMenu(e, day, "calendar")}
        >
            <div className="calendar-header">
                <span>{day.day} {day.weekday}</span>
                <span>{day.month}</span>
            </div>

            {plotPoints.map(plotPoint => (
                <DraggablePlotPointCard
                    key={plotPoint.id}
                    plotPoint={plotPoint}
                    onContextMenu={onContextMenu}
                />
            ))}
        </div>
    );
}

