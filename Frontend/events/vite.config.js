import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'events',
      filename: 'remoteEntry.js',
      exposes: {
        './Events': './src/components/events/table/Events.jsx',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ],
});
