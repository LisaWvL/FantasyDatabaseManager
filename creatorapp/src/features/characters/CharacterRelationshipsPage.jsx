// CharacterPage.jsx

import '../entities/ModularEntityPage.css';
import ModularEntityPage from '../entities/ModularEntityPage';

export default function CharactersPage() {
    return <ModularEntityPage cardEntity="Character2" sectionEntity="Character1" updateFK="ChapterId" />;
}

//no idea how to update this with my current modularEntityPage






//import { useEffect, useRef, useState } from 'react';
//import {
//  fetchCharacterRelationships,
//  createCharacterRelationship,
//  updateCharacterRelationship,
//  deleteCharacterRelationship,
//} from './CharacterRelationshipApi';
//import { fetchCharacters } from '../characters/CharacterApi';
//import { fetchChapters } from '../chapters/ChapterApi';
//import { useRowEditKeyboard } from '../../src/hooks/useRowEditKeyboard';
//import './CharacterRelationshipPage.css';

//export default function CharacterRelationshipPage() {
//  const [relationships, setRelationships] = useState([]);
//  const [characters, setCharacters] = useState([]);
//  const [chapters, setChapters] = useState([]);
//  const [filters, setFilters] = useState({ characterId: '', chapterId: '' });
//  const [editingRowId, setEditingRowId] = useState(null);
//  const [editedData, setEditedData] = useState({});
//  const rowRef = useRef(null);

//  // ✅ Fix: move this ABOVE useRowEditKeyboard
//  const handleCancel = () => {
//    setEditingRowId(null);
//    setEditedData({});
//    loadAll(); // undo changes
//  };

//  async function handleSave() {
//    if (editingRowId === 'new') {
//      const created = await createCharacterRelationship(editedData);
//      setRelationships((prev) => [created, ...prev.slice(1)]);
//    } else {
//      await updateCharacterRelationship(editingRowId, editedData);
//    }
//    setEditingRowId(null);
//    setEditedData({});
//    await loadAll();
//  }

//  useRowEditKeyboard(rowRef, handleSave, handleCancel);

//  useEffect(() => {
//    loadAll();
//  }, []);

//  const loadAll = async () => {
//    const [rels, chars, snaps] = await Promise.all([
//      fetchCharacterRelationships(),
//      fetchCharacters(),
//      fetchChapters(),
//    ]);
//    setRelationships(rels);
//    setCharacters(chars);
//    setChapters(snaps);
//  };

//  const getCharacterName = (id) => {
//    const match = characters.find((c) => Number(c.id) === Number(id));
//    return match?.name || '[None]';
//  };

//  const getChapterName = (id) => {
//    const match = chapters.find((s) => Number(s.id) === Number(id));
//    return match?.chapterName || '[None]';
//  };

//  const handleFilterChange = (e) => {
//    const { name, value } = e.target;
//    setFilters((prev) => ({ ...prev, [name]: value }));
//  };

//  const filtered = relationships.filter((r) => {
//    const { characterId, chapterId } = filters;
//    return (
//      (!characterId ||
//        r.character1Id === parseInt(characterId) ||
//        r.character2Id === parseInt(characterId)) &&
//      (!chapterId || r.chapterId === parseInt(chapterId))
//    );
//  });

//  const handleEdit = (id) => {
//    setEditingRowId(id);
//    const target = relationships.find((r) => r.id === id);
//    setEditedData({ ...target });
//  };

//  const handleChange = (e) => {
//    const { name, value } = e.target;
//    setEditedData((prev) => ({ ...prev, [name]: value }));
//  };

//  const handleDelete = async (id) => {
//    if (confirm('Are you sure you want to delete this relationship?')) {
//      await deleteCharacterRelationship(id);
//      setRelationships((prev) => prev.filter((r) => r.id !== id));
//    }
//  };

//  const handleDuplicate = async (id) => {
//    const rel = relationships.find((r) => r.id === id);
//    const clone = { ...rel, id: undefined };
//    const created = await createCharacterRelationship(clone);
//    setRelationships((prev) => [...prev, created]);
//  };

//  const handleCreate = () => {
//    const blank = {
//      id: 'new',
//      character1Id: '',
//      character2Id: '',
//      relationshipType: '',
//      relationshipDynamic: '',
//      chapterId: '',
//    };
//    setEditingRowId('new');
//    setEditedData(blank);
//    setRelationships((prev) => [blank, ...prev]);
//  };

//  return (
//    <div className="character-relationship-page">
//      <h2>Character Relationships</h2>

//      <div className="filters">
//        <select name="characterId" value={filters.characterId} onChange={handleFilterChange}>
//          <option value="">-- Filter by Character --</option>
//          {characters.map((c) => (
//            <option key={c.id} value={c.id}>
//              {c.name}
//            </option>
//          ))}
//        </select>

//        <select name="chapterId" value={filters.chapterId} onChange={handleFilterChange}>
//          <option value="">-- Filter by Chapter --</option>
//          {chapters.map((s) => (
//            <option key={s.id} value={s.id}>
//              {s.chapterName}
//            </option>
//          ))}
//        </select>

//        <button onClick={handleCreate}>➕ Add Relationship</button>
//      </div>

//      <table className="relationship-table">
//        <thead>
//          <tr>
//            <th>Character 1</th>
//            <th>Character 2</th>
//            <th>Type</th>
//            <th style={{ width: '35%' }}>Dynamic</th>
//            <th>Chapter</th>
//            <th>Actions</th>
//          </tr>
//        </thead>
//        <tbody>
//          {filtered.map((rel) => (
//            <tr
//              key={rel.id}
//              ref={editingRowId === rel.id ? rowRef : null}
//              onDoubleClick={() => handleEdit(rel.id)}
//            >
//              <td>
//                {editingRowId === rel.id ? (
//                  <select
//                    name="character1Id"
//                    value={editedData.character1Id || ''}
//                    onChange={handleChange}
//                  >
//                    {characters.map((c) => (
//                      <option key={c.id} value={c.id}>
//                        {c.name}
//                      </option>
//                    ))}
//                  </select>
//                ) : (
//                  <span>{getCharacterName(rel.character1Id)}</span>
//                )}
//              </td>
//              <td>
//                {editingRowId === rel.id ? (
//                  <select
//                    name="character2Id"
//                    value={editedData.character2Id || ''}
//                    onChange={handleChange}
//                  >
//                    {characters.map((c) => (
//                      <option key={c.id} value={c.id}>
//                        {c.name}
//                      </option>
//                    ))}
//                  </select>
//                ) : (
//                  <span>{getCharacterName(rel.character2Id)}</span>
//                )}
//              </td>
//              <td>
//                {editingRowId === rel.id ? (
//                  <input
//                    name="relationshipType"
//                    value={editedData.relationshipType || ''}
//                    onChange={handleChange}
//                  />
//                ) : (
//                  <span>{rel.relationshipType}</span>
//                )}
//              </td>
//              <td>
//                {editingRowId === rel.id ? (
//                  <textarea
//                    name="relationshipDynamic"
//                    value={editedData.relationshipDynamic || ''}
//                    onChange={handleChange}
//                    style={{ width: '100%', minHeight: '3rem' }}
//                  />
//                ) : (
//                  <span style={{ whiteSpace: 'pre-wrap' }}>{rel.relationshipDynamic}</span>
//                )}
//              </td>
//              <td>
//                {editingRowId === rel.id ? (
//                  <select
//                    name="chapterId"
//                    value={editedData.chapterId || ''}
//                    onChange={handleChange}
//                  >
//                    {chapters.map((s) => (
//                      <option key={s.id} value={s.id}>
//                        {s.chapterName}
//                      </option>
//                    ))}
//                  </select>
//                ) : (
//                  <span>{getChapterName(rel.chapterId)}</span>
//                )}
//              </td>
//              <td>
//                {editingRowId === rel.id ? (
//                  <>
//                    <button onClick={handleSave}>💾</button>
//                    <button onClick={handleCancel}>❌</button>
//                  </>
//                ) : (
//                  <>
//                    <button onClick={() => handleDuplicate(rel.id)}>📄</button>
//                    <button onClick={() => handleDelete(rel.id)}>🗑️</button>
//                  </>
//                )}
//              </td>
//            </tr>
//          ))}
//        </tbody>
//      </table>
//    </div>
//  );
//}
