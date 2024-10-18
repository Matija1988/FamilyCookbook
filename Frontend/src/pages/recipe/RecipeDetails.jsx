import { useEffect, useState } from "react";
import {
  Button,
  Col,
  Collapse,
  Container,
  Form,
  ListGroup,
  ListGroupItem,
  Row,
} from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import RecipeService from "../../services/RecipeService";

import "./recipeDetails.css";
import CommentList from "../../components/CommentList";
import CommentCreate from "../../components/CommentCreate";
import ErrorModal from "../../components/ErrorModal";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";
import { App } from "../../constants/constants";
import PlaceholderLeft from "../homeComponents/PlaceholderLeft";
import PlaceholderRight from "../homeComponents/PlaceholderRight";

export default function RecipeDetails() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    text: "",
    categoryName: "",
    categoryDescription: "",
    members: [],
    pictureLocation: "",
  };
  const [recipe, setRecipe] = useState(recipeState);
  const [members, setMembers] = useState([]);
  const [category, setCategory] = useState("");
  const { showError, showErrorModal, errors, hideError } = useError();
  const { showLoading, hideLoading } = useLoading();
  const [openMemberId, setOpenMemberId] = useState(null);

  const URL = App.URL;

  const routeParams = useParams();
  const navigate = useNavigate();

  async function fetchRecipe() {
    showLoading();
    const response = await RecipeService.getById("recipe", routeParams.id);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    setRecipe(response.data);
    setMembers(response.data.members);
    setCategory(response.data.categoryName);
    hideLoading();
  }

  useEffect(() => {
    fetchRecipe();
  }, []);

  const toggleBiography = (id) => {
    setOpenMemberId(openMemberId === id ? null : id);
  };

  return (
    <>
      <Row>
        <Col style={{ flex: "0 0 20%" }}>
          <br></br>
          <br></br>
          <br></br>
          <PlaceholderLeft></PlaceholderLeft>
        </Col>
        <Col style={{ flex: "0 0 60%" }}>
          <Container className="recDetails">
            <h2>{recipe.title}</h2>
            <h4>{recipe.subtitle}</h4>
            <h6 className="category">CATEGORY: {recipe.categoryName}</h6>
            <h6 className="category-desc">{recipe.categoryDescription}</h6>
            <div
              style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <img
                src={URL + recipe.pictureLocation}
                style={{ width: "60%", height: "60%", alignContent: "center" }}
              />
            </div>
            <div dangerouslySetInnerHTML={{ __html: recipe.text }}></div>
            <ListGroup>
              {members.map((member) => (
                <ListGroup.Item className="lg-item" key={member.id}>
                  <div
                    style={{ display: "flex", justifyContent: "space-between" }}
                  >
                    <div>
                      Author: {member.firstName + " " + member.lastName}
                    </div>

                    <Button
                      variant="link"
                      onClick={() => toggleBiography(member.id)}
                      aria-controls={`bio-${member.id}`}
                      aria-expanded={openMemberId === member.id}
                    >
                      {openMemberId === member.id ? "Hide Bio" : "Show Bio"}
                    </Button>
                  </div>

                  <Collapse in={openMemberId === member.id}>
                    <div id={`bio-${member.id}`} style={{ marginTop: "10px" }}>
                      <strong>Biography:</strong>
                      <p>{member.bio}</p>
                    </div>
                  </Collapse>
                </ListGroup.Item>
              ))}
            </ListGroup>

            <CommentList recipeId={routeParams.id}></CommentList>
          </Container>
        </Col>
        <Col style={{ flex: "0 0 20%" }}>
          <br></br>
          <br></br>
          <br></br>
          <PlaceholderRight> </PlaceholderRight>
        </Col>
      </Row>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
