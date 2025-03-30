// src/main.jsx
import React, { useEffect } from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';
import { SnapshotProvider } from './context/snapshotContext';

// 🔌 Hook into browser close event to notify backend
function AppWithShutdown() {
    useEffect(() => {
        const shutdownAll = async () => {
            try {
                await fetch('http://localhost:8000/shutdown', {
                    method: 'POST',
                });
                console.log('🛑 Sent shutdown signal to backend.');
            } catch (e) {
                console.warn('Failed to shut down backend:', e);
            }
        };

        window.addEventListener('beforeunload', shutdownAll);
        return () => window.removeEventListener('beforeunload', shutdownAll);
    }, []);

    return (
        <SnapshotProvider>
            <App />
        </SnapshotProvider>
    );
}

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <AppWithShutdown />
    </React.StrictMode>
);
