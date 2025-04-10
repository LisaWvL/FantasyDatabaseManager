// CharacterCardGroupList.jsx
import React, { useEffect, useState } from 'react';
import { fetchCharacters } from '../api/CharacterApi';
import CharacterCard from './CharacterCard';

export default function CharacterCardGroupList({ onEdit, onDelete, onCreateChapter }) {
  const [characterGroups, setCharacterGroups] = useState([]);

  useEffect(() => {
    fetchCharacters().then((all) => {
      const grouped = all.reduce((acc, char) => {
        if (!acc[char.name]) acc[char.name] = [];
        acc[char.name].push(char);
        return acc;
      }, {});
      setCharacterGroups(Object.values(grouped));
    });
  }, []);

  return (
    <div className="character-card-group-list">
      {characterGroups.map((group, i) => (
        <CharacterCard
          key={i}
          chapters={group}
          onEdit={onEdit}
          onDelete={onDelete}
          onCreateChapter={onCreateChapter}
        />
      ))}
    </div>
  );
}
