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
import AnalyticsPage from './pages/AnalyticsPage.jsx';
import ProfilePage from './pages/ProfilePage.jsx';
import ProtectedRoute from './components/ProtectedRoute.jsx';

const routes = [
  {
    index: true,
    path: '/',
    element: <Homepage />,
  },
  {
    path: '/events',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <EventsPage />
    //   </ProtectedRoute>
    // ),
    element: <EventsPage />
  },
  {
    path: '/users',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <UsersPage />
    //   </ProtectedRoute>
    // ),
    element: <UsersPage />,
  },
  {
    path: '/rules',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <RulesPage />
    //   </ProtectedRoute>
    // ),
    element: <RulesPage />,
  },
  {
    path: '/alerts',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <AlertsPage />
    //   </ProtectedRoute>
    // ),
    element: <AlertsPage />,
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/analytics',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <AnalyticsPage />
    //   </ProtectedRoute>
    // ),
    element: <AnalyticsPage />,
  },
  {
    path: '/profile',
    //TODO: заменить на:
    // element: (
    //   <ProtectedRoute>
    //     <EventsPage />
    //   </ProtectedRoute>
    // ),
    element: <ProfilePage />,
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
