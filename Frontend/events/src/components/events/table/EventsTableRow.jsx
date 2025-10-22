import PropTypes from 'prop-types';

const EventsTableRow = ({ event }) => {

    return (
        <tr>
            <td>{event.category}</td>
            <td>{event.device}</td>
            <td>{event.ip}</td>
            <td>{event.isLan}</td>
            <td>{event.timestamp}</td>
        </tr>
    );
};

EventsTableRow.propTypes = {
    event: PropTypes.object
};

export default EventsTableRow;
