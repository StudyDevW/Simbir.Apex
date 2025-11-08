import React, { useState } from "react";
import Cross from "../assets/icon/icon-cross.png";
import Pencil from "../assets/icon/icon-pencil.png";
import Plus from "../assets/icon/icon-plus.png";
import "./Users.sass";
import useUsers from "../hooks/UsersHook";
import usePagination from 'shell/pagination/usePagination';
import UsersApiService from "../service/UsersApiServe";

export interface User {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  password: string;
  role: "Аналитик" | "Руководитель" | "Эксперт";
}

const useModal = () => {
  const [isModalShow, setModalShow] = useState(false);
  const showModal = () => setModalShow(true);
  const hideModal = () => setModalShow(false);
  return { isModalShow, showModal, hideModal };
};

interface ModalFormProps {
  show: boolean;
  title: string;
  onSubmit: (e: React.FormEvent<HTMLFormElement>) => void;
  onClose: () => void;
  children: React.ReactNode;
}

const ModalForm: React.FC<ModalFormProps> = ({ show, title, onSubmit, onClose, children }) => {
  if (!show) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-window">
        <div className="modal-header">
          <h2>{title}</h2>
          <button className="modal-close" onClick={onClose}>
            ✕
          </button>
        </div>
        <form onSubmit={onSubmit} className="modal-body">
          {children}
          <div className="modal-footer">
            <button type="submit" className="btn-primary">Сохранить</button>
            <button type="button" className="btn-secondary" onClick={onClose}>
              Отмена
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

const Users: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const { isModalShow, showModal, hideModal } = useModal();
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    phone: "",
    password: "",
    role: "Аналитик" as "Аналитик" | "Руководитель" | "Эксперт",
  });

  const { currentPage } = usePagination();
  const { users, handleUsersChange } = useUsers(currentPage);

  const filteredUsers = users.filter(
    (user) =>
      user.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleDeleteUser = async (userId: number) => {
    if (window.confirm("Вы уверены, что хотите удалить пользователя?")) {
      try {
        await UsersApiService.delete(userId.toString());
        handleUsersChange();
      } catch (error) {
        console.error("Ошибка при удалении пользователя:", error);
        alert("Не удалось удалить пользователя");
      }
    }
  };

  const handleSaveUser = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!formData.fullName || !formData.email || !formData.phone || !formData.password) {
      alert("Все поля обязательны для заполнения");
      return;
    }

    try {
      if (editingUser) {
        await UsersApiService.update(editingUser.id.toString(), formData);
      } else {
        await UsersApiService.create(formData);
      }

      handleUsersChange();
      hideModal();
      setEditingUser(null);
      setFormData({
        fullName: "",
        email: "",
        phone: "",
        password: "",
        role: "Аналитик",
      });
    } catch (error) {
      console.error("Ошибка при сохранении пользователя:", error);
      alert("Не удалось сохранить пользователя");
    }
  };

  const handleCreateUser = () => {
    setEditingUser(null);
    setFormData({
      fullName: "",
      email: "",
      phone: "",
      password: "",
      role: "Аналитик",
    });
    showModal();
  };

  const handleEditUser = (user: User) => {
    setEditingUser(user);
    setFormData({
      fullName: user.fullName,
      email: user.email,
      phone: user.phone,
      password: user.password,
      role: user.role,
    });
    showModal();
  };

  const handleCloseModal = () => {
    hideModal();
    setEditingUser(null);
    setFormData({
      fullName: "",
      email: "",
      phone: "",
      password: "",
      role: "Аналитик",
    });
  };

  return (
    <div className="users-page">
      <div className="main-content">
        <div className="users-table-container">
          <div className="search-container">
            <input
              type="text"
              placeholder="Поиск пользователей"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
            <img
              src={Plus}
              className="content-header-plus"
              alt="Добавить"
              onClick={handleCreateUser}
              style={{ cursor: "pointer" }}
            />
          </div>

          <table className="users-table">
            <thead>
              <tr>
                <th>ФИО</th>
                <th>Email</th>
                <th>Телефон</th>
                <th>Пароль</th>
                <th>Роли</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {filteredUsers.map((user) => (
                <tr key={user.id}>
                  <td>{user.fullName}</td>
                  <td>{user.email}</td>
                  <td>{user.phone}</td>
                  <td>{user.password}</td>
                  <td>
                    <span className={`role ${user.role}`}>
                      {user.role === "Аналитик" ? "Аналитик" : user.role === "Руководитель" ? "Руководитель" : "Эксперт"}
                    </span>
                  </td>
                  <td className="actions-cell">
                    <img
                      className="editing"
                      src={Pencil}
                      alt="Редактировать"
                      onClick={() => handleEditUser(user)}
                      style={{ cursor: "pointer", marginRight: "10px" }}
                    />
                    <img
                      className="cross"
                      src={Cross}
                      alt="Удалить"
                      onClick={() => handleDeleteUser(user.id)}
                      style={{ cursor: "pointer" }}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <ModalForm
        show={isModalShow}
        title={editingUser ? "Редактирование пользователя" : "Создание пользователя"}
        onClose={handleCloseModal}
        onSubmit={handleSaveUser}
      >
        <div className="form-group">
          <label>ФИО</label>
          <input
            type="text"
            value={formData.fullName}
            onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Email</label>
          <input
            type="email"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Телефон</label>
          <input
            type="tel"
            value={formData.phone}
            onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Пароль</label>
          <input
            type="password"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Роль</label>
          <select
            value={formData.role}
            onChange={(e) => setFormData({ ...formData, role: e.target.value as "Аналитик" | "Эксперт" | "Руководитель" })}
          >
            <option value="Аналитик">Аналитик</option>
            <option value="Руководитель">Руководитель</option>
            <option value="Эксперт">Эксперт</option>
          </select>
        </div>
      </ModalForm>
    </div>
  );
};

export default Users;