// src/features/plotpoints/plotPointFormUtils.js

import {
    fetchDateDayByMonthAndDay,
    fetchMonths
} from '../features/plotpoints/DateApi';
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

export async function resolveDateIds(startMonth, startDay, endMonth, endDay) {
    const start = await fetchDateDayByMonthAndDay(startMonth, parseInt(startDay));
    const end = await fetchDateDayByMonthAndDay(endMonth, parseInt(endDay));
    return { startId: start.id, endId: end.id };
}
