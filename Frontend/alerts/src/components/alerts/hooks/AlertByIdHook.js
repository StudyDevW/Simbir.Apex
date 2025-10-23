import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useAlert = (id) => {
    const emptyAlert = {
        id: '',
        title: '',
        ruleId: '',
        ruleName: '',
        assignedToId: '',
        hostname: '',
        description: '',
        status: '',
        severity: '',
        rawData: '',
        createdAt: '',
        closedAt: '',
        resolutionNotes: '',
    };

    const [alert, setAlert] = useState({ ...emptyAlert });

    const getAlertById = async (alertId) => {
        if (alertId) {
            const data = await AlertsApiService.get(alertId);
            setAlert(data);
        } else {
            setAlert({ ...emptyAlert });
        }
    };

    useEffect(() => {
        getAlertById(id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id]);

    return {
        alert,
        setAlert
    };
};

export default useAlert;
