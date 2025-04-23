// src/utils/EntityActionHelper.js
import { entitySchemas } from './entitySchemas'
import useEntityRegistry from '../stores/EntityRegistry'

export function handleEntityDrop({ entity, from, to, context }) {
    const registry = useEntityRegistry.getState()

    const chapterId = context?.chapterId
    const type = entity.type

    if (!entitySchemas[type]) return

    // From calendar to unassigned
    if (from === 'calendar' && to === 'unassigned') {
        const nullable = ['startDateId', 'endDateId']
        nullable.forEach(field => {
            if (entity[field] != null) {
                registry.setFieldToNull(type, entity.id, field)
            }
        })
    }

    // From unassigned to writing zone
    if (from === 'unassigned' && to === 'chapter-drop') {
        const chapterField = entitySchemas[type].fields.find(f => f.fkType === 'Chapter')?.key
        if (chapterField && chapterId) {
            registry.update(type, entity.id, { [chapterField]: chapterId })
        }
    }
}
