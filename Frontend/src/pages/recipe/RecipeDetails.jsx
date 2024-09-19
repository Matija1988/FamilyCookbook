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

  const [openMemberId, setOpenMemberId] = useState(null);
  const [error, setError] = useState([]);
  const routeParams = useParams();
  const navigate = useNavigate();

  async function fetchRecipe() {
    try {
      const response = await RecipeService.getById("recipe", routeParams.id);
      if (response.ok) {
        setRecipe(response.data);
        setMembers(response.data.members);
        setCategory(response.data.categoryName);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRecipe();
  }, []);

  const toggleBiography = (id) => {
    setOpenMemberId(openMemberId === id ? null : id);
  };

  console.log("Picture location: " + recipe.pictureLocation);

  return (
    <>
      <Container className="recDetails">
        <h2>{recipe.title}</h2>
        <h4>{recipe.subtitle}</h4>
        <h6 className="category">CATEGORY: {recipe.categoryName}</h6>
        <h7 className="category-desc">{recipe.categoryDescription}</h7>
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
      </Container>
    </>
  );
}
