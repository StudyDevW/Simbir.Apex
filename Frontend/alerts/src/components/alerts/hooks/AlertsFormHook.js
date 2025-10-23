import { useState } from 'react';
import toast from 'react-hot-toast';
import useAlert from './AlertByIdHook';
import AlertsApiService from '../service/AlertsApiService';



const useAlertsForm = (id, alertsChangeHandle) => {
    const { alert, setAlert } = useAlert(id);

    const [validated, setValidated] = useState(false);

    const resetValidity = () => {
        setValidated(false);
    };

    const getAlertObject = (formData) => {
        const title = formData.title;
        const ruleId = formData.ruleId;
        const assignedToId = formData.assignedToId;
        const hostname = formData.hostname;
        const description = formData.description;
        const status = formData.status;
        const severity = formData.severity;
        const rawData = formData.rawData;
        const createdAt = formData.createdAt;
        const closedAt = formData.closedAt;
        const resolutionNotes = formData.resolutionNotes;
        return {
            title: title,
            ruleId: ruleId,
            assignedToId: assignedToId,
            hostname: hostname,
            description: description,
            status: status,
            severity: severity,
            rawData: rawData,
            createdAt: createdAt,
            closedAt: closedAt,
            resolutionNotes: resolutionNotes,
        };
    };

    const handleChange = (event) => {
        const inputName = event.target.name;
        const inputValue = event.target.type === 'checkbox' ? event.target.checked : event.target.value;
        setAlert({
            ...alert,
            [inputName]: inputValue,
        });
    };

    const handleSubmit = async (event) => {
        const form = event.currentTarget;
        event.preventDefault();
        event.stopPropagation();
        const body = getAlertObject(alert);
        console.log(body)
        if (form.checkValidity()) {
            await AlertsApiService.update(id, body);
            if (alertsChangeHandle) alertsChangeHandle();
            toast.success('Элемент успешно сохранен', { id: 'AlertsTable' });
            return true;
        }
        setValidated(true);
        return false;
    };

    return {
        alert,
        validated,
        handleSubmit,
        handleChange,
        resetValidity,
    };
};

export default useAlertsForm;
