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
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import Sidebar from "../AdminPanel/Sidebar";

export default function CreateMember() {
  const initialState = {
    firstName: "",
    lastName: "",
    dateOfBirth: {},
    roleId: null,
    username: "",
    password: "",
    biography: "",
  };

  const [member, setMember] = useState(initialState);
  const [roles, setRoles] = useState();
  const [roleId, setRoleId] = useState();

  const { showLoading, hideLoading } = useLoading();

  const { showError, showErrorModal, errors, hideError } = useError();

  const [submitted, setSubmitted] = useState(false);

  const navigate = useNavigate();

  async function fetchRoles() {
    showLoading();

    const response = await RoleService.readAll("role");
    if (!response.ok) {
      hideLoading();
      showError(response);
    }
    setRoles(response.data);
    hideLoading();
  }

  async function addMember(e) {
    showLoading();
    const response = await MembersService.create("member/create", e);
    if (!response.ok) {
      showError(response.data);
      hideLoading();
      return;
    }
    hideLoading();
    setSubmitted(true);
  }

  useEffect(() => {
    fetchRoles();
  }, []);

  function handleSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const information = new FormData(form);

    member.dateOfBirth = "";

    if (information.get("dateOfBirth") != "") {
      member.dateOfBirth = information.get("dateOfBirth") + "T00:00:00.000Z";
    }

    member.firstName = information.get("First name");
    member.lastName = information.get("Last name");
    member.biography = information.get("Biography");
    member.username = information.get("Username");
    member.password = information.get("Password");
    member.roleId = parseInt(roleId);

    addMember({
      firstName: member.firstName,
      lastName: member.lastName,
      bio: member.biography,
      dateOfBirth: member.dateOfBirth,
      username: member.username,
      password: member.password,
      roleId: member.roleId,
    });
  }

  function handleCancel() {
    navigate(RouteNames.MEMBERS);
  }

  const newMember = () => {
    setMember(initialState);
    setSubmitted(false);
  };

  const handleReturn = () => {
    navigate(RouteNames.MEMBERS);
  };

  return (
    <>
      <Sidebar></Sidebar>
      <Container className="primaryContainer">
        <h1>CREATE MEMBER</h1>
        {submitted ? (
          <div>
            <h4>Member submitted successfully</h4>

            <CustomButton
              variant="primary"
              label="ADD ANOTHER"
              onClick={newMember}
            ></CustomButton>
            <CustomButton
              variant="secondary"
              label="RETURN"
              onClick={handleReturn}
            ></CustomButton>
          </div>
        ) : (
          <Form onSubmit={handleSubmit} className="createForm">
            <Row>
              <Col>
                <InputText
                  atribute="First name"
                  value=""
                  required={true}
                ></InputText>
              </Col>
              <Col>
                <InputText
                  atribute="Last name"
                  value=""
                  required={true}
                ></InputText>
              </Col>
            </Row>
            <Row>
              <Col>
                <DateAndTime
                  atribute="Date of birth"
                  propertyName="dateOfBirth"
                  required={true}
                ></DateAndTime>
              </Col>
              <Col>
                <SelectionDropdown
                  atribute="Select role"
                  entities={roles || []}
                  onChanged={(r) => setRoleId(r.target.value)}
                ></SelectionDropdown>
              </Col>
            </Row>
            <Row>
              <Col>
                <InputText
                  atribute="Username"
                  value=""
                  required={true}
                ></InputText>
              </Col>
              <Col>
                <InputText
                  atribute="Password"
                  value=""
                  required={true}
                ></InputText>
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
        )}
      </Container>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
