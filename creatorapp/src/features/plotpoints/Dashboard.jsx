// 📁 Dashboard.jsx
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CalendarGrid from './CalendarGrid';
//import PlotPointModal from './PlotPointModal';
//import UnassignedSidebar from '../sidebars/UnassignedSidebar';
import Card from '../../components/Card';
import { fetchDashboard } from './PlotPointApi';
import { EntityDeleter } from '../../store/EntityManager';
import { useEntityContextMenu } from '../../hooks/useEntityContextMenu';
import './Dashboard.css';
import './CalendarGrid.css';
import './CalendarDayCell.css';
import { useOutletContext } from 'react-router-dom';



export default function Dashboard() {
    const [calendarGrid, setCalendarGrid] = useState([]);
    //const [cards, setCards] = useState([]);
    const { cards, setCards } = useOutletContext();
    const [collapsedMonths, setCollapsedMonths] = useState({});
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [editingId, setEditingId] = useState(null);

    useEffect(() => {
        async function load() {
            try {
                const { calendarGrid } = await fetchDashboard();
                setCalendarGrid(calendarGrid);
            } catch (error) {
                console.error('🚨 Dashboard load failed:', error);
            }
        }
        load();
    }, []);

    const handleDeleteEntity = async (entityType, id) => {
        await EntityDeleter.delete(entityType, id);
        //check if Id or id
        setCards(prev => prev.filter(c => c.cardData.Id !== id));
    };

    const handleUpdateCard = (updated) => {
        setCards(prev =>
            //check if Id or id
            prev.map(c => c.cardData.Id === updated.id ? { ...c, CardData: updated } : c)
        );
    };

    const {
        showContextMenu,
        openContextMenu,
        contextMenuPortal
    } = useEntityContextMenu({
        onCreate: () => setShowModal(true),
        onEdit: (entity) => {
            setEditingId(entity.id);
            setShowModal(true);
        },
        onDelete: (entity) => handleDeleteEntity(entity.entityType, entity.id),
    });

    function getEditableCardData() {
        if (editingId) {
            //check if Id or id
            const existing = cards.find(c => c.cardData.Id === editingId);
            return existing ? { ...existing.CardData } : {};
        }

        // for "create" mode: return a skeleton with default fields
        return {
            entityType: "PlotPoint", // or whatever default you prefer
            Name: "",
            Title: "",
            ChapterTitle: "",
            startDateId: null,
            endDateId: null,
            // add more based on schema
        };
    }



    return (
        <>
            <div className="sticky-header">
                //Placeholder for header content
            </div>

            <CalendarGrid
                calendarDays={calendarGrid}
                cards={cards}
                collapsedMonths={collapsedMonths}
                setCollapsedMonths={setCollapsedMonths}
                onContextMenu={showContextMenu}
                onUpdateCard={handleUpdateCard}
                onCardDrop={async (payload) => {
                    await axios.put('/api/cards/drop', payload);
                    const updated = await axios.get(`/api/cards/${payload.entityType}/${payload.entityId}`);
                    setCards(prev =>
                        prev.map(c => c.cardData.Id === payload.entityId ? updated.data : c)
                    );
                }}
            />

            {contextMenuPortal}

            {showModal && (
                <div className="inline-card-editor">
                    <Card
                        key={editingId || 'new'}
                        cards={{
                            CardData: getEditableCardData(),
                            DisplayMode: 'full',
                            Styling: {}
                        }}
                        mode="full"
                        onClick={() => {
                            setShowModal(false);
                            setEditingId(null);
                        }}
                    />
                    <button
                        className="inline-card-close"
                        onClick={() => {
                            setShowModal(false);
                            setEditingId(null);
                        }}
                    >
                        ✕ Close
                    </button>
                </div>
            )}
        </>
    );
}