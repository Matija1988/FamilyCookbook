import { useEffect, useState } from "react";
import { Col, Container, Form, Row } from "react-bootstrap";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";
import RecipeService from "../../services/RecipeService";
import ArticleCard from "../homeComponents/ArticleCard";
import CustomPagination from "../../components/CustomPagination";
import { useParams } from "react-router-dom";

export default function ArticlesByCategory() {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    categoryName: "",
    members: [],
    pictureLocation: "",
  };

  const [recipes, setRecipes] = useState([]);

  const [pageSize, setPageSize] = useState(18);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [searchByActivityStatus, setSearchByActivityStatus] = useState(1);
  const { showError, showErrorModal, hideError, errors } = useError();
  const { showLoading, hideLoading } = useLoading();

  const routeParams = useParams();
  const searchByCategory = routeParams.id;

  const getRequestParams = (
    pageSize,
    pageNumber,
    searchByActivityStatus,
    searchByCategory = routeParams.categoryName
  ) => {
    return { pageSize, pageNumber, searchByActivityStatus, searchByCategory };
  };

  async function fetchRecipes() {
    const params = getRequestParams(
      pageSize,
      pageNumber,
      searchByActivityStatus,
      searchByCategory
    );

    showLoading();
    const response = await RecipeService.paginate("recipe/paginate", params);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    const { items, pageCount } = response.data;
    setRecipes(items);
    setTotalPages(pageCount);
    hideLoading();
  }
  useEffect(() => {
    fetchRecipes();
  }, [routeParams.id, pageNumber]);

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  return (
    <>
      <Container className="cat-cont">
        <Row>
          {recipes.map((recipe, index) => (
            <Col
              key={index}
              className="mb-2"
              xs={12}
              sm={5}
              md={4}
              lg={3}
              xl={2}
            >
              <ArticleCard key={recipe.id} recipe={recipe}></ArticleCard>
            </Col>
          ))}
        </Row>
        <CustomPagination
          pageNumber={pageNumber}
          pageSize={pageSize}
          totalPages={totalPages}
          handlePageChange={handlePageChange}
          className="home-article-pagination"
        ></CustomPagination>
      </Container>
    </>
  );
}
