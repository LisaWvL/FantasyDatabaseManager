//TODO this is my main layout,
//it has a sidebar on the left which can be folded in and out
// add a main content area where different pages are shown
//add a header part, which adapts depending on the content of the content part
//in the top right corner add a search bar which is present at all times
//add a footer to the bottom of the page


// src/layout/MainLayout.jsx

// MainLayout.jsx
import { useState } from 'react';
import Sidebar from '../components/Sidebar.jsx';
import { Outlet } from 'react-router-dom';

export default function MainLayout() {
    const [sidebarOpen, setSidebarOpen] = useState(true);
    const toggleSidebar = () => setSidebarOpen(prev => !prev);

    return (
        <div className="app-shell">
            <Sidebar show={sidebarOpen} toggle={toggleSidebar} />
            <div className={`main-content ${sidebarOpen ? '' : 'expanded'}`}>
                <Outlet />
            </div>
        </div>
    );
}

