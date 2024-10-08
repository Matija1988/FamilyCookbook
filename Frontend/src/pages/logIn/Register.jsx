import { Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import RegisterService from "../../services/RegisterService";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";

export default function Register() {
  const initialState = {
    firstName: "",
    lastName: "",
    dateOfBirth: {},
    username: "",
    password: "",
  };
  const navigate = useNavigate();

  const { showError, showErrorModal, hideError, errors } = useError();

  async function registerUser(e) {
    const response = await RegisterService.RegisterUser(e);
    if (!response.ok) {
      showError(response.data);
      return;
    }
    navigate(RouteNames.HOME);
  }

  async function handleSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const information = new FormData(form);

    if (information.get("dateOfBirth") != "") {
      initialState.dateOfBirth =
        information.get("dateOfBirth") + "T00:00:00.000Z";
    }

    initialState.firstName = information.get("First name");
    initialState.lastName = information.get("Last name");
    initialState.username = information.get("Username");
    initialState.password = information.get("Password");

    registerUser({
      firstName: initialState.firstName,
      lastName: initialState.lastName,
      username: initialState.username,
      password: initialState.password,
      dateOfBirth: initialState.dateOfBirth,
    });
  }

  return (
    <>
      <Container className="w-25">
        <Form onSubmit={handleSubmit}>
          <Form.Label>REGISTER</Form.Label>
          <InputText atribute="First name" required={true}></InputText>
          <InputText atribute="Last name" required={true}></InputText>
          <InputText atribute="Username" required={true}></InputText>
          <InputText atribute="Password" required={true}></InputText>
          <DateAndTime
            atribute="Date of birth"
            required={true}
            propertyName="dateOfBirth"
          ></DateAndTime>
          <Row>
            <Col>
              <CustomButton
                label="Register"
                type="submit"
                className="reg-btn"
              ></CustomButton>
            </Col>
            <Col>
              <CustomButton
                label="Cancel"
                type="button"
                onClick={() => navigate(RouteNames.HOME)}
                className="cncl-btn"
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
