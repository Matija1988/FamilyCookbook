import { Form } from "react-bootstrap";
import PropTypes from "prop-types";

export default function InputTextArea({ atribute, rows, value }) {
  return (
    <>
      <Form.Group>
        <Form.Label>{atribute}</Form.Label>
        <Form.Control
          as="textarea"
          rows={rows}
          name={atribute}
          defaultValue={value}
        />
      </Form.Group>
    </>
  );
}

InputTextArea.propTypes = {
  atribute: PropTypes.string.isRequired,
  rows: PropTypes.number,
};
