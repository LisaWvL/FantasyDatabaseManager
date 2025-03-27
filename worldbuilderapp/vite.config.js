// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
//import { resolve } from 'path';

export default defineConfig({
    plugins: [react()],
    server: {
        port: 56507,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'https://localhost:5001', // ✅ Adjust if your backend uses a different port
                changeOrigin: true,
                secure: false
            }
        }
    },
    //resolve: {
    //    alias: {
    //        '@': resolve(__dirname, './src')
    //    }
    //},
    //define: {
    //    'process.env': {},
    //    __dirname: JSON.stringify(__dirname)
    //}
});
