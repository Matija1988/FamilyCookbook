import { Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import CustomButton from "../../components/CustomButton";

import "../../App.css";
import useAuth from "../../hooks/useAuth";

export default function LogIn() {
  const { login } = useAuth();

  return (
    <Container className="w-25">
      <Form>
        <InputText className="logIn-text" atribute="username"></InputText>
        <InputText className="logIn-text" atribute="password"></InputText>
        <Row className="row">
          <CustomButton className="logIn-btn" label="Login"></CustomButton>
        </Row>
        <Row>
          <CustomButton className="logIn-cancel" label="Cancel"></CustomButton>
        </Row>
      </Form>
    </Container>
  );
}
