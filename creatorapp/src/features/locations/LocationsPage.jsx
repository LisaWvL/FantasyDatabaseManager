// src/pages/LocationPage.jsx
import { useEffect, useState } from 'react';
import {
  fetchLocations,
  //createLocation,
  //updateLocation,
  deleteLocation,
  fetchLocationChapter,
} from '../locations/LocationApi';
import EntityTable from '../entities/EntityTable';

export default function LocationPage() {
  const [locations, setLocations] = useState([]);

  const load = async () => {
    try {
      const data = await fetchLocations();
      setLocations(data);
    } catch (err) {
      console.error('Failed to load locations:', err);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const handleEdit = (id) => {
    alert(`Edit location ${id}`);
    // TODO: open form or modal
  };

  const handleDelete = async (id) => {
    if (confirm('Delete this location?')) {
      await deleteLocation(id);
      await load();
    }
  };

  const handleChapter = async (id) => {
    const chapterDraft = await fetchLocationChapter(id);
    console.log('New location chapter draft:', chapterDraft);
    // TODO: Open chapter editing view
  };

  return (
    <div className="app-container">
      <h2>Locations</h2>
      <EntityTable
        data={locations}
        onEdit={handleEdit}
        onDelete={handleDelete}
        onChapter={handleChapter}
      />
    </div>
  );
}
