import { useState } from 'react';
import Sidebar from '../../features/sidebar/Sidebar.jsx';
import { Outlet } from 'react-router-dom';
import { ControlledMenu, MenuItem } from '@szhsin/react-menu';

import '@szhsin/react-menu/dist/index.css';
import '@szhsin/react-menu/dist/theme-dark.css';

export default function MainLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const toggleSidebar = () => setSidebarOpen((prev) => !prev);

  const [menuPosition, setMenuPosition] = useState({ x: 0, y: 0 });
  const [menuVisible, setMenuVisible] = useState(false);

  const handleContextMenu = (e) => {
    e.preventDefault();
    setMenuPosition({ x: e.clientX, y: e.clientY });
    setMenuVisible(true);
  };

  const closeMenu = () => setMenuVisible(false);

  return (
    <div className="app-shell" onContextMenu={handleContextMenu}>
      <Sidebar show={sidebarOpen} toggle={toggleSidebar} />
      <div className={`main-content ${sidebarOpen ? '' : 'expanded'}`}>
        <Outlet />
      </div>

      <ControlledMenu
        anchorPoint={menuPosition}
        state={menuVisible ? 'open' : 'closed'}
        onClose={closeMenu}
        className="context-menu"
      >
        <MenuItem
          onClick={() => {
            console.log('💾 Save');
            closeMenu();
          }}
        >
          Save
        </MenuItem>
        <MenuItem
          onClick={() => {
            console.log('✖ Cancel');
            closeMenu();
          }}
        >
          Cancel
        </MenuItem>
        <MenuItem
          onClick={() => {
            console.log('📋 Copy');
            closeMenu();
          }}
        >
          Copy
        </MenuItem>
      </ControlledMenu>
    </div>
  );
}
