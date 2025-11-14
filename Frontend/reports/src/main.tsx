import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider, createBrowserRouter, type RouteObject } from 'react-router-dom';
import './index.sass';
import App from './App';
// import Events from '../../events/src/components/events/table/Events.jsx'
import Reports from './reports/Reports';


const routes: RouteObject[] = [
    {
        index: true,
        element: <Reports />,
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
