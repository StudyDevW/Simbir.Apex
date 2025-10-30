import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Cross from "../../assets/icon/icon-cross.png";
import Pencil from "../../assets/icon/icon-pencil.png";
import Plus from "../../assets/icon/icon-plus.png";
import LeftArrow from "../../assets/icon/icon-left.png"
import './Users.sass';

export interface User {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  password: string;
  status: 'online' | 'offline';
}

interface UsersPageProps {
  users: User[];
  onEditUser: (userId: number) => void;
  onCreateUser: () => void;
  onDeleteUser: (userId: number) => void;
}

const UsersPage: React.FC<UsersPageProps> = ({
  users,
  onEditUser,
  onCreateUser,
  onDeleteUser
}) => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');

  const filteredUsers = users.filter(user =>
    user.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleEditClick = (userId: number) => (event: React.MouseEvent<HTMLImageElement>) => {
    event.preventDefault();
    onEditUser(userId);
  };

  const handleDeleteClick = (userId: number) => (event: React.MouseEvent<HTMLImageElement>) => {
    event.preventDefault();
    onDeleteUser(userId);
  };

  const handleGoBack = () => {
    navigate(-1);
  };

  return (
    <div className="users-page">
      <div className="sidebar">
        <div className="sidebar-header">
          <div className="sidebar-title">
            <h1 className="title">Пользователи</h1>
            <p className='mini_title'>Активная вкладка</p>
          </div>
          <nav className="main-nav">
            <a href="#" className="nav-item">Главная</a>
            <a href="#" className="nav-item">Отчеты</a>
            <a href="#" className="nav-item-active">Пользователи</a>
          </nav>
        </div>

        <div className="sidebar-content">
          <div className="section">
            <h3>Основное</h3>
            <ul>
              <li><a href="#">Аналитика</a></li>
              <li><a href="#">Угрозы</a></li>
              <li><a href="#">Уведомления</a></li>
            </ul>
          </div>

          <div className="section">
            <h3>Дополнительно</h3>
            <ul>
              <li><a href="#">Настройки</a></li>
            </ul>
          </div>
        </div>

        <div className="sidebar-footer">
          <div className="user-info">
            <div className="user-avatar"></div>
            <div className="user-details">
              <strong>Иван (Desa1s13)</strong>
              <span>Руководитель</span>
            </div>
          </div>
        </div>
      </div>

      <div className="main-content">
        <div className="content-header">
          <div className="content-header-left">
            <img
              src={LeftArrow}
              className="content-header-back"
              alt="Назад"
              onClick={handleGoBack}
              style={{ cursor: 'pointer' }}
            />
            <h1 className='content-header-title'>Пользователи</h1>
          </div>
          <img
            src={Plus}
            className="content-header-plus"
            alt="Добавить"
            onClick={onCreateUser}
            style={{ cursor: 'pointer' }}
          />
        </div>

        <div className="users-table-container">
          <div className="search-container">
            <input
              type="text"
              placeholder="Поиск пользователей"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
          </div>

          <table className="users-table">
            <thead>
              <tr>
                <th>ФИО</th>
                <th>Email</th>
                <th>Телефон</th>
                <th>Пароль</th>
                <th>Статус</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {filteredUsers.map(user => (
                <tr key={user.id}>
                  <td>{user.fullName}</td>
                  <td>{user.email}</td>
                  <td>{user.phone}</td>
                  <td>{user.password}</td>
                  <td>
                    <span className={`status ${user.status}`}>
                      {user.status === 'online' ? 'В сети' : 'Не в сети'}
                    </span>
                  </td>
                  <td className="actions-cell">
                    <img
                      className='editing'
                      src={Pencil}
                      alt="Редактировать"
                      onClick={handleEditClick(user.id)}
                      style={{ cursor: 'pointer', marginRight: '10px' }}
                    />
                    <img
                      className='cross'
                      src={Cross}
                      alt="Удалить"
                      onClick={handleDeleteClick(user.id)}
                      style={{ cursor: 'pointer' }}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default UsersPage;