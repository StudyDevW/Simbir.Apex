import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { User } from '../users/Users';
import LeftArrow from "../assets/icon/icon-left-up.png";
import './Create.sass';

interface CreateUserPageProps {
  onCreateUser: (userData: Omit<User, 'id'>) => void;
}

const CreateUserPage: React.FC<CreateUserPageProps> = ({ onCreateUser }) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    phone: '',
    password: '',
    status: 'online' as 'online' | 'offline'
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [touched, setTouched] = useState({
    fullName: false,
    email: false,
    phone: false,
    password: false
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);

    await new Promise(resolve => setTimeout(resolve, 500));

    onCreateUser(formData);
    setIsSubmitting(false);
    navigate('/users');
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

  const handleBlur = (field: keyof typeof touched) => {
    setTouched(prev => ({
      ...prev,
      [field]: true
    }));
  };

  const isFormValid = formData.fullName.trim() &&
    formData.email.trim() &&
    formData.phone.trim() &&
    formData.password.length >= 6;

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
            <div className="user-avatar">И</div>
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
            />
            <h1 className='content-header-title'>Создание пользователя</h1>
          </div>
        </div>

        <div className="create-user-form">
          <form onSubmit={handleSubmit} noValidate>
            <div className="form-group">
              <label htmlFor="fullName">ФИО</label>
              <input
                type="text"
                id="fullName"
                value={formData.fullName}
                onChange={(e) => handleChange('fullName', e.target.value)}
                onBlur={() => handleBlur('fullName')}
                className='form-group-input'
                required
                placeholder="Введите ФИО пользователя"
                minLength={2}
                maxLength={100}
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="email">Email</label>
              <input
                type="email"
                id="email"
                value={formData.email}
                onChange={(e) => handleChange('email', e.target.value)}
                onBlur={() => handleBlur('email')}
                className='form-group-input'
                required
                placeholder="Введите email"
                pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="phone">Телефон</label>
              <input
                type="tel"
                id="phone"
                value={formData.phone}
                onChange={(e) => handleChange('phone', e.target.value)}
                onBlur={() => handleBlur('phone')}
                className='form-group-input'
                required
                placeholder="Введите телефон"
                pattern="[\+]?[0-9\s\-\(\)]+"
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Пароль</label>
              <input
                type="password"
                id="password"
                value={formData.password}
                onChange={(e) => handleChange('password', e.target.value)}
                onBlur={() => handleBlur('password')}
                className='form-group-input'
                required
                placeholder="Введите пароль"
                minLength={6}
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="status">Роль</label>
              <select
                id="status"
                value={formData.status}
                onChange={(e) => handleChange('status', e.target.value)}
                className='form-group-status-input'
                disabled={isSubmitting}
              >
                <option value="online">Аналитик</option>
                <option value="offline">Руководитель</option>
                <option value="online">Эксперт</option>
              </select>
            </div>

            <div className="form-actions">
              <button
                type="submit"
                className="btn-primary"
                disabled={isSubmitting || !isFormValid}
              >
                {isSubmitting ? (
                  <>
                    <span className="loading-spinner"></span>
                    Создание...
                  </>
                ) : (
                  'Создать пользователя'
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CreateUserPage;