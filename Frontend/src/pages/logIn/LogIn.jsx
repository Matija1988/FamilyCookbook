import { Alert, Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import CustomButton from "../../components/CustomButton";

import "../../App.css";
import useAuth from "../../hooks/useAuth";
import { Routes, useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import { useState } from "react";

export default function LogIn() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();

    const data = new FormData(e.target);
    const error = await login({
      username: data.get("username"),
      password: data.get("password"),
    });
    if (error) {
      setErrorMessage(error);
    }
  }

  return (
    <Container className="w-25">
      <Form onSubmit={handleSubmit}>
        {errorMessage && <Alert variant="danger">{errorMessage}</Alert>}
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
          <CustomButton
            variant="link"
            label="Not registered? Register as member"
            onClick={() => navigate(RouteNames.REGISTER)}
          ></CustomButton>
        </Row>
      </Form>
    </Container>
  );
}
