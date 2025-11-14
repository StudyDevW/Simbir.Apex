import React from 'react';
import './Analytics.sass';
import Grafic from "../../img/icon/icon-grafic.png"
import Glass from "../../img/icon/icon-glass.png"
import { useNavigate } from 'react-router-dom';

export interface Analytics {
    description: string;
}

const Analytics: React.FC = () => {
    const navigate = useNavigate();

    const handleReportsClick = () => {
        navigate('/reports');
    };

    const handleEventsClick = () => {
        navigate('/events');
    };
    return (
        <>
            <div className="analytics">
                <main className='main'>
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
                            <p
                                className='events-section-title'
                                onClick={handleEventsClick}
                                style={{ cursor: 'pointer' }}
                            >
                                События
                            </p>
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
                                <p
                                    className='section-header-title'
                                    onClick={handleReportsClick}
                                    style={{ cursor: 'pointer' }}
                                >
                                    Отчеты
                                </p>
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