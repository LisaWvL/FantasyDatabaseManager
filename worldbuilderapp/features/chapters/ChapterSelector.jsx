//TODO
//ChapterSelector.jsx
// Enable timeline grouping(show Book > Act > Chapter visually)
// Add search and sort capabilities
// Add �Create New Chapter� button
// Add �Delete Chapter� button
// Add �Duplicate Chapter� button
// Add �Edit Chapter� button
// Add �Show All Related� button
// Add �Copy to Chapter� button
// Add �Pin� button
// Add �Show PlotPoints for Current Chapter Only� toggle

// src/components/ChapterSelector.jsx
import React, { useEffect, useState } from 'react';
import { fetchChapters } from './ChapterApi';

const ChapterSelector = ({ value, onChange }) => {
  const [chapters, setChapters] = useState([]);

  useEffect(() => {
    const loadChapters = async () => {
      try {
        const data = await fetchChapters();
        setChapters(data);
      } catch (error) {
        console.error('Failed to fetch chapters:', error);
      }
    };
    loadChapters();
  }, []);

  return (
    <div className="chapter-selector">
      <label>Select Chapter:</label>
      <select onChange={(e) => onChange(parseInt(e.target.value))} value={value || ''}>
        <option value="">-- Choose Chapter --</option>
        {chapters.map((s) => (
          <option key={s.id} value={s.id}>
            {s.chapterName}
          </option>
        ))}
      </select>
    </div>
  );
};

export default ChapterSelector;
