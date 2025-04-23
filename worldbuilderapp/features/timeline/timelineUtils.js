// timelineUtils.js
export function getYearFromCalendar(calendar) {
    if (!calendar || typeof calendar !== 'object') return null;

    const year = calendar.year;

    // Optional: handle legacy string/int cases
    if (typeof year === 'string') return parseInt(year, 10);
    if (typeof year === 'number') return year;

    return null;
}
