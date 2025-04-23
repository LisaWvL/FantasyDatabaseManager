import React from 'react';
import './TimelineStoryView.css';
//import { getYearFromDate } from './timelineUtils';

export default function PlotPointTimeline({ plotPoints, range }) {
    const { startYear, endYear } = range;

    const getYearFromDate = (date) => {
        if (!date || typeof date.year !== 'number') return null;
        return date.year;
    };

    return (
        <div className="timeline-row plotpoints-row">
            <h4>📍 PlotPoints</h4>
            <div className="timeline-scroll">
                {plotPoints.map((pp) => {
                    const start = getYearFromDate(pp.startDate);
                    const end = getYearFromDate(pp.endDate ?? pp.startDate);

                    if (start === null || end === null) return null;
                    if (end < startYear || start > endYear) return null;

                    const left = ((start - startYear) / (endYear - startYear)) * 100;
                    const duration = Math.max(end - start, 1); // ensure at least 1 year
                    const width = (duration / (endYear - startYear)) * 100;

                    return (
                        <div
                            key={pp.id}
                            className="timeline-block plotpoint-block"
                            style={{ left: `${left}%`, width: `${width}%` }}
                            title={`${pp.title} (${start} – ${end})`}
                        >
                            {pp.title}
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
