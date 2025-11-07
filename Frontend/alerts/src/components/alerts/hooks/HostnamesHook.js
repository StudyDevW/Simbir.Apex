import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useHostnames = () => {
    const [hostnames, setHostnames] = useState([]);

    const getHostnames = async () => {
        const data = await AlertsApiService.getHostnames();
        setHostnames(data ?? []);
    };

    useEffect(() => {
        getHostnames();
    }, []);

    return {
        hostnames,
    };
};

export default useHostnames;