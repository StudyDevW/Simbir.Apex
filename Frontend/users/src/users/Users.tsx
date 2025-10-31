import React, { useState } from "react";
import Cross from "../assets/icon/icon-cross.png";
import Pencil from "../assets/icon/icon-pencil.png";
import Plus from "../assets/icon/icon-plus.png";
import LeftArrow from "../assets/icon/icon-left.png";
import "./Users.sass";

export interface User {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  password: string;
  status: "online" | "offline";
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
  const [users, setUsers] = useState<User[]>([
    {
      id: 1,
      fullName: "Еремеев Ф.Б.",
      email: "eremeev531@gmail.com",
      phone: "+7(962)-876-80-87",
      password: "qwert123",
      status: "online",
    },
    {
      id: 2,
      fullName: "Марков А.М.",
      email: "markovA01@gmail.com",
      phone: "+7(937)-823-84-32",
      password: "helmik789",
      status: "offline",
    },
    {
      id: 3,
      fullName: "Орлеев К.С.",
      email: "orleevKKK@gmail.com",
      phone: "+7(905)-421-92-92",
      password: "gamalion456",
      status: "online",
    },
  ]);

  const [searchTerm, setSearchTerm] = useState("");
  const { isModalShow, showModal, hideModal } = useModal();
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    phone: "",
    password: "",
    status: "online" as "online" | "offline",
  });

  const filteredUsers = users.filter(
    (user) =>
      user.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleDeleteUser = (userId: number) => {
    if (window.confirm("Вы уверены, что хотите удалить пользователя?")) {
      setUsers((prev) => prev.filter((u) => u.id !== userId));
    }
  };

  const handleSave = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!formData.fullName || !formData.email || !formData.phone || !formData.password) {
      alert("Все поля обязательны для заполнения");
      return;
    }

    if (editingUser) {
      setUsers((prev) =>
        prev.map((u) => (u.id === editingUser.id ? { ...editingUser, ...formData } : u))
      );
    } else {
      const newUser: User = {
        id: users.length > 0 ? Math.max(...users.map((u) => u.id)) + 1 : 1,
        ...formData,
      };
      setUsers((prev) => [...prev, newUser]);
    }

    hideModal();
    setEditingUser(null);
    setFormData({
      fullName: "",
      email: "",
      phone: "",
      password: "",
      status: "online",
    });
  };

  const handleOpenCreate = () => {
    setEditingUser(null);
    setFormData({
      fullName: "",
      email: "",
      phone: "",
      password: "",
      status: "online",
    });
    showModal();
  };

  const handleOpenEdit = (user: User) => {
    setEditingUser(user);
    setFormData({
      fullName: user.fullName,
      email: user.email,
      phone: user.phone,
      password: user.password,
      status: user.status,
    });
    showModal();
  };

  return (
    <div className="users-page">
      <div className="main-content">
        <div className="content-header">
          <div className="content-header-left">
            <img src={LeftArrow} className="content-header-back" alt="Назад" />
            <h1 className="content-header-title">Пользователи</h1>
            <img
              src={Plus}
              className="content-header-plus"
              alt="Добавить"
              onClick={handleOpenCreate}
              style={{ cursor: "pointer" }}
            />
          </div>
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
              {filteredUsers.map((user) => (
                <tr key={user.id}>
                  <td>{user.fullName}</td>
                  <td>{user.email}</td>
                  <td>{user.phone}</td>
                  <td>{user.password}</td>
                  <td>
                    <span className={`status ${user.status}`}>
                      {user.status === "online" ? "В сети" : "Не в сети"}
                    </span>
                  </td>
                  <td className="actions-cell">
                    <img
                      className="editing"
                      src={Pencil}
                      alt="Редактировать"
                      onClick={() => handleOpenEdit(user)}
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
        onClose={() => {
          hideModal();
          setEditingUser(null);
        }}
        onSubmit={handleSave}
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
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value as "online" | "offline" })}
          >
            <option value="online">Аналитик</option>
            <option value="offline">Руководитель</option>
            <option value="online">Эксперт</option>
          </select>
        </div>
      </ModalForm>
    </div>
  );
};

export default Users;