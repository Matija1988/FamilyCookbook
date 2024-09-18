import { useState } from "react";
import { Container } from "react-bootstrap";
import RecipeService from "../../services/RecipeService";

export default function homeArticleList() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    categoryName: "",
    members: [],
    pictureLocation: "",
  };

  const [recipes, setRecipes] = useState();
  const [error, setError] = useState("");

  const getRequestParams = (PageSize, PageNumber, SearchByActivityStatus) => {
    return {
      PageSize,
      PageNumber,
      SearchByActivityStatus,
    };
  };

  async function fetchRecipes() {
    try {
      const response = await RecipeService.paginate(params);
    } catch (error) {
      alert(error.message);
    }
  }

  const pageSize = 10;

  return (
    <>
      <Container></Container>
    </>
  );
}
