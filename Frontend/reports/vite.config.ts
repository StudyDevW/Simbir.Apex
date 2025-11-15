import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
    plugins: [
        react(),
        federation({
            name: 'reports',
            filename: 'remoteEntry.js',
            exposes: {
                './Reports': './src/assets/reports/Reports.tsx',
            },
            remotes: {
                shell: 'http://localhost:5000/assets/remoteEntry.js',
            },
            shared: ['react', 'react-dom', 'react-router-dom']
        })
    ],
});