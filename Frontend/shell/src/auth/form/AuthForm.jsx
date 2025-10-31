import { Form, Button, Spinner, Card } from 'react-bootstrap';
import useAuthForm from '../hooks/AuthHook';

const AuthForm = () => {
  const { credentials, loading, handleChange, handleSubmit } = useAuthForm();

  return (
    <div className="d-flex justify-content-center align-items-center">
      <Card className="p-4 shadow" style={{ width: '380px', borderRadius: '16px' }}>
        <h4 className="text-center mb-4">Вход в систему</h4>
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-3" controlId="username">
            <Form.Label>Имя пользователя</Form.Label>
            <Form.Control
              type="text"
              name="username"
              value={credentials.username}
              onChange={handleChange}
              placeholder="Введите имя пользователя"
              required
            />
          </Form.Group>

          <Form.Group className="mb-3" controlId="password">
            <Form.Label>Пароль</Form.Label>
            <Form.Control
              type="password"
              name="password"
              value={credentials.password}
              onChange={handleChange}
              placeholder="Введите пароль"
              required
            />
          </Form.Group>

          <Button
            type="submit"
            variant="primary"
            className="w-100"
            disabled={loading}
          >
            {loading ? <Spinner size="sm" animation="border" /> : 'Войти'}
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default AuthForm;