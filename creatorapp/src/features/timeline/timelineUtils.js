// timelineUtils.js
export function getYearFromDate(date) {
    if (!date || typeof date !== 'object') return null;

    const year = date.year;

    // Optional: handle legacy string/int cases
    if (typeof year === 'string') return parseInt(year, 10);
    if (typeof year === 'number') return year;

    return null;
}
