import { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";
import RecipeService from "../../services/RecipeService";
import CustomPagination from "../../components/CustomPagination";
import ArticleCard from "./ArticleCard";

import "./homeArticleList.css";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";

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

  const { showError, onHide } = useError();
  const { showLoading, hideLoading } = useLoading();

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(12);
  const [totalPages, setTotalPages] = useState(0);
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
    showLoading();
    const response = await RecipeService.paginate(params);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    const { items, pageCount } = response.data;
    setRecipes(items);
    setTotalPages(pageCount);
    hideLoading();
  }

  useEffect(() => {
    fetchRecipes();
  }, [pageNumber]);

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  return (
    <>
      <Container>
        <Row>
          {recipes ? (
            recipes.map((recipe, index) => (
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
            ))
          ) : (
            <Col>
              <h2 className="warning-header">
                No recipes found. Please try again later!
              </h2>
            </Col>
          )}
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
