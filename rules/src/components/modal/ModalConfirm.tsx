import { Button, Modal } from "react-bootstrap";
import { createPortal } from "react-dom";

interface ModalConfirmProps {
  show: boolean;
  title: string;
  message: string;
  onConfirm: () => void | Promise<void>;
  onClose: () => void;
}

const ModalConfirm = ({
  show,
  title,
  message,
  onConfirm,
  onClose,
}: ModalConfirmProps) => {
  return createPortal(
    <Modal show={show} backdrop="static" onHide={onClose}>
      <Modal.Header className="pt-2 pb-2 ps-3 pe-3" closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>

      <Modal.Body>{message}</Modal.Body>

      <Modal.Footer className="m-0 pt-2 pb-2 ps-3 pe-3 row justify-content-center">
        <Button
          variant="secondary"
          className="col-5 m-0 me-2"
          onClick={onClose}
        >
          Нет
        </Button>
        <Button variant="primary" className="col-5 m-0 ms-2" onClick={onConfirm}>
          Да
        </Button>
      </Modal.Footer>
    </Modal>,
    document.body
  );
};

export default ModalConfirm;
