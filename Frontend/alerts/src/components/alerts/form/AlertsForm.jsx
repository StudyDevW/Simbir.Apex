import PropTypes from 'prop-types';
import Input from '../../input/Input.jsx';
import Select from '../../input/Select.jsx';
import useHostnames from '../hooks/HostnamesHook.js';
import useUsers from '../hooks/UsersHook.js';

const AlertsForm = ({ alert, handleChange }) => {
    const { hostnames } = useHostnames();
    const { users } = useUsers();
    const severities = ["Низкий", "Средний", "Высокий", "Критический"]
    const statuses = ["Активна", "Не активна"]
    return (
        <>
            <Input name='title' label='Название' value={alert.title} onChange={handleChange}
                type='text' required />
            <Input name='rule' label='Шаблон' value={alert.ruleName} type='text' disabled />
            <Select values={users} name='assignedToId' label='Назначено' value={alert.assignedToId} onChange={handleChange}
                required />
            <Select values={hostnames} name='hostname' label='Источник' value={alert.hostname} onChange={handleChange}
                required />
            <Input name='description' label='Описание' value={alert.description} as='textarea' onChange={handleChange} />
            <Select values={statuses} name='status' label='Статус' value={alert.status} onChange={handleChange}
                required />
            <Select values={severities} name='severity' label='Уровень риска' value={alert.severity} onChange={handleChange}
                required />
            <Input name='rawData' label='Сырые данные' value={alert.rawData} as='textarea' rows={5} onChange={handleChange} />
            <Input name='createdAt' label='Создано' value={alert.createdAt} onChange={handleChange}
                type='datetime-local' disabled />
            <Input name='createdAt' label='Закрыто' value={alert.closedAt} onChange={handleChange}
                type='datetime-local' disabled />
            <Input name='resolutionNotes' label='Комментарии' value={alert.resolutionNotes} as='textarea' rows={3} onChange={handleChange} />
        </>
    );
};

AlertsForm.propTypes = {
    alert: PropTypes.object,
    handleChange: PropTypes.func,
};

export default AlertsForm;
