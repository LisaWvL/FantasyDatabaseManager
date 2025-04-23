// 📁 src/entities/ModularEntityPage.jsx
import { useEffect, useState } from 'react';
import MainLayout from '../../src/layout/MainLayout';
import Card from '../../src/components/Card';
import { entitySchemas } from '../../utils/entitySchemas';
import useEntityRegistry from '../../utils/EntityRegistry';
import { safeLower } from '../../utils/UpperLowerCase';
import useDragScroll from '../../hooks/useDragScroll';
import { useDragAndDrop } from '../../utils/dragDropHandlers';
import './ModularEntityPage.css';

export default function ModularEntityPage({ cardEntity, sectionEntity, updateFK }) {
    const cardSchema = entitySchemas[cardEntity];
    const sectionSchema = entitySchemas[sectionEntity];

    const [cards, setCards] = useState([]);
    const [sections, setSections] = useState([]);
    const [dragOverSectionId, setDragOverSectionId] = useState(null);
    const [isDragging, setIsDragging] = useState(false);
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [isOverSidebar, setIsOverSidebar] = useState(false);

    const registry = useEntityRegistry();
    useDragScroll(isDragging);

    useEffect(() => {
        const loadData = async () => {
            await registry.load(sectionEntity);
            await registry.load(cardEntity);
            setSections(Object.values(registry.cache[sectionEntity] || {}));
            setCards(Object.values(registry.cache[cardEntity] || {}));
        };
        loadData();
    }, [cardEntity, sectionEntity, registry]);

    const handleDragStart = () => setIsDragging(true);
    const handleDragEnd = () => setIsDragging(false);

    const handleUpdateCard = async (updatedCard) => {
        await registry.update(cardEntity, updatedCard.id, updatedCard);
        setCards(Object.values(registry.cache[cardEntity] || {}));
    };

    const handleDeleteEntity = async (card) => {
        await registry.delete?.(cardEntity, card.id); // Optional delete future
        setCards(prev => prev.filter((c) => c.id !== card.id));
    };

    const handleCreateNewEntity = async () => {
        const payload = { name: `New ${cardEntity}`, [updateFK]: null };
        const created = await registry.create?.(cardEntity, payload); // Optional create future
        if (created) setCards(prev => [created, ...prev]);
    };

    const { handleDrop } = useDragAndDrop({
        handleUpdateEntity: async ({ entity, dropTarget }) => {
            if (dropTarget === 'unassigned-sidebar') {
                registry.setFieldToNull(entity.entityType, entity.id, updateFK);
            } else if (dropTarget === 'context-dropzone') {
                const chapterId = parseInt(entity.context?.chapterId);
                if (chapterId) {
                    registry.update(entity.entityType, entity.id, { [updateFK]: chapterId });
                }
            }
        },
    });

    const PageContent = (
        <div className="entity-page">
            <div className="entity-page-header">
                <h2>{cardSchema.label} Assignment</h2>
                <button onClick={handleCreateNewEntity}>➕ Add {cardSchema.label}</button>
            </div>

            {sections.map((section) => {
                const sectionCards = cards.filter((c) => c[updateFK] === section.id);
                return (
                    <div
                        key={section.id}
                        className={`chapter-section ${dragOverSectionId === section.id ? 'drag-over' : ''}`}
                        data-chapter-id={section.id}
                        onDragOver={(e) => e.preventDefault()}
                        onDragEnter={() => setDragOverSectionId(section.id)}
                        onDragLeave={(e) => {
                            const target = e.relatedTarget;
                            if (!(target instanceof Node) || !e.currentTarget.contains(target)) {
                                setDragOverSectionId(null);
                            }
                        }}
                        onDrop={(e) => {
                            e.preventDefault();
                            handleDrop(e, 'context-dropzone', { chapterId: section.id });
                            setDragOverSectionId(null);
                        }}
                    >
                        <div className="chapter-header">
                            <h3>{section.title || section.name || `${sectionSchema.label} #${section.id}`}</h3>
                        </div>
                        <div className="entity-card-row">
                            {sectionCards.map((card) => (
                                <Card
                                    key={card.id}
                                    entity={card}
                                    entityType={cardEntity}
                                    onFieldUpdate={handleUpdateCard} // ✅ Card prop fix
                                    onDelete={() => handleDeleteEntity(card)}
                                    onCreateNewVersion={handleCreateNewEntity} // ✅ Fixed from 'onCreateNewEntity'
                                    draggable
                                    onResizeEnd={(newWidth) => {
                                        handleUpdateCard({ ...card, width: newWidth });
                                    }} // ✅ Added resize end handler
                                    onDragStart={(e) => {
                                        e.dataTransfer.setData(`${safeLower(cardEntity)}Id`, card.id);
                                        handleDragStart();
                                    }}
                                    onDragEnd={handleDragEnd}
                                    onContextMenu={(e) => {
                                        e.preventDefault();
                                        console.log('Right-clicked', card);
                                    }} // ✅ Added context menu

                                />
                            ))}
                        </div>
                    </div>
                );
            })}
        </div>
    );

    return (
        <MainLayout
            headerContent={<h2>{cardSchema.label} Assignment</h2>}
            unassignedSidebar={{
                entityType: cardEntity,
                items: cards,
                isUnassigned: (c) => !c[updateFK],
                onDropToUnassigned: (cardId) => {
                    const card = cards.find((c) => c.id === cardId);
                    if (!card) return;
                    registry.setFieldToNull(cardEntity, card.id, updateFK);
                },
                onContextMenu: (e, item) => {
                    e.preventDefault();
                    console.log('Right-clicked', item);
                },
                renderItem: (item) => (
                    <Card
                        key={item.id}
                        entity={item}
                        entityType={cardEntity}
                        onFieldUpdate={handleUpdateCard}
                        onDelete={() => handleDeleteEntity(item)}
                        onCreateNewVersion={handleCreateNewEntity}
                        draggable
                        onDragStart={(e) => {
                            e.dataTransfer.setData(`${safeLower(cardEntity)}Id`, item.id);
                            handleDragStart();
                        }}
                        onDragEnd={handleDragEnd}
                        onContextMenu={(e) => {
                            e.preventDefault();
                            console.log('Right-clicked', item);
                        }}
                    />
                ),
                isSidebarOpen,
                setIsSidebarOpen,
                isOverSidebar,
                onSidebarDragOver: () => setIsOverSidebar(true),
                onSidebarDragLeave: () => setIsOverSidebar(false),
            }}
        >
            {PageContent}
        </MainLayout>
    );
}