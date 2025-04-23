// 📁 src/hooks/useDragAndDrop.ts
export function useDragAndDrop({ handleUpdateEntity }) {
    const handleDragStart = (e, entity) => {
        if (!entity || !entity.entityType || !entity.id) {
            console.warn('❌ Invalid drag entity:', entity);
            return;
        }

        e.dataTransfer.setData('entityType', entity.entityType);
        e.dataTransfer.setData('entityId', entity.id.toString());
    };

    const handleDrop = (e, dropTarget) => {
        e.preventDefault();

        const entityType = e.dataTransfer.getData('entityType');
        const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);

        if (!entityType || !entityId) {
            console.warn('❌ Missing drop data');
            return;
        }

        const entity = { id: entityId, entityType };
        const payload = {};
        let updateTargetType = entityType;
        let updateTargetId = entityId;

        if (dropTarget === 'POV-dropzone' && entityType === 'Character') {
            const chapterId = e.currentTarget.closest('[data-chapter-id]')?.dataset?.chapterId;
            if (!chapterId) {
                console.warn('❌ No chapterId found for POV drop');
                return;
            }
            updateTargetType = 'Chapter';
            updateTargetId = parseInt(chapterId);
            payload.povCharacterId = entity.id;
        } else if (dropTarget === 'context-dropzone' || dropTarget === 'unassigned-sidebar') {
            const chapterId = dropTarget === 'unassigned-sidebar'
                ? null
                : parseInt(e.currentTarget.closest('[data-chapter-id]')?.dataset?.chapterId || '');
            payload.chapterId = chapterId;
        }

        console.log('[📦 handleDrop]', { entity, payload, dropTarget });
        handleUpdateEntity({ entity, payload, updateTargetType, updateTargetId, dropTarget });
    };

    const handleDragOver = (e) => e.preventDefault();
    const handleDragLeave = () => { };

    return {
        handleDragStart,
        handleDrop,
        handleDragOver,
        handleDragLeave,
    };
}
