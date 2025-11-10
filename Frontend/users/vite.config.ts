import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import federation from '@originjs/vite-plugin-federation';

/* plugins: [
    react(),
    federation({
      name: 'rules',
      filename: 'remoteEntry.js',
      exposes: {
        './Rules': './src/components/rules/table/Rules.jsx',
      },
      remotes: {
        shell: 'http://localhost:5000/assets/remoteEntry.js',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ], */

export default defineConfig({
  plugins: [
    react(),
    federation({
      name: 'users',
      filename: 'remoteEntry.js',
      exposes: {
        './Users': './src/users/Users.tsx',
      },
      remotes: {
        shell: 'http://localhost:5000/assets/remoteEntry.js',
      },
      shared: ['react', 'react-dom', 'react-router-dom']
    })
  ],
});
