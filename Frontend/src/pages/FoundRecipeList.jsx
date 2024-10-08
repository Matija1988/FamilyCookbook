import { useEffect, useState } from "react";
import useLoading from "../hooks/useLoading";
import { Col, Container, Row } from "react-bootstrap";
import ArticleCard from "./homeComponents/ArticleCard";
import CustomPagination from "../components/CustomPagination";
import useError from "../hooks/useError";

export default function FoundRecipeList({ recipes }) {
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
