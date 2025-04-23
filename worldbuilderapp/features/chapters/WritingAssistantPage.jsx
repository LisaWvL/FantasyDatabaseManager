import { useEffect, useRef, useState } from 'react';
import ChapterEditor from './ChapterEditor';
import { EntityFetcher, EntityUpdater, EntityDeleter } from '../../utils/EntityManager';
import useEntityRegistry from '../../utils/EntityRegistry';

import ContextMenu from '../../utils/ContextMenu';
import { fetchFlatEntity } from '../../src/api/api'
import ConfirmDialog from '../../utils/ConfirmDialog';
import SmallEntityCard from '../../utils//SmallEntityCard';
import MainLayout from '../../src/layout/MainLayout';

export default function WritingAssistantPage() {
    const [fullChapters, setFullChapters] = useState([]);
    const [unassignedEntities, setUnassignedEntities] = useState([]);
    const [contextMenu, setContextMenu] = useState(null);
    const [confirmDialog, setConfirmDialog] = useState(null);
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [isOverSidebar, setIsOverSidebar] = useState(false);
    const scrollRef = useRef(null);


    const registry = useEntityRegistry();

    useEffect(() => {
        const load = async () => {
            await Promise.all([
                registry.load('Chapter'),
                registry.load('PlotPoint'),
                registry.load('Character'),
                registry.load('Scene'),
                registry.load('Faction'),
                registry.load('Item'),
                registry.load('Event'),
                registry.load('Era'),
                registry.load('CharacterRelationship'),
            ]);

            const chapters = Object.values(registry.cache.Chapter || {});
            const plotPoints = Object.values(registry.cache.PlotPoint || {});
            const allEntities = [
                ...Object.values(registry.cache.Character || {}),
                ...Object.values(registry.cache.Scene || {}),
                ...Object.values(registry.cache.Faction || {}),
                ...Object.values(registry.cache.Item || {}),
                ...Object.values(registry.cache.Event || {}),
                ...Object.values(registry.cache.Era || {}),
                ...Object.values(registry.cache.CharacterRelationship || {}),
            ];

            const populated = chapters.map((chapter) => {
                const chapterEntities = allEntities.filter(e => e.chapterId === chapter.id);
                const relatedPlotPoint = plotPoints.find(p => p.id === chapter.plotPointId);
                return {
                    id: chapter.id,
                    chapter,
                    plotPoint: relatedPlotPoint || null,
                    entities: chapterEntities,
                };
            });

            setFullChapters(populated);

            const unassigned = allEntities.filter(e => !e.chapterId);
            setUnassignedEntities(unassigned);
        };

        load();
    }, [registry]);



    const handleDrop = async ({ entityId, entityType, targetChapterId = null, isPOV = false }) => {
        try {
            if (isPOV && entityType === 'Character') {
                const updatedChapter = await EntityUpdater.update('Chapter', targetChapterId, {
                    povCharacterId: entityId,
                });

                setFullChapters((prev) =>
                    prev.map((fc) =>
                        fc.id === targetChapterId ? { ...fc, chapter: updatedChapter } : fc
                    )
                );
                return;
            }

            const updatedEntity = await EntityUpdater.update(entityType, entityId, {
                chapterId: targetChapterId,
            });

            if (!targetChapterId) {
                setUnassignedEntities((prev) => {
                    const seen = new Set(prev.map(e => `${e.entityType}-${e.id}`));
                    const key = `${entityType}-${entityId}`;
                    return seen.has(key) ? prev : [...prev, { ...updatedEntity, entityType }];
                });
            } else {
                setUnassignedEntities((prev) =>
                    prev.filter((e) => !(e.id === entityId && e.entityType === entityType))
                );

                const flatEntities = await fetchFlatEntity(entityType); // load only the dropped type
                const chapterEntities = flatEntities
                    .filter((e) => e.chapterId === targetChapterId)
                    .map(e => ({ ...e, entityType }));

                setFullChapters((prev) =>
                    prev.map((fc) =>
                        fc.id === targetChapterId ? { ...fc, entities: chapterEntities } : fc
                    )
                );
            }
        } catch (err) {
            console.error(`❌ Failed to drop ${entityType} #${entityId}`, err);
        }
    };






    const handleSave = (updatedChapter) => {
        setFullChapters((prev) =>
            prev.map((fc) => (fc.id === updatedChapter.id ? { ...fc, chapter: updatedChapter } : fc))
        );
    };

    const handleDelete = async (entity) => {
        await EntityDeleter.delete(entity.entityType, entity.id);
        setUnassignedEntities(prev => prev.filter(e => !(e.id === entity.id && e.entityType === entity.entityType)));
        setConfirmDialog(null);
        setContextMenu(null);
    };

    const headerContent = (
        <div className="chapter-header-bar">
            <button onClick={() => scrollRef.current?.scrollIntoView({ behavior: 'smooth' })}>
                ⬆ Go to Top
            </button>
            <h2>Word Count: {fullChapters.reduce((sum, c) => sum + (c.chapter.wordCount || 0), 0)}</h2>
            <button className="new-chapter-button" onClick={() => { }}>
                ➕ New Chapter
            </button>
        </div>
    );

    const usedKeys = new Set();
    unassignedEntities.forEach((e) => {
        const key = `${e.entityType?.toLowerCase?.()}-${e.id}`;
        if (usedKeys.has(key)) {
            console.warn("⚠️ Duplicate key detected:", key, e);
        }
        usedKeys.add(key);
    });


    const unassignedSidebar = {
        entityType: 'Entity',
        items: unassignedEntities,
        isUnassigned: (e) => !e.chapterId,
        renderItem: (e) => {
            if (!e.id || !e.entityType) {
                console.warn('⚠️ Entity missing id or type:', e);
                return null;
            }

            return (
                <SmallEntityCard
                    key={`${e.entityType?.toLowerCase?.()}-${e.id}`}
                    entity={e}
                    entityType={e.entityType}
                    onContextMenu={(ev) =>
                        setContextMenu({ x: ev.clientX, y: ev.clientY, item: e, entityType: e.entityType })
                    }
                    onDragStart={(event) => {
                        event.dataTransfer.setData('entityId', e.id);
                        event.dataTransfer.setData('entityType', e.entityType);
                    }}
                    onDragEnd={() => setIsOverSidebar(false)}
                    draggable
                />
            );
        },

        onDropToUnassigned: handleDrop,
        onContextMenu: (e, item, type) => setContextMenu({ x: e.clientX, y: e.clientY, item, entityType: type }),
        isSidebarOpen,
        setIsSidebarOpen,
        isOverSidebar,
        onSidebarDragOver: (e) => {
            e.preventDefault();
            setIsOverSidebar(true);
        },
        onSidebarDragLeave: () => setIsOverSidebar(false),
    };

    return (
        <>
            <MainLayout
                headerContent={headerContent}
                content={
                    <div className="chapter-editor-list">
                        {fullChapters.map((fc, index) => (
                            <div key={fc.id} ref={index === fullChapters.length - 1 ? scrollRef : null}>
                                <ChapterEditor
                                    full={fc}
                                    onSave={handleSave}
                                    onDropToChapter={(entityId, entityType) =>
                                        handleDrop({ entityId, entityType, targetChapterId: fc.id })
                                    } />
                            </div>
                        ))}
                    </div>
                }
                unassignedSidebar={unassignedSidebar}
            />

            {contextMenu && (
                <ContextMenu
                    x={contextMenu.x}
                    y={contextMenu.y}
                    onClose={() => setContextMenu(null)}
                    onEdit={() => alert('Edit not implemented')}
                    onCreate={() => alert('Create not implemented')}
                    onDelete={() => setConfirmDialog(contextMenu)}
                />
            )}
            {confirmDialog && (
                <ConfirmDialog
                    title="Confirm Delete"
                    message={`Are you sure you want to delete this ${confirmDialog.entityType}?`}
                    onCancel={() => setConfirmDialog(null)}
                    onConfirm={() => handleDelete(confirmDialog.item)}
                />
            )}
        </>
    );
}
