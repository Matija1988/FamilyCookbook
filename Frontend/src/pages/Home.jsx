import { Container, Form } from "react-bootstrap";
import NavBar from "../components/NavBar";

import "bootstrap/dist/css/bootstrap.min.css";
import RotatingCarousel from "../components/RotatingCarousel";
import { useState } from "react";
import RecipeService from "../services/RecipeService";
import HomeArticleList from "./homeComponents/homeArticleList.jsx";

export default function Home() {
  return (
    <>
      <RotatingCarousel></RotatingCarousel>
      <Container>
        <HomeArticleList></HomeArticleList>
      </Container>
    </>
  );
}
