import React from 'react';
import './DashboardLayout.css';

export default function DashboardLayout({
    headerContent,
    sidebarContent,
    isSidebarOpen,
    setIsSidebarOpen,
    isOverSidebar,
    onSidebarDragOver,
    onSidebarDragLeave,
    onSidebarDrop,
    children
}) {
    return (
        <div className="body-wrapper">
            <div className="sticky-header">
                {headerContent}
            </div>
            <div className="main-body">
                <div className={`content-area ${isSidebarOpen ? 'with-sidebar' : ''}`}>
                    {children}
                </div>

                <div className="floating-sidebar-wrapper">
                    <div
                        className={`floating-sidebar ${isSidebarOpen ? 'open' : ''} ${isOverSidebar ? 'highlight-drop' : ''}`}
                        onDragOver={onSidebarDragOver}
                        onDragLeave={onSidebarDragLeave}
                        onDrop={onSidebarDrop}
                    >
                        <button
                            className="floating-sidebar-toggle"
                            onClick={() => setIsSidebarOpen(!isSidebarOpen)}
                        >
                            MORE
                        </button>

                        {sidebarContent}
                    </div>
                </div>
            </div>
        </div>
    );
}