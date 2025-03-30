//TODO
// 1. Create a Sidebar component that will be used to display the navigation links.
// 2. The Sidebar component should accept two props: show and toggle.
// 3. The show prop will determine whether the sidebar is visible or hidden.
// 4. The toggle prop will be a function that toggles the visibility of the sidebar.
// 5. The Sidebar component should display a list of navigation links.
// 6. Each navigation link should be a Link component from react-router-dom.
// 7. The Sidebar component should have a toggle button that changes the visibility of the sidebar.
// 8. The toggle button should display a left arrow when the sidebar is visible and a right arrow when the sidebar is hidden.
// 9. The Sidebar component should have a title at the top.
// 10. The Sidebar component should have groups of navigation links with titles.
// 11. The Sidebar component should be styled with CSS.
// 12. The Sidebar component should be exported as the default export.
// 13. The Sidebar component should be created in the components folder.
// 14. The Sidebar component should be imported in the App.js file.
// 15. The Sidebar component should be rendered in the SidebarLayout component.
// 16. The Sidebar component should be used to navigate between different sections of the application.
// 17. The Sidebar component should be responsive and collapse on smaller screens.


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

                <SidebarGroup title="AI Assistant">
                    <SidebarLink to="/assistant">Writing Assistant</SidebarLink>

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
