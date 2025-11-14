import React, { useState } from 'react';
import './Reports.sass';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import Cross from "../img/icon/icon-cross.png";
import Plus from "../img/icon/icon-plus.png";
import LeftArrow from '../img/icon/icon-left-up.png'
import useUsers from '../hook/ReportsHook';
import ReportsApiService from '../service/ReportsApiService';

export interface Reports {
    id: string;
    name: string;
    status: string;
    datatime: Date;
}

const Reports: React.FC = () => {
    const { users, handleUsersChange } = useUsers();
    const [searchTerm, setSearchTerm] = useState("");
    const navigate = useNavigate();

    const filteredUsers = users.filter(
        (user) =>
            user.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const formatDate = (date: Date): string => {
        return date.toLocaleDateString();
    };

    const handleDeleteUser = async (userId: string) => {
        if (window.confirm("Вы уверены, что хотите удалить пользователя?")) {
            try {
                await ReportsApiService.delete(userId);
                handleUsersChange();
            } catch (error) {
                console.error("Ошибка при удалении пользователя:", error);
                alert("Не удалось удалить пользователя");
            }
        }
    };

    return (
        <>
            <div className="reports-page">
                <div className="main-content">
                    <div className="content-header">
                        <div className="content-header-left">
                            <img src={LeftArrow} className="content-header-back" alt="Назад" />
                            <h1 className="content-header-title">Отчеты</h1>
                        </div>
                    </div>

                    <div className="reports-table-container">
                        <div className="search-container">
                            <input
                                type="text"
                                placeholder="Поиск пользователей"
                                className="search-input"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                            <img
                                src={Plus}
                                className="content-header-plus"
                                alt="Добавить"
                                style={{ cursor: "pointer" }}
                            />
                        </div>

                        <table className="reports-table">
                            <thead>
                                <tr>
                                    <th>Имя</th>
                                    <th>Статус</th>
                                    <th>Дата и время</th>
                                    <th>Действия</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filteredUsers.map((user) => (
                                    <tr key={user.id}>
                                        <td>{user.name}</td>
                                        <td>{user.status}</td>
                                        <td>{formatDate(user.datatime)}</td>
                                        <td className="actions-cell">
                                            {/* <img
                                                className="editing"
                                                src={Pencil}
                                                alt="Редактировать"
                                                onClick={() => handleOpenEdit?.(user)}
                                                style={{ cursor: "pointer", marginRight: "10px" }}
                                            /> */}
                                            <img
                                                className="cross"
                                                src={Cross}
                                                alt="Удалить"
                                                onClick={() => handleDeleteUser?.(user.id)}
                                                style={{ cursor: "pointer" }}
                                            />
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Reports;