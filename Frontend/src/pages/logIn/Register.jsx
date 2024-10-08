import { Col, Container, Form, Row } from "react-bootstrap";
import InputText    from "../../components/InputText";
import DateAndTime from "../../components/DateAndTime";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

export default function Register() {
  const navigate = useNavigate();
  async function handleSubmit() {}

  const handleFirstNameChange = () => {};

  const handleLastNameChange = () => {};

  const handleUsernameChange = () => {};

  const handlePasswordChange = () => {};
  return (
    <>
      <Container className="w-25">
        <Form onSubmit={handleSubmit}>
          <Form.Label>REGISTER</Form.Label>
          <InputText
            atribute="First name"
            required={true}
            onChange={handleFirstNameChange}
          ></InputText>
          <InputText
            atribute="Last name"
            required={true}
            onChange={handleLastNameChange}
          ></InputText>
          <InputText
            atribute="Username"
            required={true}
            onChange={handleUsernameChange}
          ></InputText>
          <InputText
            atribute="Password"
            required={true}
            onChange={handlePasswordChange}
          ></InputText>
          <DateAndTime atribute="Date of birth" required={true}></DateAndTime>
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
    </>
  );
}
