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
                target: 'https://localhost:5001',
                changeOrigin: true,
                secure: false,
            },
        },
    },
});
