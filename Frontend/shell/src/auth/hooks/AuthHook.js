import { useState } from 'react';
import AuthApiService from '../service/AuthApiService';
import { TokenService } from '../../api/ApiClient';
import { useNavigate } from "react-router-dom";
import toast from 'react-hot-toast';

const useAuthForm = () => {
    const [credentials, setCredentials] = useState({
        name: '',
        password: '',
    });

    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCredentials((prev) => ({ ...prev, [name]: value }));
    };

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        try {
        const response = await AuthApiService.login(credentials);
        if (response?.accessToken) {
            TokenService.setAccessToken(response.accessToken);
            toast.success('Успешный вход!');
            navigate("/");
        } else {
            toast.error('Ошибка авторизации: токен не получен');
        }
        } catch (error) {
            toast.error('Неверные данные входа');
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    return {
        credentials,
        loading,
        handleChange,
        handleSubmit,
    };
};

export default useAuthForm;