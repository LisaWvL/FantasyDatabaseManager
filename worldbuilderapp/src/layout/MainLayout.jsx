// src/layout/MainLayout.jsx
import { useState } from 'react';
import Sidebar from '../components/Sidebar.jsx';
import { Outlet } from 'react-router-dom';

export default function MainLayout() {
    const [sidebarOpen, setSidebarOpen] = useState();

    const toggleSidebar = () => {
        setSidebarOpen(prev => !prev);
    };

    return (
        <div className="app-shell">
            <Sidebar show={sidebarOpen} toggle={toggleSidebar} />
            <div className={`main-content ${sidebarOpen ? 'with-sidebar' : ''}`}>
                <Outlet />
            </div>
        </div>
    );
}
