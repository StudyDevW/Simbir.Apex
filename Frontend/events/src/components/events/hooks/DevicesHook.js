import { useEffect, useState } from 'react';
import EventsApiService from '../service/EventsApiService';

const useDevices = () => {
    const [devices, setDevices] = useState([]);

    const getDevices = async () => {
        const data = await EventsApiService.getDevices();
        setDevices(data ?? []);
    };

    useEffect(() => {
        getDevices();
    }, []);

    return {
        devices,
    };
};

export default useDevices;