import React from 'react';
import './Analytics.sass';

const Analytics: React.FC = () => {
    return (
        <div className="analytics">
            <div className="sidebar">
                <div className="sidebar-header">
                    <div className="sidebar-title">
                        <h1 className="title">Аналитика</h1>
                        <h3 className="mini-title">Активная вкладка</h3>
                    </div>
                    <nav className="main-nav">
                        <a href="#" className="nav-item">Главная</a>
                        <a href="#" className="nav-item">Отчеты</a>
                        <a href="#" className="nav-item-active">События</a>
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
                    <span>Аналитик</span>
                    </div>
                </div>
                </div>
            </div>
            <main className='main'>
                <div className="main-header">
                    <p>Добро пожаловать</p>
                    <p>Анализируйте события и Угрозы</p>
                    <section className="stats-section">
                        <table className="stats-table">
                            <tbody>
                            <tr>
                                <td>Количество необработанных событий</td>
                                <td className="stats-value">50</td>
                            </tr>
                            </tbody>
                        </table>
                    </section>
                    <img src="" alt="" />
                </div>

                <hr className="section-divider" />

                <section className="events-section">
                    <p>События</p>
                    <div className="search-container">
                        <input 
                        type="text" 
                        placeholder='Поиск события'
                        className='search-input'
                        />
                        <img src="" alt="" />
                    </div>
                    <table className='data-table'>
                        <thead>
                            <tr>
                                <th>Имя</th>
                                <th>Сервер</th>
                                <th>Описание</th>
                                <th>Статус</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Вход</td>
                                <td>server.me</td>
                                <td>Вход (username) с десятой попытки</td>
                                <td>Новое</td>
                            </tr>
                            <tr>
                                <td>Вход</td>
                                <td>server.me</td>
                                <td>Вход (username) с десятой попытки</td>
                                <td>Новое</td>
                            </tr>
                            <tr>
                                <td>Вход</td>
                                <td>server.me</td>
                                <td>Вход (username) с десятой попытки</td>
                                <td>Новое</td>
                            </tr>
                        </tbody>
                    </table>

                    <hr className="section-divider" />

                    <section className='reports-section'>
                        <div className="section-header">
                            <p>Отчеты</p>
                            <div className="search-container">
                                <input 
                                type="text"
                                placeholder='Поиск отчетов'
                                className='search-input' 
                                />
                                <img src="" alt="" />
                            </div>
                        </div>
                        <div className="reports-grid">
                            <div className="report-card">
                                <p className='report-title'>Отчет 05.10.25</p>
                                <p className='report-view'>Посмотреть</p>
                            </div>
                            <div className="report-card">
                                <p className='report-title'>Отчет 07.10.25</p>
                                <p className='report-view'>Посмотреть</p>
                            </div>
                            <div className="report-card">
                                <p className='report-title'>Отчет 08.10.25</p>
                                <p className='report-view'>Посмотреть</p>
                            </div>
                            <div className="report-card">
                                <p className='report-title'>Отчет 11.10.25</p>
                                <p className='report-view'>Посмотреть</p>
                            </div>
                            <div className="report-card">
                                <p className='report-title'>Отчет 13.10.25</p>
                                <p className='report-view'>Посмотреть</p>
                            </div>
                        </div>
                    </section>
                </section>
            </main>
        </div>
    );
  };
  
  export default Analytics;