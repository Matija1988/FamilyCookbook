import { useEffect, useState } from "react";
import RecipeService from "../../services/RecipeService";
import { Col, Container, Row, Table } from "react-bootstrap";
import CustomButton from "../../components/CustomButton";
import GenericTable from "../../components/GenericTable";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import GenericInputs from "../../components/GenericInputs";
import PageSizeDropdown from "../../components/PageSizeDropdown";
import CustomPagination from "../../components/CustomPagination";
import ActivityStatusSelection from "../../components/ActivityStatusSelection";
import SelectionDropdown from "../../components/SelectionDropdown";
import CategoriesService from "../../services/CategoriesService";
import RecipeTable from "./components/RecipeTable";

import "./createForm.css";
import Sidebar from "../AdminPanel/Sidebar";
import DeleteModal from "../../components/DeleteModal";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";

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
  const [categories, setCategories] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [countPage, setCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [searchByTitle, setSearchByTitle] = useState("");
  const [searchBySubtitle, setSearchBySubtitle] = useState("");
  const [searchByCategoryId, setSearchByCategoryId] = useState();
  const [searchByFirstName, setSearchByFirstName] = useState("");
  const [searchByLastName, setSearchByLastName] = useState("");
  const [entityId, setEntityId] = useState();
  const [activityStatus, setActivityStatus] = useState(1);
  const [entityToDelete, setEntityToDelete] = useState(null);

  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const { showError, showErrorModal, errors, hideError } = useError();

  const { showLoading, hideLoading } = useLoading();

  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 2, name: "Not active" },
  ];

  const navigate = useNavigate();
  const getRequestParams = (pageSize, pageNumber, activityStatus) => {
    let params = {};

    if (pageSize) {
      params["PageSize"] = pageSize;
    }
    if (pageNumber) {
      params["PageNumber"] = pageNumber;
    }
    if (activityStatus) {
      params["SearchByActivityStatus"] = activityStatus;
    }
    if (searchByTitle) {
      params["SearchByTitle"] = searchByTitle;
    }
    if (searchBySubtitle) {
      params["SearchBySubtitle"] = searchBySubtitle;
    }
    if (searchByCategoryId) {
      params["SearchByCategory"] = searchByCategoryId;
    }
    if (searchByFirstName) {
      params["SearchByAuthorName"] = searchByFirstName;
    }
    if (searchByLastName) {
      params["SearchByAutorSurname"] = searchByLastName;
    }
    return params;
  };

  async function paginateRecipes() {
    showLoading();
    const params = getRequestParams(pageSize, pageNumber, activityStatus);
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

  async function fetchCategories() {
    const response = await CategoriesService.readAll("category");
    if (!response.ok) {
      showError(response.data);
    }
    setCategories(response.data.items);
  }

  useEffect(() => {
    paginateRecipes();
    fetchCategories();
  }, [pageNumber, pageSize]);

  function createRecipe() {
    navigate(RouteNames.RECIPES_CREATE);
  }

  async function handleDelete(recipe) {
    try {
      const response = await RecipeService.setNotActive(
        "recipe/disable",
        recipe.id
      );
      if (response.ok) {
        paginateRecipes();
      }
    } catch (error) {
      alert(error.message);
    }
  }

  const onSearchByTitleChange = (e) => {
    const titleSearch = e.target.value;
    setSearchByTitle(titleSearch);
  };

  const onSearchBySubtitleChange = (e) => {
    const subtitleSearch = e.target.value;
    setSearchBySubtitle(subtitleSearch);
  };

  const handlePageSizeChange = (e) => {
    setPageSize(e.target.value);
    setPageNumber(1);
  };

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  const onAuthorNameChange = (e) => {
    setSearchByFirstName(e.target.value);
  };

  const onAuthorSurnameChange = (e) => {
    setSearchByLastName(e.target.value);
  };

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={8}>
          <Container className="primaryContainer">
            <h1>RECIPES PAGE</h1>
            <Row>
              <Col>
                <CustomButton
                  label="Create new"
                  variant="primary"
                  onClick={() => createRecipe()}
                  className="create-new-btn"
                ></CustomButton>
              </Col>
              <Col>
                <PageSizeDropdown
                  onChanged={handlePageSizeChange}
                  initValue={pageSize}
                ></PageSizeDropdown>
              </Col>
              <Col>
                <GenericInputs
                  atribute="Search by title"
                  type="text"
                  value=""
                  onChange={onSearchByTitleChange}
                ></GenericInputs>
              </Col>
              <Col>
                <GenericInputs
                  atribute="Search by subtitle"
                  type="text"
                  value=""
                  onChange={onSearchBySubtitleChange}
                ></GenericInputs>
              </Col>
            </Row>
            <Row>
              <Col>
                <ActivityStatusSelection
                  entities={statusOptions || []}
                  value={statusOptions.indexOf(1)}
                  onChanged={(e) => setActivityStatus(e.target.value)}
                  atribute="Search by activity status"
                ></ActivityStatusSelection>
              </Col>
              <Col>
                <SelectionDropdown
                  atribute="Search by category"
                  entities={categories || []}
                  onChanged={(e) => setSearchByCategoryId(e.target.value)}
                ></SelectionDropdown>
              </Col>
              <Col>
                <GenericInputs
                  atribute="Search by author name"
                  type="text"
                  value=""
                  onChange={onAuthorNameChange}
                ></GenericInputs>
              </Col>
              <Col>
                <GenericInputs
                  atribute="Search by author last name"
                  type="text"
                  value=""
                  onChange={onAuthorSurnameChange}
                ></GenericInputs>
              </Col>
            </Row>
            <Row>
              <Col></Col>
              <Col></Col>
              <Col></Col>
              <Col>
                <CustomButton
                  label="Search"
                  onClick={paginateRecipes}
                  className="search-btn"
                ></CustomButton>
              </Col>
            </Row>
            <RecipeTable
              recipes={recipes}
              handleDelete={(recipe) => {
                setEntityToDelete(recipe), setShowDeleteModal(true);
              }}
            ></RecipeTable>
            <CustomPagination
              pageNumber={pageNumber}
              totalPages={totalPages}
              handlePageChange={handlePageChange}
            ></CustomPagination>
          </Container>
        </Col>
      </Row>
      <DeleteModal
        show={showDeleteModal}
        handleClose={() => setShowDeleteModal(false)}
        entity={entityToDelete}
        handleDelete={handleDelete}
      ></DeleteModal>
    </>
  );
}
