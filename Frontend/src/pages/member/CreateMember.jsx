import { Form, Container, Row, Col } from "react-bootstrap";
import InputText from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import SelectionDropdown from "../../components/SelectionDropdown";
import RoleService from "../../services/RoleService";
import { useEffect, useState } from "react";
import { Input } from "react-select/animated";
import CustomButton from "../../components/CustomButton";
import MembersService from "../../services/MembersService";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

export default function CreateMember() {
  const [roles, setRoles] = useState();
  const [roleId, setRoleId] = useState();

  const [error, setError] = useState(null);

  const navigate = useNavigate();

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

  async function addActivity(e) {
    const response = await MembersService.create("member/create", e);
    try {
      if (response.ok) {
        navigate(RouteNames.MEMBERS);
        return;
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRoles();
  }, []);

  function handleSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const information = new FormData(form);

    let birthDate = "";

    if (information.get("dateOfBirth") != "") {
      birthDate = information.get("dateOfBirth") + "T00:00:00.000Z";
    }

    let name = information.get("First name");
    let surname = information.get("Last name");
    let biography = information.get("Biography");
    let user = information.get("Username");
    let pass = information.get("Password");

    addActivity({
      firstName: name,
      lastName: surname,
      bio: biography,
      dateOfBirth: birthDate,
      username: user,
      password: pass,
      roleId: parseInt(roleId),
    });
  }

  function handleCancel() {
    navigate(RouteNames.MEMBERS);
  }

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
                atribute="Date of birth"
                propertyName="dateOfBirth"
              ></DateAndTime>
            </Col>
            <Col>
              <SelectionDropdown
                atribute="Select role"
                entities={roles || []}
                onSelect={(r) => setRoleId(r.target.value)}
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
          <Row>
            <Form.Label>Biography</Form.Label>
            <Form.Control as="textarea" rows={3} name="Biography" />
          </Row>
          <Row>
            <Col>
              <CustomButton
                label="SUBMIT"
                type="submit"
                variant="primary  m-3"
              ></CustomButton>
              <CustomButton
                label="CANCEL"
                onClick={handleCancel}
                variant="secondary  m-3"
              ></CustomButton>
            </Col>
          </Row>
        </Form>
      </Container>
    </>
  );
}
