import PropTypes from 'prop-types';
import Input from '../../input/Input.jsx';
import Select from '../../input/Select.jsx';
import useHostnames from '../hooks/HostnamesHook.js';
import useUsers from '../hooks/UsersHook.js';
import useRules from '../hooks/RulesHook.js';

const AlertsFiltersForm = ({ filters, handleChange }) => {
    const { hostnames } = useHostnames();
    const { users } = useUsers();
    const { rules } = useRules();
    const severities = ["Низкий", "Средний", "Высокий", "Критический"]
    const statuses = ["Активна", "Не активна"]
    return (
        <>
            <Select values={rules} name='ruleId' label='Шаблон' value={filters.ruleId} onChange={handleChange} />
            <Select values={users} name='assignedToId' label='Назначено' value={filters.assignedToId} onChange={handleChange} />
            <Select values={hostnames} name='hostname' label='Источник' value={filters.hostname} onChange={handleChange} />
            <Select values={statuses} name='status' label='Статус' value={alert.status} onChange={handleChange} />
            <Select values={severities} name='severity' label='Уровень риска' value={filters.severity} onChange={handleChange} />
            <Input name='startDate' label='С' value={filters.startDate} onChange={handleChange}
                type='datetime-local' />
            <Input name='endDate' label='До' value={filters.endDate} onChange={handleChange}
                type='datetime-local' />
        </>
    );
};

AlertsFiltersForm.propTypes = {
    filters: PropTypes.object,
    handleChange: PropTypes.func,
};

export default AlertsFiltersForm;
