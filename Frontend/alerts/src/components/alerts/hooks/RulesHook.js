import { useEffect, useState } from 'react';
import AlertsApiService from '../service/AlertsApiService';

const useRules = () => {
    const [rules, setRules] = useState([]);

    const getRules = async () => {
        const data = await AlertsApiService.getRules();
        setRules(data ?? []);
    };

    useEffect(() => {
        getRules();
    }, []);

    return {
        rules,
    };
};

export default useRules;