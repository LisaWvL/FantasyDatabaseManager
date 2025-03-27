// src/components/Sidebar.jsx
import { Link } from 'react-router-dom';
import '../styles/Sidebar.css'; // optional: move styles here

export default function Sidebar({ show, toggle }) {
    return (
        <div className={`sidebar ${show ? 'open' : 'collapsed'}`}>
            {/* Toggle Arrow */}
            <button className="sidebar-toggle-btn" onClick={toggle}>
                {show ? '❮' : '❯'}
            </button>

            <h2>Worldbuilder</h2>
            <nav>
                <SidebarGroup title="Characters">
                    <SidebarLink to="/characters">Characters</SidebarLink>
                    <SidebarLink to="/character-relationships">Relationships</SidebarLink>
                    <SidebarLink to="/factions">Factions</SidebarLink>
                    <SidebarLink to="/languages">Languages</SidebarLink>
                </SidebarGroup>

                <SidebarGroup title="Timeline">
                    <SidebarLink to="/eras">Eras</SidebarLink>
                    <SidebarLink to="/plotpoints">PlotPoints</SidebarLink>
                    <SidebarLink to="/snapshots">Snapshots</SidebarLink>
                    <SidebarLink to="/events">Events</SidebarLink>
                </SidebarGroup>

                <SidebarGroup title="Items">
                    <SidebarLink to="/items">Items</SidebarLink>
                    <SidebarLink to="/price-examples">Price Examples</SidebarLink>
                </SidebarGroup>

                <SidebarGroup title="Geography">
                    <SidebarLink to="/locations">Locations</SidebarLink>
                    <SidebarLink to="/rivers">Rivers</SidebarLink>
                    <SidebarLink to="/routes">Routes</SidebarLink>
                </SidebarGroup>
            </nav>
        </div>
    );
}

function SidebarLink({ to, children }) {
    return <li><Link to={to}>{children}</Link></li>;
}

function SidebarGroup({ title, children }) {
    return (
        <div className="nav-group">
            <h4>{title}</h4>
            <ul>{children}</ul>
        </div>
    );
}
