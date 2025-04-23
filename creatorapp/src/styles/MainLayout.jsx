// 📁 MainLayout.jsx
import { useEffect, useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import axios from 'axios';

import '../styles/MainLayout.css';
import NavSidebar from '../features/sidebars/NavSidebar';
import UnassignedSidebar from '../features/sidebars/UnassignedSidebar';
import '../features/sidebars/UnassignedSidebar.css';
import '../features/sidebars/NavSidebar.css';

import { fetchDashboard } from '../features/plotpoints/PlotPointApi';


export default function MainLayout({ headerContent }) {
    const [leftSidebarOpen, setLeftSidebarOpen] = useState(true);
    const [rightSidebarOpen, setRightSidebarOpen] = useState(true);
    const [cards, setCards] = useState([]);
    const [calendarGrid, setCalendarGrid] = useState([]);

    const location = useLocation();

        // ✅ Fetch cards and grid once when MainLayout loads
        useEffect(() => {
            async function load() {
                try {
                    const { calendarGrid, cards } = await fetchDashboard();
                    setCards(cards);
                    setCalendarGrid(calendarGrid);
                } catch (err) {
                    console.error('❌ Failed to fetch dashboard data in MainLayout:', err);
                }
            }
            load();
        }, []);

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
                    <Outlet context={{ cards, setCards, calendarGrid }} />
                </div>

                <div className={`unassigned-sidebar ${rightSidebarOpen ? 'open' : 'collapsed'}`}>
                    <UnassignedSidebar
                        isSidebarOpen={rightSidebarOpen}
                        setIsSidebarOpen={setRightSidebarOpen}
                        allCards={cards}
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


