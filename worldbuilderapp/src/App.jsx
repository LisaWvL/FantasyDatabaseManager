// src/App.jsx

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import 'react-quill/dist/quill.snow.css';

import MainLayout from './layout/MainLayout';

// Pages
import Dashboard from './pages/Dashboard.jsx';
import CalendarPlotView from './pages/CalendarPlotView.jsx';
import ChapterEntityView from './pages/ChapterEntityView.jsx';

import CharactersPage from './pages/CharactersPage.jsx';
import CharacterRelationshipsPage from './pages/CharacterRelationshipsPage.jsx';
import FactionsPage from './pages/FactionsPage.jsx';
import LanguagesPage from './pages/LanguagesPage.jsx';

import ErasPage from './pages/ErasPage.jsx';
import EventsPage from './pages/EventsPage.jsx';
import PlotPointsPage from './pages/PlotPointsPage.jsx';
import TimelineStoryView from './pages/TimelineStoryView.jsx';

import ItemsPage from './pages/ItemsPage.jsx';
import PriceExamplesPage from './pages/PriceExamplesPage.jsx';

import LocationsPage from './pages/LocationsPage.jsx';
import RiversPage from './pages/RiversPage.jsx';
import RoutesPage from './pages/RoutesPage.jsx';
import AssistantPage from './pages/AssistantPage.jsx';
import WritingAssistantPage from './pages/WritingAssistantPage.jsx';


function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    {/* Default Dashboard or Timeline View */}
                    <Route index element={<CalendarPlotView />} />

                    {/* Core Pages */}
                    <Route path="dashboard" element={<Dashboard />} />
                    <Route path="calendar" element={<CalendarPlotView />} />
                    <Route path="chapters" element={<ChapterEntityView />} />

                    {/* Characters & Relations */}
                    <Route path="characters" element={<CharactersPage />} />
                    <Route path="character-relationships" element={<CharacterRelationshipsPage />} />
                    <Route path="factions" element={<FactionsPage />} />
                    <Route path="languages" element={<LanguagesPage />} />

                    {/* Timeline */}
                    <Route path="eras" element={<ErasPage />} />
                    <Route path="events" element={<EventsPage />} />
                    <Route path="plotpoints" element={<PlotPointsPage />} />
                    <Route path="/timelinestoryview" element={<TimelineStoryView />} />

                    {/* Items */}
                    <Route path="items" element={<ItemsPage />} />
                    <Route path="price-examples" element={<PriceExamplesPage />} />

                    {/* Geography */}
                    <Route path="locations" element={<LocationsPage />} />
                    <Route path="rivers" element={<RiversPage />} />
                    <Route path="routes" element={<RoutesPage />} />

                    {/* Writing Assistant */}
                    <Route path="/assistant" element={<AssistantPage />} />
                    <Route path="writing-assistant" element={<WritingAssistantPage />} />
                </Route>
            </Routes>
        </Router>
    );
}

export default App;
