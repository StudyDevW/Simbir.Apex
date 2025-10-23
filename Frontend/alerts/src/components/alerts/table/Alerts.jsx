import { Button, Form, Modal } from 'react-bootstrap';
import useAlertsFormModal from '../hooks/AlertsFormModalHook.js';
import useAlerts from '../hooks/AlertsHook.js';
import AlertsTable from './AlertsTable.jsx';
import AlertsTableRow from './AlertsTableRow.jsx';
import AlertsForm from '../form/AlertsForm.jsx';
import useAlertsFiltersForm from '../hooks/AlertsFiltersHook.js';
import AlertsFiltersForm from '../form/AlertsFiltersForm.jsx';
import { Funnel } from 'react-bootstrap-icons';


const Alerts = () => {
    const {
        filters,
        handleFiltersChange,
        showFiltersForm,
        handleShowFiltersForm,
        handleCloseFiltersForm,
        applyFilters,
        resetFilters,
    } = useAlertsFiltersForm();
    
    const { alerts, handleAlertsChange } = useAlerts(filters);

    const {
        isFormModalShow,
        isFormValidated,
        showFormModal,
        currentAlert,
        handleAlertChange,
        handleFormSubmit,
        handleFormClose,
    } = useAlertsFormModal(handleAlertsChange);

    return (
        <>
            <Button variant="outline-dark" className="me-2" onClick={handleShowFiltersForm}>
                <Funnel />
            </Button>
            <AlertsTable>
                {
                    alerts.map((alert) =>
                        <AlertsTableRow key={alert.id}
                            alert={alert}
                            onEdit={() => showFormModal(alert.id)}
                        />)
                }
            </AlertsTable>
            <Modal show={isFormModalShow} backdrop='static' onHide={handleFormClose}>
                <Modal.Header className='pt-2 pb-2 ps-3 pe-3' closeButton>
                    <Modal.Title>Редактирование угрозы</Modal.Title>
                </Modal.Header>
                <Form className='m-0 rounded-bottom' noValidate validated={isFormValidated} onSubmit={handleFormSubmit}>
                    <Modal.Body>
                        <AlertsForm alert={currentAlert} handleChange={handleAlertChange} />
                    </Modal.Body>

                    <Modal.Footer className='m-0 pt-2 pb-2 ps-3 pe-3 row justify-content-center'>
                        <Button variant='secondary' className='col-5 m-0 me-2'
                            onClick={handleFormClose}>
                            Отмена
                        </Button>
                        <Button variant='primary' className='col-5 m-0 ms-2' type='submit'>
                            Сохранить
                        </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
            <Modal show={showFiltersForm} onHide={handleCloseFiltersForm} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Фильтры угроз</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <AlertsFiltersForm filters={filters} handleChange={handleFiltersChange} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => {resetFilters(); handleAlertsChange();}}>Очистить</Button>
                    <Button variant="primary" onClick={() => {applyFilters(); handleAlertsChange();}}>Применить</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default Alerts;
