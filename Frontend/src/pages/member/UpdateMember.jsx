import { Container, Form, Row, Col } from "react-bootstrap";
import { useParams } from "react-router-dom";
import InputText from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import SelectionDropdown from "../../components/SelectionDropdown";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";
import RoleService from "../../services/RoleService";
import MembersService from "../../services/MembersService";
import moment from "moment";

export default function UpdateMember() {
  const [roles, setRoles] = useState();
  const [roleId, setRoleId] = useState();
  const [member, setMember] = useState({});

  const [error, setError] = useState(null);

  const routeParams = useParams();

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

  async function fetchMember() {
    try {
      const response = await MembersService.getById("member", routeParams.id);
      if (response.ok) {
        let member = response.data;
        member.birthDate = moment.utc(member.dateOfBirth).format("yyyy-MM-DD");
        setMember(response.data);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRoles();
    fetchMember();
  }, []);

  function handleSubmit() {}

  function handleCancel() {}

  return (
    <>
      <Container className="primaryContainer">
        <Form onSubmit={handleSubmit} className="createForm">
          <Row>
            <Col>
              <InputText
                atribute="First name"
                value={member.firstName}
              ></InputText>
            </Col>
            <Col>
              <InputText
                atribute="Last name"
                value={member.lastName}
              ></InputText>
            </Col>
          </Row>
          <Row>
            <Col>
              <DateAndTime
                atribute="Date of birth"
                propertyName="dateOfBirth"
                value={member.birthDate}
              ></DateAndTime>
            </Col>
            <Col>
              <SelectionDropdown
                atribute="Select role"
                entities={roles || []}
                initValue={member.roleId}
                onSelect={(r) => setRoleId(r.target.value)}
              ></SelectionDropdown>
            </Col>
          </Row>
          <Row>
            <Col>
              <InputText
                atribute="Username"
                value={member.username}
              ></InputText>
            </Col>
            <Col>
              <InputText
                atribute="Password"
                value={member.password}
              ></InputText>
            </Col>
          </Row>
          <Row>
            <Form.Label>Biography</Form.Label>
            <Form.Control
              as="textarea"
              rows={3}
              name="Biography"
              value={member.bio}
            />
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
