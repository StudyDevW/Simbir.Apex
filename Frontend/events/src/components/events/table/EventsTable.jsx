import PropTypes from 'prop-types';
import { Table } from 'react-bootstrap';

const EventsTable = ({ children }) => {
    return (
        <Table className='mt-2' striped responsive hover>
            <thead>
                <tr>
                    <th scope="col">Категория</th>
                    <th scope="col">Источник</th>
                    <th scope="col">IP адрес</th>
                    <th scope="col">Локальная сеть</th>
                    <th scope="col">Время</th>
                </tr>
            </thead>
            <tbody>
                {children}
            </tbody >
        </Table >
    );
};

EventsTable.propTypes = {
    children: PropTypes.node,
};

export default EventsTable;
