// 📁 features/plotpoints/UnassignedSidebar.jsx
import './UnassignedSidebar.css';
import { useState } from 'react';
import {
    useDragAndDrop,
    useDragScroll,
    createDragOverHandler,
    createDragLeaveHandler
} from '../../hooks/useDragAndDrop';
import Card from '../../components/Card';

export default function UnassignedSidebar({
    isSidebarOpen,
    setIsSidebarOpen,
    onContextMenu,
    allCards = [],
    refreshCards
}) {
    const [isOverSidebar, setIsOverSidebar] = useState(false);
    const [selectedIds, setSelectedIds] = useState(new Set());
    const [lastSelectedId, setLastSelectedId] = useState(null);

    const unassignedCards = allCards.filter(card => {
        const isUnassigned = card.targetZone === 'unassigned';
        const hasId = card.cardData?.Id ?? card.CardData?.Id ?? card.id;
        return isUnassigned && hasId !== undefined;
    });

    const { handleDrop, handleDragStart } = useDragAndDrop({
        onDropSuccess: async () => {
            console.log('📦 Card successfully unassigned');
            await refreshCards?.(); // ✅ No undefined error if passed
        }
    });

    useDragScroll(isOverSidebar);

    const toggleSelection = (e, itemId) => {
        if (e.shiftKey && lastSelectedId !== null) {
            const allIds = unassignedCards.map(c => c.cardData?.Id ?? c.CardData?.Id);
            const currentIndex = allIds.indexOf(itemId);
            const lastIndex = allIds.indexOf(lastSelectedId);
            const [start, end] = [Math.min(currentIndex, lastIndex), Math.max(currentIndex, lastIndex)];
            const newIds = allIds.slice(start, end + 1);
            setSelectedIds(new Set([...selectedIds, ...newIds]));
        } else {
            const updated = new Set(selectedIds);
            updated.has(itemId) ? updated.delete(itemId) : updated.add(itemId);
            setSelectedIds(updated);
            setLastSelectedId(itemId);
        }
    };

    return (
        <div className={`unassigned-sidebar-wrapper ${isSidebarOpen ? 'open' : 'collapsed'}`}>
            <div
                className={`unassigned-sidebar ${isSidebarOpen ? 'open' : 'collapsed'} ${isOverSidebar ? 'highlight-drop' : ''}`}
            >
                <h3>Unassigned Items</h3>

                <div
                    className="unassigned-dropzone"
                    onDragOver={createDragOverHandler(setIsOverSidebar)}
                    onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
                    onDrop={(e) => handleDrop(e, 'unassigned-dropzone')}
                >
                    {unassignedCards.map(card => {
                        const id = card.cardData?.Id ?? card.CardData?.Id ?? card.id;
                        const entityType =
                            card.cardData?.EntityType ??
                            card.CardData?.EntityType ??
                            card.entityType ??
                            card.EntityType ??
                            'Unknown';

                        return (
                            <Card
                                key={`card-${entityType}-${id}`}
                                card={card}
                                mode="compact"
                                onClick={(e) => toggleSelection(e, id)}
                                onContextMenu={(e) => {
                                    e.preventDefault();
                                    onContextMenu?.(e, card, entityType);
                                }}
                                onDragStart={(e) => {
                                    handleDragStart(e, { entityType, id }, 'unassignedsidebar');
                                }}
                            />
                        );
                    })}
                </div>
            </div>

            <button
                className="unassigned-sidebar-toggle"
                onClick={() => setIsSidebarOpen?.(!isSidebarOpen)}
            >
                MORE
            </button>
        </div>
    );
}



//import './UnassignedSidebar.css';
//import { useState } from 'react';
//import {
//    useDragAndDrop,
//    useDragScroll,
//    createDragOverHandler,
//    createDragLeaveHandler,
//} from '../../hooks/useDragAndDrop';
//import Card from '../../components/Card';

//export default function UnassignedSidebar({
//    isSidebarOpen,
//    setIsSidebarOpen,
//    onContextMenu,
//    allCards = []
//}) {
//    const [isOverSidebar, setIsOverSidebar] = useState(false);
//    const [selectedIds, setSelectedIds] = useState(new Set());
//    const [lastSelectedId, setLastSelectedId] = useState(null);

//    const unassignedCards = allCards.filter(card => {
//        const isUnassigned = card.targetZone === 'unassigned';
//        const hasId = card.cardData?.Id ?? card.cardData?.id ?? card.CardData?.Id ?? card.CardData?.id ?? card.id;
//        return isUnassigned && hasId !== undefined;
//    });

//    const { handleDrop } = useDragAndDrop({
//        onDropSuccess: async () => {
//            console.log('📦 Card successfully unassigned');
//            await refreshCards();
//        }
//    });


//    useDragScroll(isOverSidebar);

//    const toggleSelection = (e, itemId) => {
//        if (e.shiftKey && lastSelectedId !== null) {
//            const allIds = unassignedCards.map(c => c.CardData?.Id ?? c.CardData?.id ?? c.cardData?.Id);
//            const currentIndex = allIds.indexOf(itemId);
//            const lastIndex = allIds.indexOf(lastSelectedId);
//            const [start, end] = [Math.min(currentIndex, lastIndex), Math.max(currentIndex, lastIndex)];
//            const newIds = allIds.slice(start, end + 1);
//            setSelectedIds(new Set([...selectedIds, ...newIds]));
//        } else {
//            const updated = new Set(selectedIds);
//            updated.has(itemId) ? updated.delete(itemId) : updated.add(itemId);
//            setSelectedIds(updated);
//            setLastSelectedId(itemId);
//        }
//    };

//    return (
//        <div className={`unassigned-sidebar-wrapper ${isSidebarOpen ? 'open' : 'collapsed'}`}>
//            <div
//                className={`unassigned-sidebar ${isSidebarOpen ? 'open' : 'collapsed'} ${isOverSidebar ? 'highlight-drop' : ''}`}
//            >
//                <h3>Unassigned Items</h3>

//                <div
//                    className="unassigned-dropzone"
//                    onDragOver={createDragOverHandler(setIsOverSidebar)}
//                    onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
//                    onDrop={(e) => handleDrop(e, 'unassigned-dropzone')}
//                >
//                    {unassignedCards.map(card => {
//                        const id = card.cardData?.Id ?? card.cardData?.id ?? card.CardData?.Id ?? card.CardData?.id ?? card.id ?? 0;
//                        const entityType = card.cardData?.entityType ?? card.CardData?.entityType ?? card.entityType ?? card.EntityType ?? 'Unknown';

//                        return (
//                            <Card
//                                key={`${card.entityType}-${card.cardData?.Id}`}
//                                card={card}
//                                mode="compact"
//                                draggable
//                                onClick={(e) => toggleSelection(e, id)}
//                                onContextMenu={(e) => {
//                                    e.preventDefault();
//                                    onContextMenu?.(e, card, entityType);
//                                }}
//                                onDragStart={(e) => handleDragStart(e, { entityType, id }, 'unassignedsidebar')}
//                            />
//                        );
//                    })}
//                </div>
//            </div>
//            <button
//                className="unassigned-sidebar-toggle"
//                onClick={() => setIsSidebarOpen?.(!isSidebarOpen)}
//            >
//                MORE
//            </button>
//        </div>
//    );
//}
