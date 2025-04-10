// src/pages/RiverPage.jsx
import React, { useEffect, useState } from 'react';
import {
  fetchRivers,
  createRiver,
  //updateRiver,
  deleteRiver,
} from './RiverApi';

export default function RiverPage() {
  const [rivers, setRivers] = useState([]);
  const [newRiver, setNewRiver] = useState({
    name: '',
    sourceLocationId: null,
    destinationLocationId: null,
  });

  useEffect(() => {
    loadRivers();
  }, []);

  const loadRivers = async () => {
    const data = await fetchRivers();
    setRivers(data);
  };

  const handleCreate = async () => {
    await createRiver(newRiver);
    setNewRiver({ name: '', sourceLocationId: null, destinationLocationId: null });
    loadRivers();
  };

  const handleDelete = async (id) => {
    await deleteRiver(id);
    loadRivers();
  };

  return (
    <div className="app-container">
      <h2>Rivers</h2>

      <div className="card">
        <input
          type="text"
          value={newRiver.name}
          placeholder="River Name"
          onChange={(e) => setNewRiver({ ...newRiver, name: e.target.value })}
        />
        <input
          type="number"
          placeholder="Source Location ID"
          value={newRiver.sourceLocationId || ''}
          onChange={(e) => setNewRiver({ ...newRiver, sourceLocationId: parseInt(e.target.value) })}
        />
        <input
          type="number"
          placeholder="Destination Location ID"
          value={newRiver.destinationLocationId || ''}
          onChange={(e) =>
            setNewRiver({ ...newRiver, destinationLocationId: parseInt(e.target.value) })
          }
        />
        <button onClick={handleCreate}>Add River</button>
      </div>

      <ul>
        {rivers.map((river) => (
          <li key={river.id}>
            {river.name} (From: {river.sourceLocationId}, To: {river.destinationLocationId})
            <button onClick={() => handleDelete(river.id)}>❌</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
