import { Alert, Button, Container } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const ErrorPage = () => {
    const navigate = useNavigate();

    return (
        <Container fluid className="p-2 row justify-content-center">
            <Container className='col-md-6'>
                <Alert variant="danger">
                    Страница не найдена
                </Alert>
                <Button className="w-25 mt-2" variant="primary" onClick={() => navigate(-1)}>Назад</Button>
            </Container>
        </Container>
    );
};

export default ErrorPage;
