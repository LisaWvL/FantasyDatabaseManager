import { EntitySchemas } from '../store/EntitySchemas'

export function getCompactFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.showInCompact);
}

export function getEditableFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.editable);
}

export function getRelationFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.section === 'relation');
}

export function getHeaderFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.section === 'header');
}

export function getSummaryFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.section === 'summary');
}

export function getDetailsFields<T>(schema: EntitySchemas<T>) {
  return schema.fields.filter(f => f.section === 'details');
}

export function getAllFields<T>(schema: EntitySchemas<T>) {
  return schema.fields;
}
