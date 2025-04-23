// src/layout/MainLayout.jsx
import { useState } from 'react';
import './MainLayout.css';
import NavSidebar from '../../features/sidebar/NavSidebar.jsx';
import UnassignedSidebar from '../../features/sidebar/UnassignedSidebar.jsx';

export default function MainLayout({ headerContent, children, unassignedSidebar }) {
    const [sidebarOpen, setSidebarOpen] = useState(true);
    const isUnassignedOpen = unassignedSidebar?.isSidebarOpen;

    return (
        <div className="app-shell">
            {/* Sticky Header */}
            <div className="sticky-header">
                {headerContent || <h1>Worldbuilder</h1>}
            </div>

            {/* Main Layout */}
            <div className="layout-row">
                {/* Left Sidebar */}
                <div className={`left-sidebar ${sidebarOpen ? 'open' : 'collapsed'}`}>
                    <NavSidebar show={sidebarOpen} toggle={() => setSidebarOpen((prev) => !prev)} />
                </div>

                {/* Content */}
                <div className={`main-container ${isUnassignedOpen ? 'with-unassigned-sidebar' : 'sidebar-collapsed'}`}>
                    {children}
                </div>

                {/* Right Sidebar */}
                <div className={`right-sidebar ${isUnassignedOpen ? 'open' : 'collapsed'}`}>
                    {unassignedSidebar && (
                        <UnassignedSidebar
                            isSidebarOpen={unassignedSidebar.isSidebarOpen}
                            setIsSidebarOpen={unassignedSidebar.setIsSidebarOpen}
                            onContextMenu={unassignedSidebar.onContextMenu}
                            renderItem={unassignedSidebar.renderItem}
                            entityType={unassignedSidebar.entityType}
                            items={unassignedSidebar.items}
                            isUnassigned={unassignedSidebar.isUnassigned}
                            onDropToUnassigned={unassignedSidebar.onDropToUnassigned}
                        />
                    )}
                </div>
            </div>
            <div className="footer">
                <p>Worldbuilder App © 2025</p>
            </div>
        </div>
    );
}
