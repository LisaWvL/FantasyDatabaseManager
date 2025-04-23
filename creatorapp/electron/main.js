// main.js (Electron entry)
import { app, BrowserWindow } from 'electron';
import { join } from 'path';

function createWindow() {
    const win = new BrowserWindow({
        width: 1400,
        height: 900,
        webPreferences: {
            preload: join(__dirname, 'preload.js')
        }
    });

    win.loadURL('http://localhost:56507'); // served by Vite dev
}

app.whenReady().then(() => {
    createWindow();
    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) createWindow();
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit();
});
