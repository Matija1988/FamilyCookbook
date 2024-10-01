import { Form } from "react-bootstrap";
import PropTypes from "prop-types";

export default function InputText({
  atribute,
  value,
  required,
  className,
  type,
}) {
  return (
    <Form.Group>
      <Form.Label className="labelAtribute">{atribute}</Form.Label>
      <Form.Control
        name={atribute}
        defaultValue={value}
        required={required}
        className={className}
        type={type}
      />
    </Form.Group>
  );
}

InputText.propTypes = {
  atribute: PropTypes.string.isRequired,
  value: PropTypes.string,
};
