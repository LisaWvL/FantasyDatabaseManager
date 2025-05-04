/// <reference types="node" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';



export default defineConfig({
    esbuild: {
        jsx: 'automatic',
    },
    plugins: [react()],
    server: {
        port: 56507,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'http://localhost:5000',
                changeOrigin: true,
                secure: false
            }
        },
    },
});
