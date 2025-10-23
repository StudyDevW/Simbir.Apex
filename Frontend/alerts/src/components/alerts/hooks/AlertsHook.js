import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useAlerts = (filters = {}) => {
    const [alerts, setAlerts] = useState([]);
    const [alertsRefresh, setAlertsRefresh] = useState(false);

    const handleAlertsChange = () => setAlertsRefresh((prev) => !prev);

    const getAlerts = async () => {
        const params = Object.fromEntries(
            Object.entries(filters).filter(([, value]) => value !== '' && value !== null && value !== undefined)
        );

        try {
            const data = await AlertsApiService.getAll(params);
            setAlerts(data ?? []);
        } catch (error) {
            console.error('Ошибка при загрузке алертов:', error);
            setAlerts([]);
        }
    };

    useEffect(() => {
        getAlerts();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [alertsRefresh]);

    return {
        alerts,
        handleAlertsChange
    };
};

export default useAlerts;
