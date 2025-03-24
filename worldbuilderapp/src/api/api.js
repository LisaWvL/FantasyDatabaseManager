const API_BASE = 'https://localhost:63752/api';

export async function fetchEntities(entityType, snapshotId = null) {
    const url = snapshotId ? `${API_BASE}/${entityType}?snapshotId=${snapshotId}` : `${API_BASE}/${entityType}`;
    const response = await fetch(url);
    if (!response.ok) throw new Error(`Failed to load ${entityType}`);
    return await response.json();
}

export async function fetchEntityById(entityType, id) {
    const response = await fetch(`${API_BASE}/${entityType}/${id}`);
    if (!response.ok) throw new Error(`Failed to load ${entityType} with id ${id}`);
    return await response.json();
}

export async function createEntity(entityType, payload) {
    const response = await fetch(`${API_BASE}/${entityType}/create`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
    });
    if (!response.ok) throw new Error(`Failed to create ${entityType}`);
    return await response.json();
}

export async function updateEntity(entityType, id, payload) {
    const response = await fetch(`${API_BASE}/${entityType}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
    });
    if (!response.ok) throw new Error(`Failed to update ${entityType} with id ${id}`);
    return await response.json();
}

export async function deleteEntity(entityType, id) {
    const response = await fetch(`${API_BASE}/${entityType}/${id}`, {
        method: 'DELETE',
    });
    if (!response.ok) throw new Error(`Failed to delete ${entityType} with id ${id}`);
    return true;
}
