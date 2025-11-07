import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'users',
      filename: 'remoteEntry.js',
      exposes: {
        './Users': './src/users/Users.tsx',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ],
});
