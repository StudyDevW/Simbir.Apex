import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
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
  const [showModal, setShowModal] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);

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

  const handleSave = (data: Omit<User, "id">) => {
    if (editingUser) {
      setUsers((prev) =>
        prev.map((u) => (u.id === editingUser.id ? { ...editingUser, ...data } : u))
      );
    } else {
      const newUser: User = {
        id: users.length > 0 ? Math.max(...users.map((u) => u.id)) + 1 : 1,
        ...data,
      };
      setUsers((prev) => [...prev, newUser]);
    }
    setShowModal(false);
    setEditingUser(null);
  };

  const handleOpenCreate = () => {
    setEditingUser(null);
    setShowModal(true);
  };

  const handleOpenEdit = (user: User) => {
    setEditingUser(user);
    setShowModal(true);
  };

  const isMobile = window.innerWidth <= 767;

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
            {!isMobile && (
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
            )}
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

      <UserModal
        show={showModal}
        onHide={() => {
          setShowModal(false);
          setEditingUser(null);
        }}
        title={editingUser ? "Редактирование пользователя" : "Создание пользователя"}
        initialData={editingUser || undefined}
        onSubmit={handleSave}
      />
    </div>
  );
};

interface UserModalProps {
  show: boolean;
  onHide: () => void;
  title: string;
  initialData?: Partial<User>;
  onSubmit: (data: Omit<User, "id">) => void;
}

const UserModal: React.FC<UserModalProps> = ({
  show,
  onHide,
  title,
  initialData = {},
  onSubmit,
}) => {
  const [formData, setFormData] = useState({
    fullName: initialData.fullName || "",
    email: initialData.email || "",
    phone: initialData.phone || "",
    password: initialData.password || "",
    status: (initialData.status as "online" | "offline") || "online",
  });

  useEffect(() => {
    setFormData({
      fullName: initialData.fullName || "",
      email: initialData.email || "",
      phone: initialData.phone || "",
      password: initialData.password || "",
      status: (initialData.status as "online" | "offline") || "online",
    });
  }, [initialData]);

  const handleChange = (field: keyof typeof formData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>

      <Form onSubmit={handleSubmit}>
        <Modal.Body>
          <Form.Group className="mb-3">
            <Form.Label>ФИО</Form.Label>
            <Form.Control
              type="text"
              value={formData.fullName}
              onChange={(e) => handleChange("fullName", e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              value={formData.email}
              onChange={(e) => handleChange("email", e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Телефон</Form.Label>
            <Form.Control
              type="tel"
              value={formData.phone}
              onChange={(e) => handleChange("phone", e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Пароль</Form.Label>
            <Form.Control
              type="password"
              value={formData.password}
              onChange={(e) => handleChange("password", e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Роль</Form.Label>
            <Form.Select
              value={formData.status}
              onChange={(e) => handleChange("status", e.target.value)}
            >
              <option value="online">Аналитик</option>
              <option value="offline">Руководитель</option>
              <option value="online">Эксперт</option>
            </Form.Select>
          </Form.Group>
        </Modal.Body>

        <Modal.Footer>
          <Button variant="secondary" onClick={onHide}>
            Отмена
          </Button>
          <Button variant="primary" type="submit">
            Сохранить
          </Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
};

export default Users;