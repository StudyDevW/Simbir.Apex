import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'rules',
      filename: 'remoteEntry.js',
      exposes: {
        './Rules': './src/components/rules/table/Rules.jsx',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ],
});