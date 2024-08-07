import { Row, Col, Form } from "react-bootstrap";
import PropTypes from "prop-types";

export default function DateAndTime({ atribute, propertyName, value }) {
  return (
    <Form.Group>
      <Form.Label className="labelDate">{atribute}</Form.Label>
      <Form.Control type="date" name={propertyName} defaultValue={value} />
    </Form.Group>
  );
}

DateAndTime.propTypes = {
  atribute: PropTypes.string,
  propertyName: PropTypes.string,
  value: PropTypes.string,
};