// src/components/NavSidebar.tsx
import { Link } from 'react-router-dom';
import './NavSidebar.css';

export default function NavSidebar({ show, toggle }) {
    return (
        <div className={`navsidebar-wrapper ${show ? 'open' : 'collapsed'}`}>
            <div className={`navsidebar ${show ? 'open' : 'collapsed'}`}>
                <div className="theme-toggle">
                    <label>
                        <input
                            type="checkbox"
                            onChange={(e) => {
                                const grim = e.target.checked;
                                document.documentElement.classList.toggle('grim', grim);
                                document.body.classList.toggle('grim', grim);
                                localStorage.setItem('grimTheme', grim ? '1' : '0');
                            }}
                        />
                        Grim Theme
                    </label>
                </div>
                <nav>
                    <SidebarGroup title="Planning">
                        <SidebarLink to="/Dashboard">Dashboard</SidebarLink>
                        <SidebarLink to="/TimelineStoryView">Timeline View</SidebarLink>
                        <SidebarLink to="/plotpoints">PlotPoints</SidebarLink>
                    </SidebarGroup>

                    <SidebarGroup title="Characters">
                        <SidebarLink to="/characters">Characters</SidebarLink>
                        <SidebarLink to="/character-relationships">Relationships</SidebarLink>
                        <SidebarLink to="/factions">Factions</SidebarLink>
                        <SidebarLink to="/languages">Languages</SidebarLink>
                    </SidebarGroup>

                    <SidebarGroup title="Timeline">
                        <SidebarLink to="/eras">Eras</SidebarLink>
                        <SidebarLink to="/chapters">Chapters</SidebarLink>
                        <SidebarLink to="/events">Events</SidebarLink>
                        <SidebarLink to="/timelinestoryview">Timeline View</SidebarLink>
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

                    <SidebarGroup title="Writing Assistant">
                        <SidebarLink to="/writing-assistant">Writing Assistant</SidebarLink>
                        <SidebarLink to="/assistant">AI Assistant</SidebarLink>
                    </SidebarGroup>
                </nav>
            </div>
            <button className="navsidebar-toggle-btn" onClick={toggle}>
                {show ? '❮' : '❯'}
            </button>
        </div>
    );
}

function SidebarLink({ to, children }) {
    return <li><Link to={to}>{children}</Link></li>;
}

function SidebarGroup({ title, children }) {
    return (
        <div className="nav-group">
            <h2>{title}</h2>
            <ul>{children}</ul>
        </div>
    );
}