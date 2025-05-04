import { useEffect, useState } from 'react';
import Card from '../../components/Card';
import {
    useDragAndDrop,
    useDragScroll,
    createDragOverHandler,
    createDragLeaveHandler,
} from '../../hooks/useDragAndDrop';

import { entitySchemas } from '../../store/EntitySchemas';
import {
    EntityCreator,
    EntityUpdater,
    EntityFetcher,
    EntityDeleter
} from '../../store/EntityManager';
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

    // useEffect(() => {
    //    const loadData = async () => {
    //        const [sectionResults, cardResults] = await Promise.all([
    //            EntityFetcher.fetchAll(sectionEntity),
    //            EntityFetcher.fetchAll(cardEntity)
    //        ]);
    //         setSections(sectionResults);
    //         setCards(cardResults);
    //     };
    //     loadData();
    // }, [cardEntity, sectionEntity]);



    const handleDragStart = () => setIsDragging(true);
    const handleDragEnd = () => setIsDragging(false);

    const handleUpdateCard = async (updatedCard) => {
        await EntityUpdater.update(cardEntity, updatedCard.id, updatedCard);
        setCards(prev => prev.map(c => (c.id === updatedCard.id ? { ...c, ...updatedCard } : c)));
    };

    const handleDeleteEntity = async (card) => {
        await EntityDeleter.delete(cardEntity, card.id);
        setCards(prev => prev.filter(c => c.id !== card.id));
    };

    const handleCreateNewEntity = async () => {
        const payload = { name: `New ${cardEntity}`, [updateFK]: null };
        const created = await EntityCreator.create(cardEntity, payload);
        if (created) setCards(prev => [created, ...prev]);
    };

    const { handleDrop, handleDragStart: dndDragStart } = useDragAndDrop({
        handleUpdateEntity: async ({ entity, dropContext, contextData }) => {
            if (dropContext === 'unassigned-sidebar') {
                await EntityUpdater.setNull(entity.entityType, entity.id, updateFK);
                setCards(prev => prev.map(c => (c.id === entity.id ? { ...c, [updateFK]: null } : c)));
            } else if (dropContext === 'section-dropzone') {
                const sectionId = contextData?.sectionId;
                if (sectionId) {
                    await EntityUpdater.update(entity.entityType, entity.id, {
                        [updateFK]: sectionId
                    });
                    setCards(prev => prev.map(c => (c.id === entity.id ? { ...c, [updateFK]: sectionId } : c)));
                }
            }
        }
    });

    return (
        <div className="entity-page">
            <div className="entity-page-header">
                <h2>{cardSchema.label} Assignment</h2>
                <button onClick={handleCreateNewEntity}>＋ Add {cardSchema.label}</button>
            </div>

            {sections.map(section => {
                const sectionCards = cards.filter(c => c[updateFK] === section.id);
                return (
                    <div
                        key={section.id}
                        className={`section-dropzone ${dragOverSectionId === section.id ? 'drag-over' : ''}`}
                        onDragOver={createDragOverHandler(() => setDragOverSectionId(section.id))}
                        onDragLeave={createDragLeaveHandler(() => setDragOverSectionId(null))}
                        onDrop={(e) => {
                            handleDrop(e, 'section-dropzone', { sectionId: section.id });
                            setDragOverSectionId(null);
                        }}
                    >
                        <div className="section-header">
                            <h3>{section.title || section.name || `${sectionSchema.label} #${section.id}`}</h3>
                        </div>
                        <div className="entity-card-row">
                            {sectionCards.map(card => (
                                <Card
                                    key={card.id}
                                    card={{ CardData: card, DisplayMode: 'basic' }}
                                    mode="basic"
                                    onClick={() => { }}
                                    draggable
                                    onDragStart={(e) => dndDragStart(e, { entityType: cardEntity, id: card.id }, 'section')}
                                    onDragEnd={handleDragEnd}
                                />
                            ))}
                        </div>
                    </div>
                );
            })}
        </div>
    );
}
