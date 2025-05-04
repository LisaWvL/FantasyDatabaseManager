// 📁 MainLayout.jsx
import { useEffect, useState, useRef } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import axios from 'axios';

import '../styles/MainLayout.css';
import NavSidebar from '../features/sidebars/NavSidebar';
import UnassignedSidebar from '../features/sidebars/UnassignedSidebar';
import '../features/sidebars/UnassignedSidebar.css';
import '../features/sidebars/NavSidebar.css';

import { getDashboardCards } from '../features/plotpoints/PlotPointApi';

export default function MainLayout({ headerContent }) {
    const [leftSidebarOpen, setLeftSidebarOpen] = useState(true);
    const [rightSidebarOpen, setRightSidebarOpen] = useState(true);
    const [cards, setCards] = useState([]);
    const location = useLocation();
    const hasLoadedRef = useRef(false);

    // ✅ Fetch cards once
    useEffect(() => {
        if (hasLoadedRef.current) return;
        hasLoadedRef.current = true;
        loadCards();
    }, []);

    const loadCards = async () => {
        try {
            const { cards } = await getDashboardCards();
            setCards(cards);
        } catch (err) {
            console.error('❌ Failed to fetch dashboard data in MainLayout:', err);
        }
    };

    // ✅ Define refreshCards so we can pass it down
    const refreshCards = async () => {
        try {
            const { cards: updated } = await getDashboardCards();
            setCards(updated);
        } catch (err) {
            console.error('❌ Failed to refresh cards:', err);
        }
    };

    return (
        <div className="app-shell">
            <div className="sticky-header">
                {headerContent || <h1>Worldbuilder</h1>}
                <div className="sticky-header">
                    <button className="add-new-button" onClick={() => setShowModal(true)}>＋ Add</button>
                </div>
            </div>

            <div className="layout-row">
                <div className={`left-sidebar ${leftSidebarOpen ? 'open' : 'collapsed'}`}>
                    <NavSidebar show={leftSidebarOpen} toggle={() => setLeftSidebarOpen(prev => !prev)} />
                </div>

                <div className={`main-container ${rightSidebarOpen ? 'with-unassigned-sidebar' : 'sidebar-collapsed'}`}>
                    {/* 🧠 Outlet context passes cards and setter */}
                    <Outlet context={{ cards, setCards }} />
                </div>

                <div className={`unassigned-sidebar ${rightSidebarOpen ? 'open' : 'collapsed'}`}>
                    <UnassignedSidebar
                        isSidebarOpen={rightSidebarOpen}
                        setIsSidebarOpen={setRightSidebarOpen}
                        allCards={cards}
                        refreshCards={refreshCards} // ✅ NOW we pass it
                    />
                </div>
            </div>
        </div>
    );
}



//import { useState, useEffect } from 'react';
//import { Outlet, useLocation } from 'react-router-dom'; // ✅ fixed import
//import '../styles/MainLayout.css';

//import NavSidebar from '../features/sidebars/NavSidebar';
//import UnassignedSidebar from '../features/sidebars/UnassignedSidebar';
//import { fetchDashboard } from '../features/plotpoints/PlotPointApi';

//export default function MainLayout({ headerContent }) {
//    const [leftSidebarOpen, setLeftSidebarOpen] = useState(true);
//    const [rightSidebarOpen, setRightSidebarOpen] = useState(true);
//    const [cards, setCards] = useState([]);
//    const [calendarGrid, setCalendarGrid] = useState([]);


//    const location = useLocation();

//    // ✅ Fetch cards once when MainLayout loads
//    useEffect(() => {
//        async function load() {
//            try {
//                const { data } = await fetchDashboard();
//                setCards(data.cards);
//                setCalendarGrid(data.calendarGrid);
//            } catch (err) {
//                console.error('❌ Failed to fetch dashboard data in MainLayout:', err);
//            }
//        }
//        load();
//    }, []);

//    return (
//        <div className="app-shell">
//            {/* Sticky Header */}
//            <div className="sticky-header">
//                {headerContent || <h1>Worldbuilder</h1>}
//            </div>

//            {/* Layout Row */}
//            <div className="layout-row">
//                {/* Left Sidebar */}
//                <div className={`left-sidebar ${leftSidebarOpen ? 'open' : 'collapsed'}`}>
//                    <NavSidebar
//                        show={leftSidebarOpen}
//                        toggle={() => setLeftSidebarOpen(prev => !prev)}
//                    />
//                </div>

//                {/* Main Content with shared card state */}
//                <div className={`main-container`}>
//                    <Outlet context={{ cards, setCards, calendarGrid }} />
//                </div>

//                {/* Right Sidebar — Always Mounted */}
//                <div className={`right-sidebar ${rightSidebarOpen ? 'open' : 'collapsed'}`}>
//                    <UnassignedSidebar
//                        isSidebarOpen={rightSidebarOpen}
//                        setIsSidebarOpen={setRightSidebarOpen}
//                        allCards={cards} // ✅ passes current card list
//                        onContextMenu={(e, card, entityType) => {
//                            console.log('🧭 Context menu:', card, entityType);
//                            // hook up actual logic later
//                        }}
//                    />
//                </div>
//            </div>
//        </div>
//    );
//}


