import { useEffect, useState } from "react";
import {
  Button,
  Collapse,
  Container,
  Form,
  ListGroup,
  ListGroupItem,
} from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import RecipeService from "../../services/RecipeService";

import "./recipeDetails.css";
import CommentList from "../../components/CommentList";
import CommentCreate from "../../components/CommentCreate";
import ErrorModal from "../../components/ErrorModal";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";

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
    s;
  }

  useEffect(() => {
    fetchRecipe();
  }, []);

  const toggleBiography = (id) => {
    setOpenMemberId(openMemberId === id ? null : id);
  };

  return (
    <>
      <Container className="recDetails">
        <h2>{recipe.title}</h2>
        <h4>{recipe.subtitle}</h4>
        <h6 className="category">CATEGORY: {recipe.categoryName}</h6>
        <h6 className="category-desc">{recipe.categoryDescription}</h6>
        <img src={"https://localhost:7170/" + recipe.pictureLocation} />
        <div dangerouslySetInnerHTML={{ __html: recipe.text }}></div>
        <ListGroup>
          {members.map((member) => (
            <ListGroup.Item className="lg-item" key={member.id}>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <div>{member.firstName + " " + member.lastName}</div>

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
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
