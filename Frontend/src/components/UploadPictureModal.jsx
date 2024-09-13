import { Form, Modal } from "react-bootstrap";

export default function UplaodPictureModal({
  recipe,
  setPicture,
  showModal,
  closeModal,
}) {
  return (
    <>
      <Modal show={showModal} onHide={closeModal}>
        <Modal.Header closeButton>
          <Modal.Title>
            Upload picture for <br />
            {recipe.title}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group>
              <Form.Control
                type="file"
                size="lg"
                name="Picture"
                onChange={setPicture}
              />
            </Form.Group>
            <hr />
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={closeModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}
