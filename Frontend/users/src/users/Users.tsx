import React, { useState } from "react";
import Cross from "../assets/icon/icon-cross.png";
import Pencil from "../assets/icon/icon-pencil.png";
import Plus from "../assets/icon/icon-plus.png";
import "./Users.sass";
import useUsers from "../hooks/UsersHook";
import UsersApiService from "../service/UsersApiServe";


export interface User {
  id: string;
  first_name: string;
  last_name: string;
  phone_number: string;
  photo_url: string;
  roles: "Аналитик" | "Руководитель" | "Эксперт";
  username: string;
  status: string;
  created_at: string;
  last_login: string;
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
  const { users, handleUsersChange } = useUsers();

  const [searchTerm, setSearchTerm] = useState("");
  const { isModalShow, showModal, hideModal } = useModal();
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    first_name: "",
    last_name: "",
    phone_number: "",
    photo_url: "",
    roles: "Аналитик" as "Аналитик" | "Руководитель" | "Эксперт",
    username: "",
    password: ""
  });

  const filteredUsers = users.filter(
    (user) =>
      user.first_name.toLowerCase().includes(searchTerm.toLowerCase()) 
   //   user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleDeleteUser = async (userId: string) => {
    if (window.confirm("Вы уверены, что хотите удалить пользователя?")) {
      try {
        await UsersApiService.delete(userId);
        handleUsersChange();
      } catch (error) {
        console.error("Ошибка при удалении пользователя:", error);
        alert("Не удалось удалить пользователя");
      }
    }
  };

  const handleSave = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // if (!formData.first_name || 
    //   !formData.last_name || 
    //   !formData.phone_number || 
    //   !formData.username) {
    //   alert("Все поля обязательны для заполнения");
    //   return;
    // }

    try {
      if (editingUser) {
        await UsersApiService.update(editingUser.id, formData);
      } else {
        await UsersApiService.create(formData);
      }

      handleUsersChange();

      hideModal();
      setEditingUser(null);
      setFormData({
        first_name: "",
        last_name: "",
        phone_number: "",
        photo_url: "",
        roles: "Аналитик",
        username: "",
        password: ""
      });
    } catch (error) {
      console.error("Ошибка при сохранении пользователя:", error);
      alert("Не удалось сохранить пользователя");
    }
  };

  const handleOpenCreate = () => {
    setEditingUser(null);
    setFormData({
      first_name: "",
      last_name: "",
      phone_number: "",
      photo_url: "",
      roles: "Эксперт",
      username: "",
      password: ""
    });
    showModal();
  };

  const handleOpenEdit = (user: User) => {
    setEditingUser(user);
    setFormData({
      first_name: user.first_name,
      last_name:  user.last_name,
      phone_number: user.phone_number,
      photo_url: user.photo_url,
      roles: user.roles,
      username: user.username,
      password: ""
    });
    showModal();
  };

  return (
    <div className="users-page">
      <div className="main-content">
        {/* <div className="content-header">
          <div className="content-header-left">
            <img src={LeftArrow} className="content-header-back" alt="Назад" />
            <h1 className="content-header-title">Пользователи</h1>
          </div>
        </div> */}

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
              onClick={handleOpenCreate}
              style={{ cursor: "pointer" }}
            />
          </div>

          <table className="users-table">
            <thead>
              <tr>
                <th>Имя</th>
                <th>Фамилия</th>
                <th>Телефон</th>
                <th>Статус</th>
                <th>Роли</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {filteredUsers.map((user) => (
                <tr key={user.id}>
                  <td>{user.first_name}</td>
                  <td>{user.last_name}</td>
                  <td>{user.phone_number}</td>
                  <td>{user.status}</td>
                  <td>
                    <span className={`role ${user.roles}`}>
                      {user.roles === "Аналитик" ? "Аналитик" : "Эксперт"}
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
          <label>Имя</label>
          <input
            type="text"
            value={formData.first_name}
            onChange={(e) => setFormData({ ...formData, first_name: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Фамилия</label>
          <input
           type="text"
           value={formData.last_name}
           onChange={(e) => setFormData({ ...formData, last_name: e.target.value })}
           required
          />
        </div>

        <div className="form-group">
          <label>Телефон</label>
          <input
            type="tel"
            value={formData.phone_number}
            onChange={(e) => setFormData({ ...formData, phone_number: e.target.value })}
            required
          />
        </div>

        {/* <div className="form-group">
          <label>Пароль</label>
          <input
            type="password"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
            required
          />
        </div> */}

        <div className="form-group">
          <label>Роль</label>
          <select
            value={formData.roles}
            onChange={(e) => setFormData({ ...formData, roles: e.target.value as "Аналитик" | "Эксперт" | "Руководитель" })}
          >
            <option value="Аналитик">Аналитик</option>
            <option value="Эксперт">Руководитель</option>
            <option value="Руководитель">Эксперт</option>
          </select>
        </div>
      </ModalForm>
    </div>
  );
};

export default Users;