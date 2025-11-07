import React, { useEffect, useState } from "react";
import { Button, Card, Row, Col, Spinner } from "react-bootstrap";
import { ApiClient } from "../api/ApiClient";
import toast from "react-hot-toast";

//TODO: доделать
const ProfilePage = () => {
  const [user, setUser] = useState({});
  const [loading, setLoading] = useState(true);

  const fetchUser = async () => {
    try {
      const data = await ApiClient.get("/Auth/Me");
      setUser(data);
    } catch (error) {
      toast.error("Не удалось загрузить профиль");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUser();
  }, []);

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
        <Spinner animation="border" />
      </div>
    );
  }

//   if (!user) {
//     return <div className="text-center mt-5">Пользователь не найден</div>;
//   }

  return (
    <>
        <div className="container">
        <div className="d-flex justify-content-between align-items-center mb-4">
            <h2 className="fw-semibold">Личный кабинет</h2>
            <div>
            {/* <Button variant="outline-primary" className="me-2">
                Сменить пароль
            </Button> */}
            <Button variant="outline-secondary">Выйти</Button>
            </div>
        </div>

        <Card className="shadow-sm border-0 p-4 rounded-4">
            <div className="d-flex justify-content-between align-items-start mb-3">
                <div>
                    <h5 className="fw-bold">{user.first_name + " " + user.last_name}</h5>
                    <div className="text-muted">{user.role || "Руководитель"}</div>
                </div>
                {/* <Button variant="info" className="text-white px-4 fw-semibold">
                    Редактировать
                </Button> */}
            </div>

            <Row className="g-3">
                <Col md={6}>
                    <div className="bg-light p-3 rounded-3">
                    <div className="text-secondary small mb-1">Логин</div>
                    <div className="fw-semibold">{user.username || "ivan.ivanov"}</div>
                    </div>
                </Col>

                {/* <Col md={6}>
                    <div className="bg-light p-3 rounded-3">
                    <div className="text-secondary small mb-1">Email</div>
                    <div className="fw-semibold">{user.email}</div>
                    </div>
                </Col> */}

                <Col md={6}>
                    <div className="bg-light p-3 rounded-3">
                    <div className="text-secondary small mb-1">Телефон</div>
                    <div className="fw-semibold">{user.phone_number || "+7 (999) 123-45-67"}</div>
                    </div>
                </Col>
            </Row>
        </Card>
        </div>
    </>
  );
};

export default ProfilePage;