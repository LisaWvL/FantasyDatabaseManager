// src/context/useSnapshot.js
import { useContext } from 'react';
import { SnapshotContext } from './SnapshotContext';

export const useSnapshot = () => useContext(SnapshotContext);
