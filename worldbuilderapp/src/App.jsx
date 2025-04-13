// src/App.jsx

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import 'react-quill/dist/quill.snow.css';

import MainLayout from './layout/MainLayout';

// Pages
import Dashboard from '../features/plotpoints/Dashboard.jsx';
import ChapterEntityView from '../features/chapters/ChapterEntityView.jsx';
import TimelineStoryView from '../features/timeline/TimelineStoryView.jsx';

import CharactersPage from '../features/characters/CharactersPage.jsx';
import CharacterRelationshipsPage from '../features/characters/CharacterRelationshipsPage.jsx';
import FactionsPage from '../features/factions/FactionsPage.jsx';
import LanguagesPage from '../features/languages/LanguagesPage.jsx';

import ErasPage from '../features/eras/ErasPage.jsx';
import EventsPage from '../features/events/EventsPage.jsx';
import PlotPointsPage from '../features/plotpoints/PlotPointsPage.jsx';

import ItemsPage from '../features/items/ItemsPage.jsx';
import PriceExamplesPage from '../features/items/PriceExamplesPage.jsx';

import LocationsPage from '../features/locations/LocationsPage.jsx';
import RiversPage from '../features/rivers/RiversPage.jsx';
import RoutesPage from '../features/routes/RoutesPage.jsx';
import AssistantPage from '../features/chapters/AssistantPage.jsx';
import WritingAssistantPage from '../features/ai/WritingAssistantPage.jsx';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          {/* Default Dashboard or Timeline View */}
          <Route index element={<Dashboard />} />

          {/* Core Pages */}
          {/*<Route path="dashboard" element={<Dashboard />} />*/}
          <Route path="Dashboard" element={<Dashboard />} />
          <Route path="chapters" element={<ChapterEntityView />} />
          <Route path="TimelineStoryView" element={<TimelineStoryView/>} />


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
