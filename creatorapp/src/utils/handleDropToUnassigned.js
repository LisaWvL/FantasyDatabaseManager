// src/utils/handleDropToUnassigned.js
//Responsibility:
//Handle updating entities when dropped on the date or sidebar.
//Ensure null fields are set correctly for unassigned entities.
//Files to Handle:
//handleDropToUnassigned.ts:
//This utility function should handle updating PlotPoints when dropped into the sidebar or unassigned zone. 
//The rules for nulling out fields based on the entity type should be used here.
//What's Missing/Needed:
//Generalize this utility so it can be reused for different entity types (e.g., Character, PlotPoint).





import { EntityUpdater } from './EntityManager';

const unassignNullRules = {
    Character: {
        date: ['startDateId', 'endDateId'],
        writingAssistant: ['chapterId'],
    },
    PlotPoint: {
        date: ['startDateId', 'endDateId', 'chapterId'],
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
