import { Container, Form, Row, Col } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import InputText from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import SelectionDropdown from "../../components/SelectionDropdown";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";
import RoleService from "../../services/RoleService";
import MembersService from "../../services/MembersService";
import moment from "moment";
import { RouteNames } from "../../constants/constants";
import { update } from "../../services/HttpService";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";

export default function UpdateMember() {
  const [roles, setRoles] = useState();
  const [selectRoleId, setRoleId] = useState("");
  const [member, setMember] = useState({});

  const { showLoading, hideLoading } = useLoading();

  const { showError, showErrorModal, errors, hideError } = useError();

  const navigate = useNavigate();

  const routeParams = useParams();

  async function fetchRoles() {
    const response = await RoleService.readAll("role");
    if (!response.ok) {
      showError(response.data);
    }
    setRoles(response.data);
  }

  async function fetchMember() {
    const response = await MembersService.getById("member", routeParams.id);
    if (!response.ok) {
      showError(response.data);
    }
    let member = response.data;
    member.birthDate = moment.utc(member.dateOfBirth).format("yyyy-MM-DD");
    setMember(response.data);
    setRoleId(response.data.roleId);
  }

  async function updateMember(entity) {
    showLoading();
    const response = await MembersService.update(
      "member/update",
      routeParams.id,
      entity
    );
    if (!response.ok) {
      showError(response.data);
    }
    navigate(RouteNames.MEMBERS);
  }

  useEffect(() => {
    showLoading();
    fetchRoles();
    fetchMember();
    hideLoading();
  }, []);

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    let birthDate = "";

    if (information.get("dateOfBirth") != "") {
      birthDate = information.get("dateOfBirth") + "T00:00:00.000Z";
    }

    updateMember({
      firstName: information.get("First name"),
      lastName: information.get("Last name"),
      bio: information.get("Biography"),
      roleId: parseInt(selectRoleId),
      username: information.get("Username"),
      password: information.get("Password"),
      dateOfBirth: birthDate,
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
                onChanged={(e) => {
                  setRoleId(e.target.value);
                }}
                initValue={selectRoleId}
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
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
