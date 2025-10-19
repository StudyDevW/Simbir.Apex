import { Button, Form, Modal } from "react-bootstrap";
import { createPortal } from "react-dom";
import { type ReactNode } from "react";

interface ModalFormProps {
  show: boolean;
  title: string;
  validated: boolean;
  onSubmit: (event: React.FormEvent<HTMLFormElement>) => void | Promise<void>;
  onClose: () => void;
  children: ReactNode;
}

const ModalForm = ({
  show,
  title,
  validated,
  onSubmit,
  onClose,
  children,
}: ModalFormProps) => {
  return createPortal(
    <Modal show={show} backdrop="static" onHide={onClose}>
      <Modal.Header className="pt-2 pb-2 ps-3 pe-3" closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Form
        className="m-0 rounded-bottom"
        noValidate
        validated={validated}
        onSubmit={onSubmit}
      >
        <Modal.Body>{children}</Modal.Body>

        <Modal.Footer className="m-0 pt-2 pb-2 ps-3 pe-3 row justify-content-center">
          <Button
            variant="secondary"
            className="col-5 m-0 me-2"
            onClick={onClose}
          >
            Отмена
          </Button>
          <Button variant="primary" className="col-5 m-0 ms-2" type="submit">
            Сохранить
          </Button>
        </Modal.Footer>
      </Form>
    </Modal>,
    document.body
  );
};

export default ModalForm;
