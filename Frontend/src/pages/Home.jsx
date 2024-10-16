import { Container, Form, Row, Col } from "react-bootstrap";
import NavBar from "../components/NavBar";

import "bootstrap/dist/css/bootstrap.min.css";
import RotatingCarousel from "../components/RotatingCarousel";
import { useState } from "react";
import RecipeService from "../services/RecipeService";
import HomeArticleList from "./homeComponents/homeArticleList.jsx";
import PlaceholderRight from "./homeComponents/PlaceholderRight.jsx";
import PlaceholderLeft from "./homeComponents/PlaceholderLeft.jsx";

export default function Home() {
  return (
    <>
      <Row>
        <Col style={{ flex: "0 0 10%" }}></Col>
        <Col style={{ flex: "0 0 80%" }}>
          <RotatingCarousel></RotatingCarousel>
        </Col>
        <Col style={{ flex: "0 0 10%" }}></Col>
      </Row>
      <Row>
        <Col style={{ flex: "0 0 14%" }}>
          <PlaceholderLeft></PlaceholderLeft>
        </Col>
        <Col style={{ flex: "0 0 70%" }}>
          <Container>
            <HomeArticleList></HomeArticleList>
          </Container>
        </Col>
        <Col style={{ flex: "0 0 14%" }}>
          <PlaceholderRight></PlaceholderRight>
        </Col>
      </Row>
    </>
  );
}
