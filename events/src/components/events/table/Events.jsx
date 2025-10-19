import { useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { useFilter, useDeviceFilter } from '../hooks/EventsFilterHook.js';
import useEvents from '../hooks/EventsHook.js';
import EventsTable from './EventsTable.jsx';
import EventsTableRow from './EventsTableRow.jsx';
import Select from '../../input/Select.jsx';
import Input from '../../input/Input.jsx';
import { Button, Form, Modal } from 'react-bootstrap';
import PseudoQueryModal from './PseudoQueryModal.jsx';
import { Funnel, ArrowLeft } from "react-bootstrap-icons";

const Events = () => {
    const currentCategoryFilter = useFilter("category");
    const { devices, currentDeviceFilter } = useDeviceFilter();
    const currentLanFilter = useFilter("isLan");
    const currentStartDateFilter = useFilter("startDate");
    const currentEndDateFilter = useFilter("endDate");

    const [localCategory, setLocalCategory] = useState(currentCategoryFilter);
    const [localDevice, setLocalDevice] = useState(currentDeviceFilter);
    const [localLan, setLocalLan] = useState(currentLanFilter);
    const [startDateInput, setStartDateInput] = useState(currentStartDateFilter);
    const [endDateInput, setEndDateInput] = useState(currentEndDateFilter);

    const { events, clearFilters } = useEvents({
        categoryFilter: currentCategoryFilter,
        deviceFilter: currentDeviceFilter,
        lanFilter: currentLanFilter,
        startDateFilter: currentStartDateFilter,
        endDateFilter: currentEndDateFilter
    });

    const [searchParams, setSearchParams] = useSearchParams();
    const categories = ["unknown", "auth", "file_system", "process", "hardware", "network"];

    // Фильтры
    const [showFilters, setShowFilters] = useState(false);
    const handleCloseFilters = () => setShowFilters(false);
    const handleShowFilters = () => {
        setLocalCategory(currentCategoryFilter);
        setLocalDevice(currentDeviceFilter);
        setLocalLan(currentLanFilter);
        setStartDateInput(currentStartDateFilter);
        setEndDateInput(currentEndDateFilter);
        setShowFilters(true);
    };

    const applyFilters = () => {
        const next = new URLSearchParams(searchParams);

        localCategory ? next.set('category', localCategory) : next.delete('category');
        localDevice ? next.set('device', localDevice) : next.delete('device');
        localLan ? next.set('isLan', localLan) : next.delete('isLan');
        startDateInput ? next.set('startDate', startDateInput) : next.delete('startDate');
        endDateInput ? next.set('endDate', endDateInput) : next.delete('endDate');

        setSearchParams(next);
        handleCloseFilters();
    };

    const resetFilters = () => {
        clearFilters();
        setSearchParams({});
        setLocalCategory('');
        setLocalDevice('');
        setLocalLan('');
        setStartDateInput('');
        setEndDateInput('');
        handleCloseFilters();
    };

    // Псевдоязык
    const [showPseudo, setShowPseudo] = useState(false);
    const handleApplyPseudo = (parsed) => {
        clearFilters();
        const next = new URLSearchParams();
        Object.entries(parsed).forEach(([k, v]) => next.set(k, v));
        setSearchParams(next);
    };

    return (
        <>
            <div className='bg-dark p-3 fs-3 rounded fw-bold text-white mb-2'>
                <a href='' className='text-white text-decoration-none d-flex align-items-center'>
                    <ArrowLeft />
                    &nbsp;
                    <span className='mb-1'>События</span>
                </a>
            </div>
            <div className="d-flex mb-2">
                <Button variant="outline-dark" className="me-2" onClick={handleShowFilters}>
                    <Funnel />
                </Button>
                <Button variant="outline-dark" onClick={() => setShowPseudo(true)}>
                    &lt;/&gt;
                </Button>
            </div>

            <Modal show={showFilters} onHide={handleCloseFilters} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Фильтры событий</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Select
                        values={categories}
                        label='Категория'
                        value={localCategory}
                        onChange={(e) => setLocalCategory(e.target.value)}
                    />
                    <Form.Label className='fw-bold mb-2'>Источник</Form.Label>
                    <Form.Select value={localDevice} onChange={(e) => setLocalDevice(e.target.value)}>
                        <option value=''>Выберите значение</option>
                        {devices.map((item, index) => (
                            <option key={index} value={item.name}>{item.name}</option>
                        ))}
                    </Form.Select>

                    <div className="mt-3">
                        <Form.Label>Сеть</Form.Label>
                        <Form.Check type="radio" name="lanFilter" label="Все" checked={localLan === ''} onChange={() => setLocalLan('')} />
                        <Form.Check type="radio" name="lanFilter" label="Только LAN" checked={localLan === 'true'} onChange={() => setLocalLan('true')} />
                        <Form.Check type="radio" name="lanFilter" label="Не LAN" checked={localLan === 'false'} onChange={() => setLocalLan('false')} />
                    </div>

                    <div className='mt-3'>
                        <Input name='startDateFilter' label='С' value={startDateInput} onChange={(e) => setStartDateInput(e.target.value)} type='datetime-local' />
                        <Input className='mt-2' name='endDateFilter' label='До' value={endDateInput} onChange={(e) => setEndDateInput(e.target.value)} type='datetime-local' />
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={resetFilters}>Очистить</Button>
                    <Button variant="primary" onClick={applyFilters}>Применить</Button>
                </Modal.Footer>
            </Modal>

            <PseudoQueryModal
                show={showPseudo}
                handleClose={() => setShowPseudo(false)}
                applyPseudoQuery={handleApplyPseudo}
            />

            <EventsTable>
                {events.map((event) => (
                    <EventsTableRow key={event.id} event={event} />
                ))}
            </EventsTable>
        </>
    );
};

export default Events;
