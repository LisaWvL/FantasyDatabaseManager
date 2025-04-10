// src/pages/EventsPage.jsx
import React, { useEffect, useState } from 'react';
import {
  fetchEvents,
  createEvent,
  updateEvent,
  deleteEvent,
  //fetchDuplicateForChapter
} from '../events/EventApi';

export default function EventsPage() {
  const [events, setEvents] = useState([]);
  const [editingEvent, setEditingEvent] = useState(null);
  const [newEvent, setNewEvent] = useState({ name: '', description: '' });

  useEffect(() => {
    loadEvents();
  }, []);

  const loadEvents = async () => {
    const data = await fetchEvents();
    setEvents(data);
  };

  const handleCreate = async () => {
    await createEvent(newEvent);
    setNewEvent({ name: '', description: '' });
    await loadEvents();
  };

  const handleEdit = async (id) => {
    const selected = events.find((e) => e.id === id);
    setEditingEvent({ ...selected });
  };

  const handleUpdate = async () => {
    await updateEvent(editingEvent.id, editingEvent);
    setEditingEvent(null);
    await loadEvents();
  };

  const handleDelete = async (id) => {
    if (confirm('Delete this event?')) {
      await deleteEvent(id);
      await loadEvents();
    }
  };

  return (
    <div className="app-container">
      <h2>Events</h2>

      <div className="card">
        <h4>Create New</h4>
        <input
          value={newEvent.name}
          onChange={(e) => setNewEvent({ ...newEvent, name: e.target.value })}
          placeholder="Name"
        />
        <input
          value={newEvent.description}
          onChange={(e) => setNewEvent({ ...newEvent, description: e.target.value })}
          placeholder="Description"
        />
        <button onClick={handleCreate}>Create</button>
      </div>

      <table className="table table-striped mt-4">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Chapter</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {events.map((e) => (
            <tr key={e.id}>
              <td>
                {editingEvent?.id === e.id ? (
                  <input
                    value={editingEvent.name}
                    onChange={(ev) => setEditingEvent({ ...editingEvent, name: ev.target.value })}
                  />
                ) : (
                  e.name
                )}
              </td>
              <td>
                {editingEvent?.id === e.id ? (
                  <input
                    value={editingEvent.description}
                    onChange={(ev) =>
                      setEditingEvent({ ...editingEvent, description: ev.target.value })
                    }
                  />
                ) : (
                  e.description
                )}
              </td>
              <td>{e.chapterName || '-'}</td>
              <td>
                {editingEvent?.id === e.id ? (
                  <button onClick={handleUpdate}>💾 Save</button>
                ) : (
                  <>
                    <button onClick={() => handleEdit(e.id)}>✏️ Edit</button>
                    <button onClick={() => handleCreate(e.id)}>🌀 New Chapter</button>
                    <button onClick={() => handleDelete(e.id)}>🗑️</button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
