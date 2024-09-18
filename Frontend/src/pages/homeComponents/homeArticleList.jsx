import { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";
import RecipeService from "../../services/RecipeService";
import CustomPagination from "../../components/CustomPagination";
import ArticleCard from "./ArticleCard";

export default function HomeArticleList() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    categoryName: "",
    members: [],
    pictureLocation: "",
  };

  const [recipes, setRecipes] = useState([]);
  const [error, setError] = useState("");

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(12);
  const [searchByActivityStatus, setSearchByActivityStatus] = useState(1);

  const getRequestParams = (pageSize, pageNumber, searchByActivityStatus) => {
    return {
      pageSize,
      pageNumber,
      searchByActivityStatus,
    };
  };

  async function fetchRecipes() {
    const params = getRequestParams(
      pageSize,
      pageNumber,
      searchByActivityStatus
    );
    try {
      const response = await RecipeService.paginate(params);
      if (response.ok) {
        setRecipes(response.data.items);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchRecipes();
  }, [pageNumber]);

  function handlePageChange() {}

  return (
    <>
      <Container>
        <Row>
          {recipes.map((recipe) => (
            <Col className="mb-2" xs={12} sm={5} md={4} lg={3} xl={2}>
              <ArticleCard key={recipe.id} recipe={recipe}></ArticleCard>
            </Col>
          ))}
        </Row>
        <CustomPagination
          pageNumber={pageNumber}
          pageSize={pageSize}
          handlePageChange={handlePageChange}
        ></CustomPagination>
      </Container>
    </>
  );
}
