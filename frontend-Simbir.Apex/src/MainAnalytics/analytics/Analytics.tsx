import React, { useState } from 'react';
import './Analytics.sass';
import Grafic from "../../assets/icon/icon-grafic.png"
import Glass from "../../assets/icon/icon-glass.png"

const Analytics: React.FC = () => {
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

    const closeMobileMenu = () => {
        setIsMobileMenuOpen(false);
    };

    return (
        <>
            <button
                className="mobile-menu-btn"
                onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
            >
                ☰
            </button>

            <div
                className={`mobile-overlay ${isMobileMenuOpen ? 'active' : ''}`}
                onClick={closeMobileMenu}
            />

            <div className="analytics">
                <div className={`sidebar ${isMobileMenuOpen ? 'mobile-open' : ''}`}>
                    <div className="sidebar-header">
                        <div className="sidebar-title">
                            <h1 className="title">Аналитика</h1>
                            <p className='mini_title'>Активная вкладка</p>
                        </div>
                        <nav className="main-nav">
                            <a href="#" className="nav-item" onClick={closeMobileMenu}>Главная</a>
                            <a href="#" className="nav-item" onClick={closeMobileMenu}>Отчеты</a>
                            <a href="#" className="nav-item-active" onClick={closeMobileMenu}>События</a>
                        </nav>
                    </div>
                    <div className="sidebar-content">
                        <div className="section">
                            <h3>Основное</h3>
                            <ul>
                                <li><a href="#" onClick={closeMobileMenu}>Аналитика</a></li>
                                <li><a href="#" onClick={closeMobileMenu}>Угрозы</a></li>
                                <li><a href="#" onClick={closeMobileMenu}>Уведомления</a></li>
                            </ul>
                        </div>
                        <div className="section">
                            <h3>Дополнительно</h3>
                            <ul>
                                <li><a href="#" onClick={closeMobileMenu}>Настройки</a></li>
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

                <main className='main' onClick={() => isMobileMenuOpen && closeMobileMenu()}>
                    <div className="main-header">
                        <div className="main-header-content">
                            <div className="text-content">
                                <p className='main-header-title'>Добро пожаловать в раздел аналитики</p>
                                <p className='main-header-text'>Анализируйте события и угрозы</p>
                            </div>
                            <img className="main-header-img" src={Grafic} alt="" />
                        </div>
                        <section className="stats-section">
                            <table className="stats-table">
                                <tbody>
                                    <tr>
                                        <td className='status-text'>Количество необработанных событий</td>
                                        <td className="stats-value">50</td>
                                    </tr>
                                </tbody>
                            </table>
                        </section>
                    </div>

                    <section className="events-section">
                        <div className="events-section-header">
                            <p className='events-section-title'>События</p>
                        </div>
                        <div className="search-container">
                            <input
                                type="text"
                                placeholder='Поиск событий'
                                className='search-input'
                            />
                            <img className='search-img' src={Glass} alt="" />
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

                        <section className='reports-section'>
                            <div className="section-header">
                                <p className='section-header-title'>Отчеты</p>
                                <div className="search-container">
                                    <input
                                        type="text"
                                        placeholder='Поиск отчетов'
                                        className='search-input'
                                    />
                                    <img className='search-img' src={Glass} alt="" />
                                </div>
                            </div>
                            <div className="reports-grid">
                                <div className="report-card">
                                    <p className='report-title'>Отчет</p>
                                    <p className="report-mini-title">05.10.25</p>
                                    <button className='view-btn'>Посмотреть</button>
                                </div>
                                <div className="report-card">
                                    <p className='report-title'>Отчет</p>
                                    <p className="report-mini-title">07.10.25</p>
                                    <button className='view-btn'>Посмотреть</button>
                                </div>
                                <div className="report-card">
                                    <p className='report-title'>Отчет</p>
                                    <p className="report-mini-title">09.10.25</p>
                                    <button className='view-btn'>Посмотреть</button>
                                </div>
                                <div className="report-card">
                                    <p className='report-title'>Отчет</p>
                                    <p className="report-mini-title">09.10.25</p>
                                    <button className='view-btn'>Посмотреть</button>
                                </div>
                                <div className="report-card">
                                    <p className='report-title'>Отчет</p>
                                    <p className="report-mini-title">13.10.25</p>
                                    <button className='view-btn'>Посмотреть</button>
                                </div>
                            </div>
                        </section>
                    </section>
                </main>
            </div>
        </>
    );
};

export default Analytics;