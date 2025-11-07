import PropTypes from 'prop-types';
import Input from '../../input/Input.jsx';
import Select from '../../input/Select.jsx';
import useDevices from '../hooks/DevicesHook.js';
import { Form } from 'react-bootstrap';
import { useState } from 'react';

const EventsFiltersForm = ({ filters, handleChange }) => {
    const { devices } = useDevices();
    const categories = ["unknown", "auth", "file_system", "process", "hardware", "network"];
    const [isLan, setIsLan] = useState(filters.isLan ?? '');

    return (
        <>
            <Select values={categories} name='category' label='Категория' value={filters.category} onChange={handleChange} />
            <Select values={devices} name='device' label='Источник' value={filters.device} onChange={handleChange} />
            <div className="mt-3">
                <Form.Label>Сеть</Form.Label>
                <Form.Check type="radio" name="isLan" label="Все" checked={isLan === ''} onChange={() => {setIsLan(''); filters.isLan = ''}} />
                <Form.Check type="radio" name="isLan" label="Только LAN" checked={isLan === 'true'} onChange={() => {setIsLan('true'); filters.isLan = 'true'}} />
                <Form.Check type="radio" name="isLan" label="Не LAN" checked={isLan === 'false'} onChange={() => {setIsLan('false'); filters.isLan = 'false'}} />
            </div>
            <Input name='startDate' label='С' value={filters.startDate} onChange={handleChange}
                type='datetime-local' />
            <Input name='endDate' label='До' value={filters.endDate} onChange={handleChange}
                type='datetime-local' />
        </>
    );
};

EventsFiltersForm.propTypes = {
    filters: PropTypes.object,
    handleChange: PropTypes.func,
};

export default EventsFiltersForm;
