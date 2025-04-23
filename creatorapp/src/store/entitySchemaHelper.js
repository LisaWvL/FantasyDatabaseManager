import { entitySchemas } from '../store/EntitySchemas';

export function getSchema(entityType) {
    return entitySchemas[entityType];
}

export function getFields(entityType) {
    return getSchema(entityType)?.fields || [];
}

export function getEditableFields(entityType) {
    return getFields(entityType).filter(f => f.editable);
}

export function getFKFields(entityType) {
    return getFields(entityType).filter(f => f.type === 'fk' || f.type === 'multiFk');
}

export function getFieldsBySection(entityType, section) {
    return getFields(entityType).filter(f => f.section === section);
}

export function getField(entityType, key) {
    return getFields(entityType).find(f => f.key === key);
}

export function getPrimaryDisplay(entityType, entity) {
    const schema = getSchema(entityType);
    return schema?.primaryDisplay?.(entity) || `#${entity.id}`;
}
