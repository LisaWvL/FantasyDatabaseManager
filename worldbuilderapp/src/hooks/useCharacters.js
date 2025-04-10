import { useState, useEffect } from 'react';
import { fetchCharacters } from '../api/CharacterApi';

export function useCharacters() {
  const [characters, setCharacters] = useState([]);
  useEffect(() => {
    fetchCharacters().then(setCharacters);
  }, []);
  return characters;
}
