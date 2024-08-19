import { Col, Dropdown, DropdownButton, Form, Row } from "react-bootstrap";

export default function PageSizeDropdown({ onChanged, initValue }) {
  const pageSizeOptions = [2, 5, 10, 25, 50];

  return (
    <>
      <Row>
        <Col>
          <Form.Label>Page size</Form.Label>
        </Col>
      </Row>
      <Row>
        <Form.Select onChange={onChanged} value={initValue}>
          {pageSizeOptions.map((number) => (
            <option key={number} value={number}>
              {number}
            </option>
          ))}
        </Form.Select>
      </Row>
    </>
  );
}
