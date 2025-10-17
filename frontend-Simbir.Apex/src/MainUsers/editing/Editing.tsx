import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import type { User } from '../users/Users';
import LeftArrow from "../../assets/icon/icon-left.png";
import './Editing.sass';

interface EditUserPageProps {
  users: User[];
  onSaveUser: (userData: User) => void;
}

const EditUserPage: React.FC<EditUserPageProps> = ({ users, onSaveUser }) => {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    phone: '',
    password: '',
    status: 'online' as 'online' | 'offline'
  });

  useEffect(() => {
    if (userId) {
      const foundUser = users.find(u => u.id === parseInt(userId));
      if (foundUser) {
        setUser(foundUser);
        setFormData({
          fullName: foundUser.fullName,
          email: foundUser.email,
          phone: foundUser.phone,
          password: foundUser.password,
          status: foundUser.status
        });
      }
    }
  }, [userId, users]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (user) {
      const updatedUser: User = {
        ...user,
        ...formData
      };
      onSaveUser(updatedUser);
      navigate('/users');
    }
  };

  const handleGoBack = () => {
    navigate('/users');
  };

  const handleChange = (field: keyof typeof formData, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  if (!user) {
    return <div>Пользователь не найден</div>;
  }

  return (
    <div className="users-page">
      <div className="sidebar">
        <div className="sidebar-header">
          <div className="sidebar-title">
            <h1 className="title">Пользователи</h1>
            <h3 className="mini-title">Активная вкладка</h3>
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
              <li><a href="#">Руководство</a></li>
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
            <h1 className='content-header-title'>Редактирование пользователя</h1>
          </div>
        </div>

        <div className="edit-user-form">
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label  htmlFor="fullName">ФИО</label>
              <br />
              <input
                type="text"
                id="fullName"
                value={formData.fullName}
                onChange={(e) => handleChange('fullName', e.target.value)}
                required
                className='input'
              />
            </div>

            <div className="form-group">
              <label className="Email" htmlFor="email">Email</label>
              <br />
              <input
                type="email"
                id="email"
                value={formData.email}
                onChange={(e) => handleChange('email', e.target.value)}
                required
                className='input'
              />
            </div>

            <div className="form-group">
              <label htmlFor="phone">Телефон</label>
              <br />
              <input
                type="tel"
                id="phone"
                value={formData.phone}
                onChange={(e) => handleChange('phone', e.target.value)}
                required
                className='input'
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Пароль</label>
              <br />
              <input
                type="password"
                id="password"
                value={formData.password}
                onChange={(e) => handleChange('password', e.target.value)}
                required
                className='input'
              />
            </div>

            <div className="form-group">
              <label htmlFor="status">Роль</label>
              <br />
              <select
                id="status"
                value={formData.status}
                onChange={(e) => handleChange('status', e.target.value)}
                className='input'
              >
                <option value="analytics">Аналитик</option>
                <option value="supervisor">Руководитель</option>
                <option value="Expert">Эксперт</option>

              </select>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-primary">
                Сохранить изменения
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditUserPage;