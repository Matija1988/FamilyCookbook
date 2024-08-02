import { Form, Container, Row, Col } from "react-bootstrap";
import InputText from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import SelectionDropdown from "../../components/SelectionDropdown";
import RoleService from "../../services/RoleService";
import { useEffect, useState } from "react";
import { Input } from "react-select/animated";

export default function CreateMember() {
  const [roles, setRoles] = useState();

  const [error, setError] = useState(null);

  async function fetchRoles() {
    try {
      const response = await RoleService.readAll("role");
      if (response.ok) {
        setRoles(response.data);
      }
    } catch (error) {
      setError("Error fetching roles " + error.message);
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRoles();
  }, []);

  function handleSubmit() {}

  function onSelect() {}

  return (
    <>
      <Container className="primaryContainer">
        <Form onSubmit={handleSubmit} className="createForm">
          <Row>
            <Col>
              <InputText atribute="First name" value=""></InputText>
            </Col>
            <Col>
              <InputText atribute="Last name" value=""></InputText>
            </Col>
          </Row>
          <Row>
            <Col>
              <DateAndTime
                atribute="DateOfBirth"
                propertyName="dateOfBirth"
              ></DateAndTime>
            </Col>
            <Col>
              <SelectionDropdown
                atribute="Select role"
                entities={roles || []}
                onSelect={onSelect()}
              ></SelectionDropdown>
            </Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Username" value=""></InputText>
            </Col>
            <Col>
              <InputText atribute="Password" value=""></InputText>
            </Col>
          </Row>
          <Row></Row>
        </Form>
      </Container>
    </>
  );
}
