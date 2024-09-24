import { useState } from "react";
import { Container } from "react-bootstrap";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";

export default function ArticlesByCategory({ entity }) {
  const recipeState = {
    id: "",
    title: "",
    subtitle: "",
    categoryName: "",
    members: [],
    pictureLocation: "",
  };

  const [recipes, setRecipes] = useState();

  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageCount, setPageCount] = useState(0);
  const [searchByActivityStatus, setSearchByActivityStatus] = useState(1);
  const [searchByCategory, setSearchByCategory] = useState(entity);

  const { showError, showErrorModal, hideError, errors } = useError();
  const { showLoading, hideLoading } = useLoading();

  const getRequestParams = (
    pageSize,
    pageNumber,
    searchByActivityStatus,
    searchByCategory
  ) => {
    return { pageSize, pageNumber, searchByActivityStatus, searchByCategory };
  };

  return (
    <>
      <Container></Container>
    </>
  );
}
