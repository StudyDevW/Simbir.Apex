import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useAlerts = (filters = {}, page = 1) => {
    const [alerts, setAlerts] = useState([]);
    const [totalPages, setTotalPages] = useState(1);
    const [alertsRefresh, setAlertsRefresh] = useState(false);

    const handleAlertsChange = () => setAlertsRefresh((prev) => !prev);

    const getAlerts = async () => {
        const params = Object.fromEntries(
            Object.entries(filters).filter(
                ([, value]) => value !== '' && value !== null && value !== undefined
            )
        );

        params.page = page;

        const response = await AlertsApiService.getAll(params);

        //TODO: заменить на: setAlerts(response.items ?? []);
        setAlerts(response ?? []);
        setTotalPages(response.totalPages ?? 1);
    };

    useEffect(() => {
        getAlerts();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, alertsRefresh]);

    return {
        alerts,
        totalPages,
        handleAlertsChange
    };
};

export default useAlerts;
