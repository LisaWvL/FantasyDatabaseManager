import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';
import { SnapshotProvider } from './context/snapshotContext';

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <SnapshotProvider>
            <App />
        </SnapshotProvider>
    </React.StrictMode>
);
