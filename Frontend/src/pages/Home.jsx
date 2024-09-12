import { Container, Form } from "react-bootstrap";
import NavBar from "../components/NavBar";

import "bootstrap/dist/css/bootstrap.min.css";
import RotatingCarousel from "../components/RotatingCarousel";
import { useState } from "react";
import RecipeService from "../services/RecipeService";

export default function Home() {
  

  return (
    <>
      <RotatingCarousel></RotatingCarousel>
      <Container>
        <Form.Label>HOME PAGE</Form.Label>
      </Container>
    </>
  );
}
