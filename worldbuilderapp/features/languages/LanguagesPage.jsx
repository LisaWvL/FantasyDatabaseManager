// src/pages/LanguagePage.jsx
import React, { useEffect, useState } from 'react';
import { fetchLanguages, deleteLanguage } from './LanguageApi';
import EntityTable from '../entities/EntityTable';
import { useNavigate } from 'react-router-dom';

export default function LanguagePage() {
  const [languages, setLanguages] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    loadLanguages();
  }, []);

  const loadLanguages = async () => {
    try {
      const data = await fetchLanguages();
      setLanguages(data);
    } catch (err) {
      console.error('Failed to load languages:', err);
    }
  };

  const handleEdit = (id) => navigate(`/language/edit/${id}`);
  //const handleChapter = (id) => navigate(`/language/${id}/new-chapter-page`);
  const handleDelete = async (id) => {
    if (confirm('Delete this language?')) {
      await deleteLanguage(id);
      loadLanguages();
    }
  };

  return (
    <div className="container">
      <h2 className="mb-3">Languages</h2>
      <EntityTable
        data={languages}
        onEdit={handleEdit}
        onDelete={handleDelete}
        /*  onChapter={handleChapter}*/
      />
    </div>
  );
}
