import { useEffect, useState, useCallback } from 'react';

export function useEntityManager(fetchEntity, updateEntity, entityId) {
    const [entity, setEntity] = useState(null);
    const [dirty, setDirty] = useState(false);

    useEffect(() => {
        if (!entityId) return;
        fetchEntity(entityId).then(setEntity);
    }, [entityId, fetchEntity]);

    const markDirty = () => setDirty(true);

    const saveEntity = useCallback(async (updates = {}) => {
        if (!entity) return;

        const updatedEntity = { ...entity, ...updates };
        await updateEntity(entityId, updatedEntity);
        setEntity(updatedEntity);
        setDirty(false);
        return updatedEntity;
    }, [entity, entityId, updateEntity]);

    return {
        entity,
        setEntity,
        dirty,
        markDirty,
        saveEntity,
    };
}
