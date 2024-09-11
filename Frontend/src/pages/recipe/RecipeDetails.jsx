import { useEffect, useState } from "react";
import { Container, Form } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import RecipeService from "../../services/RecipeService";

export default function RecipeDetails() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    text: "",
    categoryName: "",
    members: [],
  };
  const [recipe, setRecipe] = useState(recipeState);
  const [members, setMembers] = useState([]);
  const [category, setCategory] = useState("");

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

  return (
    <>
      <Container className="primaryContainer">
        <h2>{recipe.title}</h2>
        <h4>{recipe.subtitle}</h4>
        <div dangerouslySetInnerHTML={{ __html: recipe.text }}></div>
      </Container>
    </>
  );
}
