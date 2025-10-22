import { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import parsePseudoQuery from '../hooks/ParsePseudoQueryHook';

const PseudoQueryModal = ({ show, handleClose, applyPseudoQuery }) => {
    const [query, setQuery] = useState('');

    const handleApply = () => {
        const parsed = parsePseudoQuery(query);
        applyPseudoQuery(parsed);
        handleClose();
        setQuery('');
    };

    return (
        <Modal show={show} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Запрос на псевдоязыке</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form.Group>
                    <Form.Label>Введите запрос (пример ниже)</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={6}
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        placeholder={`from: 2025-10-01\nto: 2025-10-31\ndevice: "server 1"\ncategory: auth\nisLan: true`}
                    />
                    <Form.Text className="text-muted">
                        Поддерживаемые ключи: from, to, device, category, isLan.
                        Используйте кавычки для строк с пробелами.
                    </Form.Text>
                </Form.Group>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={() => { setQuery(''); handleClose(); }}>
                    Закрыть
                </Button>
                <Button variant="primary" onClick={handleApply}>
                    Применить
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default PseudoQueryModal;