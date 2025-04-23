import React from 'react';
import './TimelineStoryView.css';

export default function ChapterTimeline({ chapters, range }) {
    const { startYear, endYear } = range;

    const getYearFromDate = (date) => {
        if (!date || typeof date.year !== 'number') return null;
        return date.year;
    };

    // Filter and sort chapters within the selected range
    const visibleChapters = chapters
        .map((ch) => {
            const start = getYearFromDate(ch.startDate);
            const end = getYearFromDate(ch.endDate ?? ch.startDate);
            if (start === null || end === null) return null;
            if (end < startYear || start > endYear) return null;
            return { ...ch, start, end };
        })
        .filter(Boolean)
        .sort((a, b) => a.start - b.start);

    return (
        <div className="timeline-row chapters-row">
            <h4>📘 Chapters</h4>
            <div className="timeline-scroll">
                {visibleChapters.map((ch) => {
                    const { id, chapterTitle, start, end } = ch;
                    const left = ((start - startYear) / (endYear - startYear)) * 100;
                    const duration = Math.max(end - start, 1);
                    const width = (duration / (endYear - startYear)) * 100;

                    return (
                        <div
                            key={id}
                            className="timeline-block chapter-block"
                            style={{ left: `${left}%`, width: `${width}%` }}
                            title={`Chapter: ${chapterTitle} (${start} – ${end})`}
                            tabIndex={0} // ✅ Accessible
                            role="button"
                            aria-label={`Chapter ${chapterTitle}, from year ${start} to ${end}`}
                            onClick={() => console.log(`🟦 Navigate to chapter ${id}`)}
                        >
                            <div className="chapter-title">{chapterTitle}</div>
                            <div className="chapter-range">{start} – {end}</div>
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
