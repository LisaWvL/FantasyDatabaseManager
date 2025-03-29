import react from '@vitejs/plugin-react';
import path from 'path';
import { fileURLToPath } from 'url';
import { defineConfig } from 'vite';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default defineConfig({
    plugins: [react()],
    resolve: {
        //alias: {
        //    '@': path.resolve(__dirname, './src'),

        //    '@CalendarApi': path.resolve(__dirname, './api/CalendarApi.js'),
        //    '@CharacterApi': path.resolve(__dirname, './api/CharacterApi.js'),
        //    '@CharacterRelationshipApi': path.resolve(__dirname, './api/CharacterRelationshipApi.js'),
        //    '@DropdownApi': path.resolve(__dirname, './api/DropdownApi.js'),
        //    '@EraApi': path.resolve(__dirname, './api/EraApi.js'),
        //    '@EventApi': path.resolve(__dirname, './api/EventApi.js'),
        //    '@FactionApi': path.resolve(__dirname, './api/FactionApi.js'),
        //    '@ItemApi': path.resolve(__dirname, './api/ItemApi.js'),
        //    '@LanguageApi': path.resolve(__dirname, './api/LanguageApi.js'),
        //    '@LocationApi': path.resolve(__dirname, './api/LocationApi.js'),
        //    '@PlotPointApi': path.resolve(__dirname, './api/PlotPointApi.js'),
        //    '@PriceExampleApi': path.resolve(__dirname, './api/PriceExampleApi.js'),
        //    '@RiverApi': path.resolve(__dirname, './api/RiverApi.js'),
        //    '@RouteApi': path.resolve(__dirname, './api/RouteApi.js'),
        //    '@SnapshotApi': path.resolve(__dirname, './api/SnapshotApi.js')
        //}
    },
    server: {
        port: 56507,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'https://localhost:5001',
                changeOrigin: true,
                secure: false
            }
        }
    }
});
