import React, { createContext, useContext, useState } from 'react';

const SnapshotContext = createContext();

export const SnapshotProvider = ({ children }) => {
    const [currentSnapshotId, setCurrentSnapshotId] = useState(null);
    const [snapshotName, setSnapshotName] = useState("No Snapshot Selected");

    return (
        <SnapshotContext.Provider value={{
            currentSnapshotId,
            setCurrentSnapshotId,
            snapshotName,
            setSnapshotName
        }}>
            {children}
        </SnapshotContext.Provider>
    );
};

export const useSnapshot = () => useContext(SnapshotContext);
