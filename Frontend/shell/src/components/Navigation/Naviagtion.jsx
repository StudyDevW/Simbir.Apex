import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Navigation.css';

const Navigation = () => {
  const location = useLocation();

  const mainTabs = [
    { name: 'Главная', path: '/' },
    { name: 'События', path: '/events' },
    { name: 'Угрозы', path: '/alerts' },
    { name: 'Шаблоны', path: '/rules' },
    { name: 'Пользователи', path: '/users' },
    { name: 'Аналитика', path: '/analytics' },
  ];

  const additionalTabs = [
    { name: 'Настройки', path: '/settings' },
  ];

  return (
    <div className="navigation rounded">
      <div className="nav-group">
        <div className="nav-group-title">Основное</div>
        {mainTabs.map((tab) => (
          <Link
            key={tab.name}
            to={tab.path}
            className={`nav-item ${location.pathname === tab.path ? 'active' : ''}`}
          >
            {tab.name}
            {tab.name === 'Уведомления' && (
              <span className="badge bg-danger">10</span>
            )}
          </Link>
        ))}
      </div>

      <div className="nav-group">
        <div className="nav-group-title">Дополнительно</div>
        {additionalTabs.map((tab) => (
          <Link
            key={tab.name}
            to={tab.path}
            className={`nav-item ${location.pathname === tab.path ? 'active' : ''}`}
          >
            {tab.name}
          </Link>
        ))}
      </div>
    </div>
  );
};

export default Navigation;