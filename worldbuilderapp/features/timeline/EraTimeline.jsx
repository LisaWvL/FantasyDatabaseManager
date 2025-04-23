import React from 'react';
import './TimelineStoryView.css';
//import { getYearFromCalendar } from './timelineUtils';

export default function EraTimeline({ eras, range }) {
    const { startYear, endYear } = range;

    const getYearFromDate = (date) => {
        if (!date || typeof date.year !== 'number') return null;
        return date.year;
    };

    return (
        <div className="timeline-row eras-row">
            <h4>📜 Eras</h4>
            <div className="timeline-scroll">
                {eras.map((era) => {
                    const start = getYearFromDate(era.startDate);
                    const end = getYearFromDate(era.endDate ?? era.startDate);

                    if (start === null || end === null) return null;
                    if (end < startYear || start > endYear) return null;

                    const left = ((start - startYear) / (endYear - startYear)) * 100;
                    const duration = Math.max(end - start, 1);
                    const width = (duration / (endYear - startYear)) * 100;

                    return (
                        <div
                            key={era.id}
                            className="timeline-block era-block"
                            style={{ left: `${left}%`, width: `${width}%` }}
                            title={`${era.name} (${start} – ${end})`}
                        >
                            {era.name}
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
