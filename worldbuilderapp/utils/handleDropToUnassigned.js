// src/utils/handleDropToUnassigned.js
import { EntityUpdater } from './EntityManager';

const unassignNullRules = {
    Character: {
        calendar: ['startDateId', 'endDateId'],
        writingAssistant: ['chapterId'],
    },
    PlotPoint: {
        calendar: ['startDateId', 'endDateId', 'chapterId'],
    },
    Item: {
        writingAssistant: ['chapterId'],
    },
    Faction: {
        writingAssistant: ['chapterId'],
    },
    Scene: {
        writingAssistant: ['chapterId'],
    },
};

export async function handleDropToUnassigned(entityId, entityType, dragSourceContext) {
    const rules = unassignNullRules[entityType]?.[dragSourceContext];
    if (!rules) return;

    for (const field of rules) {
        try {
            await EntityUpdater.setNull(entityType, entityId, field);
        } catch (err) {
            console.error(`❌ Failed to null '${field}' on ${entityType} ${entityId}`, err);
        }
    }
}
