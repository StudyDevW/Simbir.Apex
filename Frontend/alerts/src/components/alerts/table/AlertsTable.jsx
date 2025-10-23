import PropTypes from 'prop-types';
import { Table } from 'react-bootstrap';

const AlertsTable = ({ children }) => {
    return (
        <Table className='mt-2' striped responsive hover>
            <thead>
                <tr>
                    <th scope="col">Название</th>
                    <th scope="col">Шаблон</th>
                    <th scope="col">Назначен</th>
                    <th scope="col">Источник</th>
                    <th scope="col">Статус</th>
                    <th scope="col">Уровень риска</th>
                    <th scope="col">Создано</th>
                    <th scope="col">Закрыто</th>
                    <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                {children}
            </tbody >
        </Table >
    );
};

AlertsTable.propTypes = {
    children: PropTypes.node,
};

export default AlertsTable;
