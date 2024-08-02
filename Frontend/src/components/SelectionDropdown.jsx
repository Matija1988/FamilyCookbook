import Dropdown from "react-bootstrap/Dropdown";
import DropdownButton from "react-bootstrap/DropdownButton";

export default function SelectionDropdown({ atribute, entities, onSelect }) {
  return (
    <>
      <select onChange={onSelect} className="select-header">
        <option value="">{atribute}</option>
        {entities.map((item) => (
          <option key={item.id} value={item.id}>
            {item.name}
          </option>
        ))}
      </select>
    </>
  );
}
