import Dropdown from "react-bootstrap/Dropdown";
import DropdownButton from "react-bootstrap/DropdownButton";
import { Form } from "react-bootstrap";

export default function SelectionDropdown({
  atribute,
  entities,
  onSelect,
  initValue,
}) {
  return (
    <>
      <Form.Label>{atribute}</Form.Label>
      <Form.Select
        aria-label="Default select example"
        onChange={onSelect}
        className="select-header"
        defaultValue={initValue}
      >
        <option>{atribute}</option>
        {entities.map((item) => (
          <option key={item.id} value={item.id}>
            {item.name}
          </option>
        ))}
      </Form.Select>
    </>
  );
}
