// src/context/ChapterContext.jsx
import React, { createContext, useState } from 'react';

export const ChapterProvider = ({ children }) => {
  const [currentChapterId, setCurrentChapterId] = useState(null);
  const [snapshotName, setChapterName] = useState('No Chapter Selected');
  const ChapterContext = createContext();

  return (
    <ChapterContext.Provider
      value={{
        currentChapterId,
        setCurrentChapterId,
        snapshotName,
        setChapterName,
      }}
    >
      {children}
    </ChapterContext.Provider>
  );
};
