import { Button, Col, Modal, Row } from "react-bootstrap";

export default function DeleteModal({
  show,
  handleClose,
  handleDelete,
  entity,
}) {
  return (
    <Modal show={show} onHide={handleClose} className="delete-modal">
      <Modal.Header closeButton className="modal-header">
        CONFIRM ACTION
      </Modal.Header>
      <Modal.Body className="modal-body">
        This action will delete this entry!!! Do you wish to continue?
      </Modal.Body>
      <Modal.Footer className="modal-footer">
        <Row>
          <Col>
            <Button variant="primary" onClick={handleClose}>
              CLOSE
            </Button>
          </Col>
          <Col>
            <Button variant="danger" onClick={() => handleDelete(entity)}>
              CONFIRM
            </Button>
          </Col>
        </Row>
      </Modal.Footer>
    </Modal>
  );
}
