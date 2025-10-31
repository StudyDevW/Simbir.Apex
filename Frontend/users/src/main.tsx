import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider, createBrowserRouter, type RouteObject } from 'react-router-dom';
import './index.sass';
import App from './App';
import Users from './users/Users';


const routes: RouteObject[] = [
  {
    index: true,
    element: <Users />,
  }
];

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: routes,
  },
]);

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);