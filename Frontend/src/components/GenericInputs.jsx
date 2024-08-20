import { Form } from "react-bootstrap";
import PropTypes from "prop-types";

export default function GenericInputs({ atribute, value, type, onChange }) {
  return (
    <Form.Group>
      <Form.Label className="labelAtribute">{atribute}</Form.Label>
      <Form.Control
        type={type}
        name={atribute}
        defaultValue={value}
        onChange={onChange}
      />
    </Form.Group>
  );
}

GenericInputs.propTypes = {
  atribute: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  value: PropTypes.string,
};
