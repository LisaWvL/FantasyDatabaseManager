// src/features/plotpoints/plotPointFormUtils.js

import {
    fetchCalendarDayByMonthAndDay,
    fetchMonths
} from './CalendarApi';
import {
    fetchRivers,
    fetchRoutes,
    fetchChapters
} from '../../src/api/DropdownApi';

export async function loadPlotPointDropdowns() {
    const [months, chapters, rivers, routes] = await Promise.all([
        fetchMonths(),
        fetchChapters(),
        fetchRivers(),
        fetchRoutes(),
    ]);
    return { months, chapters, rivers, routes };
}

export function toggleLinkedEntity(field, id, linkedEntities) {
    const exists = linkedEntities[field].includes(id);
    const updated = exists
        ? linkedEntities[field].filter((x) => x !== id)
        : [...linkedEntities[field], id];
    return { ...linkedEntities, [field]: updated };
}

export async function resolveCalendarIds(startMonth, startDay, endMonth, endDay) {
    const start = await fetchCalendarDayByMonthAndDay(startMonth, parseInt(startDay));
    const end = await fetchCalendarDayByMonthAndDay(endMonth, parseInt(endDay));
    return { startId: start.id, endId: end.id };
}
