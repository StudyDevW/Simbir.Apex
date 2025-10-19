import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'shell',
      remotes: {
        events: 'http://localhost:5001/assets/remoteEntry.js',
        users: 'http://localhost:5002/assets/remoteEntry.js',
        rules: 'http://localhost:5003/assets/remoteEntry.js',
      },
      shared: ['react','react-dom','react-router-dom']
    })
  ],
});
