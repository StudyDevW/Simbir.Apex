import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'alerts',
      filename: 'remoteEntry.js',
      exposes: {
        './Alerts': './src/components/alerts/table/Alerts.jsx',
      },
      remotes: {
        shell: 'http://localhost:5000/assets/remoteEntry.js',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ],
});
