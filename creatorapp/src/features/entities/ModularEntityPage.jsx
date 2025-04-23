import { useEffect, useState } from 'react';
import MainLayout from '../../styles/MainLayout';
import Card from '../../components/Card';
import ModularEntityCard from '../../components/Card';
import { safeLower } from '../../utils/UpperLowerCase';
import useDragScroll from '../../hooks/useDragScroll';
import { useDragAndDrop } from '../../utils/DragDropHandlers';
import { entitySchemas } from '../../store/EntitySchemas';
import {
    EntityCreator,
    EntityUpdater,
    EntityFetcher,
    EntityDeleter
} from '../../store/EntityManager'; // <- You’ll need to implement these
import './ModularEntityPage.css';

export default function ModularEntityPage({ cardEntity, sectionEntity, updateFK }) {
    const [cards, setCards] = useState([]);
    const [sections, setSections] = useState([]);
    const [dragOverSectionId, setDragOverSectionId] = useState(null);
    const [isDragging, setIsDragging] = useState(false);
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [isOverSidebar, setIsOverSidebar] = useState(false);

    const cardSchema = entitySchemas[cardEntity];
    const sectionSchema = entitySchemas[sectionEntity];

    useDragScroll(isDragging);

    useEffect(() => {
        const loadData = async () => {
            const [sectionResults, cardResults] = await Promise.all([
                fetchEntities(sectionEntity),
                fetchEntities(cardEntity)
            ]);
            setSections(sectionResults);
            setCards(cardResults);
        };
        loadData();
    }, [cardEntity, sectionEntity]);

    const handleDragStart = () => setIsDragging(true);
    const handleDragEnd = () => setIsDragging(false);

    const handleUpdateCard = async (updatedCard) => {
        await updateEntity(cardEntity, updatedCard.id, updatedCard);
        setCards((prev) =>
            prev.map((c) => (c.id === updatedCard.id ? { ...c, ...updatedCard } : c))
        );
    };

    const handleDeleteEntity = async (card) => {
        await deleteEntity(cardEntity, card.id);
        setCards((prev) => prev.filter((c) => c.id !== card.id));
    };

    const handleCreateNewEntity = async () => {
        const payload = { name: `New ${cardEntity}`, [updateFK]: null };
        const created = await createEntity(cardEntity, payload);
        if (created) setCards((prev) => [created, ...prev]);
    };

    const { handleDrop } = useDragAndDrop({
        handleUpdateEntity: async ({ entity, dropTarget, context }) => {
            if (dropTarget === 'unassigned-sidebar') {
                await setFieldToNull(entity.entityType, entity.id, updateFK);
                setCards((prev) =>
                    prev.map((c) =>
                        c.id === entity.id ? { ...c, [updateFK]: null } : c
                    )
                );
            } else if (dropTarget === 'context-dropzone') {
                const chapterId = parseInt(context?.chapterId);
                if (chapterId) {
                    await updateEntity(entity.entityType, entity.id, {
                        [updateFK]: chapterId,
                    });
                    setCards((prev) =>
                        prev.map((c) =>
                            c.id === entity.id ? { ...c, [updateFK]: chapterId } : c
                        )
                    );
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
                                    onFieldUpdate={handleUpdateCard}
                                    onDelete={() => handleDeleteEntity(card)}
                                    onCreateNewVersion={handleCreateNewEntity}
                                    draggable
                                    onResizeEnd={(newWidth) => {
                                        handleUpdateCard({ ...card, width: newWidth });
                                    }}
                                    onDragStart={(e) => {
                                        e.dataTransfer.setData(`${safeLower(cardEntity)}Id`, card.id);
                                        handleDragStart();
                                    }}
                                    onDragEnd={handleDragEnd}
                                    onContextMenu={(e) => {
                                        e.preventDefault();
                                        console.log('Right-clicked', card);
                                    }}
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
                onDropToUnassigned: async (cardId) => {
                    const card = cards.find((c) => c.id === cardId);
                    if (!card) return;
                    await setFieldToNull(cardEntity, card.id, updateFK);
                    setCards((prev) =>
                        prev.map((c) =>
                            c.id === card.id ? { ...c, [updateFK]: null } : c
                        )
                    );
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
