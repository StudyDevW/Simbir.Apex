import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useUsers = () => {
    const [users, setUsers] = useState([]);

    const getUsers = async () => {
        const data = await AlertsApiService.getUsers();
        setUsers(data ?? []);
    };

    useEffect(() => {
        getUsers();
    }, []);

    return {
        users,
    };
};

export default useUsers;