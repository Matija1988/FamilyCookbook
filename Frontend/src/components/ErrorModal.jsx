import { Button, Modal } from "react-bootstrap";
import PropTypes from "prop-types";

export default function ErrorModal({ show, onHide, errors }) {

  

  return (
    <>
      <Modal show={show} onHide={onHide}>
        <Modal.Header closeButton>
          <Modal.Title>Error!!!</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <ul>
            {errors &&
              errors.map((error, index) => (
                <li key={index}>{error.message}</li>
              ))}
          </ul>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onHide}>
            CLOSE
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

ErrorModal.propTypes = {
  errors: PropTypes.array,
  show: PropTypes.bool.isRequired,
};
  