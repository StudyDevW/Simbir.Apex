import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css'
import App from './App.jsx'
import ReactDOM from 'react-dom/client'
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import React from 'react';
import ErrorPage from './pages/ErrorPage';
import Homepage from './pages/Homepage';
import EventsPage from './pages/EventsPage';
import UsersPage from './pages/UsersPage';
import RulesPage from './pages/RulesPage.jsx';
import AlertsPage from './pages/AlertsPage.jsx';
import LoginPage from './pages/LoginPage.jsx';

const routes = [
  {
    index: true,
    path: '/',
    element: <Homepage />,
  },
  {
    path: '/events',
    element: <EventsPage />,
  },
  {
    path: '/users',
    element: <UsersPage />,
  },
  {
    path: '/rules',
    element: <RulesPage />,
  },
  {
    path: '/alerts',
    element: <AlertsPage />,
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
];

const router = createBrowserRouter([
  {
    path: '/',
    element: <App routes={routes} />,
    children: routes,
    errorElement: <ErrorPage />,
  },
]);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
