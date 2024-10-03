import { Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import CustomButton from "../../components/CustomButton";

import "../../App.css";
import useAuth from "../../hooks/useAuth";
import { Routes, useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

export default function LogIn() {
  const { login } = useAuth();
  const navigate = useNavigate();

  function handleSubmit(e) {
    e.preventDefault();

    const data = new FormData(e.target);
    login({
      username: data.get("username"),
      password: data.get("password"),
    });
  }

  return (
    <Container className="w-25">
      <Form onSubmit={handleSubmit}>
        <InputText
          className="logIn-text"
          atribute="username"
          required={true}
        ></InputText>
        <InputText
          className="logIn-text"
          atribute="password"
          required={true}
          type="password"
        ></InputText>
        <Row className="row">
          <CustomButton className="logIn-btn" label="Login"></CustomButton>
        </Row>
        <Row>
          <CustomButton
            className="logIn-cancel"
            label="Cancel"
            onClick={() => navigate(RouteNames.HOME)}
          ></CustomButton>
        </Row>
      </Form>
    </Container>
  );
}
