import { useEffect, useState } from "react";
import RecipeService from "../../services/RecipeService";
import { Container, Table } from "react-bootstrap";
import CustomButton from "../../components/CustomButton";
import GenericTable from "../../components/GenericTable";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

export default function Recipe() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    text: "",
    categoryName: "",
    members: [],
  };

  const [recipes, setRecipes] = useState([recipeState]);

  const navigate = useNavigate();
  async function fetchRecipes() {
    try {
      const response = await RecipeService.readAll("recipe");
      if (response.ok) {
        setRecipes(response.data || []);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRecipes();
  }, []);

  function createRecipe() {
    navigate(RouteNames.RECIPES_CREATE);
  }

  function handleDelete() {}

  function handleUpdate() {}

  function goToDetails() {}

  return (
    <>
      <Container className="primaryContainer">
        <h1>RECIPES PAGE</h1>
        <CustomButton
          label="Create new"
          variant="primary"
          onClick={() => createRecipe()}
        ></CustomButton>

        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>Title</th>
              <th>Subtitle</th>
              <th>Category</th>
              <th>Author</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {recipes.length > 0 ? (
              recipes.map((entity) => (
                <tr key={entity.id}>
                  <td>{entity.title}</td>
                  <td>{entity.subtitle}</td>
                  <td>{entity.categoryName}</td>
                  <td>
                    <ul>
                      {entity.members && entity.members.length > 0 ? (
                        entity.members.map((author) => (
                          <li key={author.id}>
                            {author.firstName} {author.lastName}
                          </li>
                        ))
                      ) : (
                        <li>No authors available</li>
                      )}
                    </ul>
                  </td>
                  <td>
                    <CustomButton
                      variant="primary"
                      onClick={handleUpdate}
                      label="UPDATE"
                    ></CustomButton>
                    <CustomButton
                      variant="secondary"
                      onClick={goToDetails}
                      label="DETAILS"
                    ></CustomButton>
                    <CustomButton
                      variant="danger"
                      onClick={handleDelete}
                      label="DELETE"
                    ></CustomButton>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="4">Loading recipes...</td>
              </tr>
            )}
          </tbody>
        </Table>
      </Container>
    </>
  );
}
