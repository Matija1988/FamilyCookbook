import { Form } from "react-bootstrap";
import PropTypes from "prop-types";

export default function InputText({ atribute, value }) {
  return (
    <Form.Group>
      <Form.Label className="labelAtribute">{atribute}</Form.Label>
      <Form.Control name={atribute} defaultValue={value} />
    </Form.Group>
  );
}

InputText.propTypes = {
  atribute: PropTypes.string.isRequired,
  value: PropTypes.any,
};
