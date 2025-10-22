import PropTypes from 'prop-types';
import { Form } from 'react-bootstrap';

const Input = ({
    name, label, value, onChange, className, ...rest
}) => {
    return (
        <Form.Group className={`mb-2 ${className || ''}`} controlId={name}>
            <Form.Label className="fw-bold mb-2">{label}</Form.Label>
            <Form.Control name={name || ''} value={value || ''} onChange={onChange} {...rest} />
        </Form.Group>
    );
};

Input.propTypes = {
    name: PropTypes.string,
    label: PropTypes.string,
    value: PropTypes.string,
    onChange: PropTypes.func,
    className: PropTypes.string,
};

export default Input;
