import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import 'react-quill/dist/quill.snow.css';

import MainLayout from './styles/MainLayout.jsx';

import Dashboard from '../src/features/plotpoints/Dashboard.jsx';
import ChapterEntityView from '../src/features/chapters/ChapterEntityView.jsx';
import TimelineStoryView from '../src/features/timeline/TimelineStoryView.jsx';

import CharactersPage from '../src/features/characters/CharactersPage.jsx';
import CharacterRelationshipsPage from '../src/features/characters/CharacterRelationshipsPage.jsx';
import FactionsPage from '../src/features/factions/FactionsPage.jsx';
import LanguagesPage from '../src/features/languages/LanguagesPage.jsx';

import ErasPage from '../src/features/eras/ErasPage.jsx';
import EventsPage from '../src/features/events/EventsPage.jsx';
import PlotPointsPage from '../src/features/plotpoints/PlotPointsPage.jsx';

import ItemsPage from '../src/features/items/ItemsPage.jsx';
import PriceExamplesPage from '../src/features/items/PriceExamplesPage.jsx';

import LocationsPage from '../src/features/locations/LocationsPage.jsx';
import RiversPage from '../src/features/rivers/RiversPage.jsx';
import RoutesPage from '../src/features/routes/RoutesPage.jsx';
import AssistantPage from '../src/features/ai/AssistantPage.jsx';
import WritingAssistantPage from '../src/features/chapters/WritingAssistantPage.jsx';

export default function App() {
    return (
        <Router>
            <Routes>
                <Route element={<MainLayout />}>
                    <Route index element={<Dashboard />} />
                    <Route path="dashboard" element={<Dashboard />} />
                    <Route path="chapters" element={<ChapterEntityView />} />
                    <Route path="timelinestoryview" element={<TimelineStoryView />} />
                    <Route path="characters" element={<CharactersPage />} />
                    <Route path="character-relationships" element={<CharacterRelationshipsPage />} />
                    <Route path="factions" element={<FactionsPage />} />
                    <Route path="languages" element={<LanguagesPage />} />
                    <Route path="eras" element={<ErasPage />} />
                    <Route path="events" element={<EventsPage />} />
                    <Route path="plotpoints" element={<PlotPointsPage />} />
                    <Route path="items" element={<ItemsPage />} />
                    <Route path="price-examples" element={<PriceExamplesPage />} />
                    <Route path="locations" element={<LocationsPage />} />
                    <Route path="rivers" element={<RiversPage />} />
                    <Route path="routes" element={<RoutesPage />} />
                    <Route path="assistant" element={<AssistantPage />} />
                    <Route path="writing-assistant" element={<WritingAssistantPage />} />
                </Route>
            </Routes>
        </Router>
    );
}


