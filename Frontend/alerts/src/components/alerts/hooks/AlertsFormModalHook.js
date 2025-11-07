import { useState } from 'react';
import useAlertsForm from './AlertsFormHook';



const useAlertsFormModal = (alertsChangeHandle) => {
    const [isModalShow, setShowModal] = useState(false);

    const showModal = () => {
        setShowModal(true);
    };

    const hideModal = () => {
        setShowModal(false);
    };

    const [currentId, setCurrentId] = useState(0);
    

    const {
        alert,
        validated,
        handleSubmit,
        handleChange,
        resetValidity,
    } = useAlertsForm(currentId, alertsChangeHandle);

    const showModalDialog = (id) => {
        setCurrentId(id);
        resetValidity();
        showModal();
    };

    const onClose = () => {
        hideModal();
    };

    const onSubmit = async (event) => {
        if (await handleSubmit(event)) {
            onClose();
        }
    };

    return {
        isFormModalShow: isModalShow,
        isFormValidated: validated,
        showFormModal: showModalDialog,
        currentAlert: alert,
        handleAlertChange: handleChange,
        handleFormSubmit: onSubmit,
        handleFormClose: onClose,
    };
};

export default useAlertsFormModal;
