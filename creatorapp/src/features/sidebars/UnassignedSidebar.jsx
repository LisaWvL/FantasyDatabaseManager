import './UnassignedSidebar.css';
import { useState, useEffect } from 'react';
import axios from 'axios';
import {
    useDragAndDrop,
    useDragScroll,
    createDragOverHandler,
    createDragLeaveHandler,
} from '../../utils/DragDropHandlers';
import Card from '../../components/Card';

export default function UnassignedSidebar({
    isSidebarOpen,
    setIsSidebarOpen,
    onContextMenu,
    allCards = []
}) {
    const [isOverSidebar, setIsOverSidebar] = useState(false);
    const [selectedIds, setSelectedIds] = useState(new Set());
    const [lastSelectedId, setLastSelectedId] = useState(null);

    const unassignedCards = allCards.filter(card => {
        const isUnassigned = card.targetZone === 'unassigned';
        const hasId = card.cardData?.Id ?? card.cardData?.id ?? card.CardData?.Id ?? card.CardData?.id ?? card.id;
        return isUnassigned && hasId !== undefined;
    });

    const { handleDrop } = useDragAndDrop({
        handleUpdateEntity: async ({ entity, dragSourceContext }) => {
            try {
                await axios.put('/api/cards/dropToUnassigned', {
                    Id: entity.id,
                    EntityType: entity.entityType,
                    FromContext: dragSourceContext
                });
                console.log('✅ [UnassignedSidebar] Dropped and cleared.');
                // Rely on global refresh or re-fetch to update sidebar
            } catch (error) {
                console.error('❌ Drop failed:', error);
            }
        }
    });

    useDragScroll(isOverSidebar);

    const toggleSelection = (e, itemId) => {
        if (e.shiftKey && lastSelectedId !== null) {
            const allIds = unassignedCards.map(c => c.CardData?.Id ?? c.CardData?.id ?? c.cardData?.Id);
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
                onDragOver={createDragOverHandler(setIsOverSidebar)}
                onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
                onDrop={(e) => {
                    handleDrop(e, 'unassignedsidebar');
                    setIsOverSidebar(false);
                }}
            >
                <h3>Unassigned Items</h3>

                <div
                    className="unassigned-dropzone"
                    onDragOver={createDragOverHandler(setIsOverSidebar)}
                    onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
                    onDrop={(e) => {
                        handleDrop(e, 'unassigned-dropzone');
                        setIsOverSidebar(false);
                    }}
                >
                    {unassignedCards.map(card => {
                        const id = card.CardData?.Id ?? card.CardData?.id ?? card.cardData?.Id ?? card.cardData?.id ?? card.id ?? 0;
                        const entityType = card.CardData?.entityType ?? card.cardData?.entityType ?? card.entityType ?? card.EntityType ?? 'Unknown';

                        return (
                            <div
                                key={`${entityType}_${id}`}
                                className={`unassigned-item ${selectedIds.has(id) ? 'selected' : ''}`}
                                onClick={(e) => toggleSelection(e, id)}
                                onContextMenu={(e) => {
                                    e.preventDefault();
                                    onContextMenu?.(e, card, entityType);
                                }}
                            >
                                <Card card={card} mode="compact" />
                            </div>
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
//import { useState, useEffect } from 'react';
//import axios from 'axios';
//import {
//    useDragAndDrop,
//    useDragScroll,
//    createDragOverHandler,
//    createDragLeaveHandler,
//} from '../../utils/DragDropHandlers';
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

//    // 🧭 Log all incoming cards once on load or when they change
//    useEffect(() => {
//        console.log('📦 [UnassignedSidebar] Received allCards:', allCards);
//    }, [allCards]);

//    const unassignedCards = allCards.filter(card => {
//        const isUnassigned = card.targetZone === 'unassigned';
//        if (!isUnassigned) return false;
//        const hasValidData =
//            (card.cardData?.Id ?? card.cardData?.id ?? card.CardData?.Id ?? card.CardData?.id) !== undefined;
//        if (!hasValidData) {
//            console.warn('⚠️ [UnassignedSidebar] Skipping card with missing CardData.Id:', card);
//        }
//        return isUnassigned && hasValidData;
//    });

//    console.log('📌 [UnassignedSidebar] Filtered unassignedCards:', unassignedCards);

//    const { handleDrop } = useDragAndDrop({
//        handleUpdateEntity: async ({ entity, dragSourceContext }) => {
//            console.log('📤 [UnassignedSidebar] Dropped entity to unassigned:', { entity, dragSourceContext });
//            try {
//                await axios.put('/api/cards/dropToUnassigned', {
//                    EntityId: entity.id,
//                    EntityType: entity.entityType,
//                    FromContext: dragSourceContext
//                });
//                console.log('✅ [UnassignedSidebar] Drop handled successfully');
//            } catch (err) {
//                console.error('❌ [UnassignedSidebar] Failed to drop to unassigned:', err);
//            }
//        }
//    });

//    useDragScroll(isOverSidebar);

//    const toggleSelection = (e, itemId) => {
//        console.log('🖱️ [UnassignedSidebar] Toggling selection for itemId:', itemId);
//        if (e.shiftKey && lastSelectedId !== null) {
//            //check if Id or id
//            const allIds = unassignedCards.map(c => c.CardData.Id);
//            const currentIndex = allIds.indexOf(itemId);
//            const lastIndex = allIds.indexOf(lastSelectedId);
//            const [start, end] = [Math.min(currentIndex, lastIndex), Math.max(currentIndex, lastIndex)];
//            const newIds = allIds.slice(start, end + 1);
//            setSelectedIds(new Set([...selectedIds, ...newIds]));
//            console.log('🔢 [UnassignedSidebar] Shift select range:', newIds);
//        } else {
//            const newSet = new Set(selectedIds);
//            if (newSet.has(itemId)) {
//                newSet.delete(itemId);
//                console.log('➖ [UnassignedSidebar] Deselected:', itemId);
//            } else {
//                newSet.add(itemId);
//                console.log('➕ [UnassignedSidebar] Selected:', itemId);
//            }
//            setSelectedIds(newSet);
//            setLastSelectedId(itemId);
//        }
//    };

//    return (
//        <div className={`unassigned-sidebar-wrapper ${isSidebarOpen ? 'open' : 'collapsed'}`}>
//            <div
//                className={`unassigned-sidebar ${isSidebarOpen ? 'open' : 'collapsed'} ${isOverSidebar ? 'highlight-drop' : ''}`}
//                onDragOver={createDragOverHandler(setIsOverSidebar)}
//                onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
//                onDrop={(e) => {
//                    console.log('📥 [UnassignedSidebar] Drop triggered on sidebar');
//                    handleDrop(e, 'unassignedsidebar');
//                    setIsOverSidebar(false);
//                }}
//            >
//                <button
//                    className="unassigned-sidebar-toggle"
//                    onClick={() => {
//                        console.log('🧭 [UnassignedSidebar] Toggled sidebar');
//                        setIsSidebarOpen?.(!isSidebarOpen);
//                    }}
//                >
//                    MORE
//                </button>

//                <h3 style={{ padding: '1rem', margin: 0 }}>Unassigned Items</h3>

//                <div
//                    className="unassigned-dropzone"
//                    onDragOver={createDragOverHandler(setIsOverSidebar)}
//                    onDragLeave={createDragLeaveHandler(setIsOverSidebar)}
//                    onDrop={(e) => {
//                        console.log('📥 [UnassignedSidebar] Drop triggered on dropzone');
//                        handleDrop(e, 'unassigned-dropzone');
//                        setIsOverSidebar(false);
//                    }}
//                >
//                    {unassignedCards.map(card => {
//                        const id =
//                            card.cardData?.Id ??
//                            card.cardData?.id ??
//                            card.CardData?.Id ??
//                            card.CardData?.id ??
//                            card.id ??
//                            0;

//                        const entityType =
//                            card.cardData?.entityType ??
//                            card.CardData?.entityType ??
//                            card.entityType ??
//                            card.EntityType ??
//                            'Unknown';

//                        console.log(`📋 [UnassignedSidebar] Rendering card: ${entityType} #${id}`);

//                        return (
//                            <div
//                                key={`${entityType}_${id}`}
//                                className={`unassigned-item ${selectedIds.has(id) ? 'selected' : ''}`}
//                                onClick={(e) => toggleSelection(e, id)}
//                                onContextMenu={(e) => {
//                                    e.preventDefault();
//                                    onContextMenu?.(e, card, entityType);
//                                }}
//                            >
//                                <Card card={card} mode="compact" />
//                            </div>
//                        );
//                    })}
//                </div>
//            </div>
//        </div>
//    );
//}
