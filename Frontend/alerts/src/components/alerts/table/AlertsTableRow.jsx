import PropTypes from 'prop-types';
import { PencilFill } from 'react-bootstrap-icons';


const AlertsTableRow = ({ alert, onEdit }) => {
    const handleAnchorClick = (event, action) => {
        event.preventDefault();
        action();
    };

    return (
        <tr>
            <td>{alert.title}</td>
            <td>{alert.ruleName}</td>
            <td>{alert.assignedToName}</td>
            <td>{alert.hostname}</td>
            <td>{alert.status}</td>
            <td>{alert.severity}</td>
            <td>{alert.createdAt}</td>
            <td>{alert.closedAt}</td>
            <td><a href="#" onClick={(event) => handleAnchorClick(event, onEdit)}><PencilFill /></a></td>
        </tr>
    );
};

AlertsTableRow.propTypes = {
    alert: PropTypes.object,
    onEdit: PropTypes.func
};

export default AlertsTableRow;
