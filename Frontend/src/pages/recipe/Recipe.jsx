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
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [countPage, setCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [searchByTitle, setSearchByTitle] = useState("");

  const [entityId, setEntityId] = useState();
  const [activityStatus, setActivityStatus] = useState(1);

  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 2, name: "Not active" },
  ];

  const navigate = useNavigate();
  const [error, setError] = useState([]);
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
    return params;
  };

  async function paginateRecipes() {
    const params = getRequestParams(pageSize, pageNumber, activityStatus);
    try {
      const response = await RecipeService.paginate(params);

      const { items, pageCount } = response.data;

      setRecipes(items);
      setTotalPages(pageCount);
    } catch (e) {
      setError("Error fetching recipes " + e.message);
    }
  }

  // async function fetchRecipes() {
  //   try {
  //     const response = await RecipeService.readAll("recipe");
  //     if (response.ok) {
  //       setRecipes(response.data || []);
  //     }
  //   } catch (error) {
  //     alert(error.message);
  //   }
  // }

  useEffect(() => {
    paginateRecipes();
    // fetchRecipes();
  }, [pageNumber, pageSize]);

  function createRecipe() {
    navigate(RouteNames.RECIPES_CREATE);
  }

  async function handleDelete(id) {
    try {
      const response = await RecipeService.setNotActive("recipe/disable/" + id);
      if (response.ok) {
        fetchRecipes();
      }
    } catch (error) {
      alert(error.message);
    }
  }

  function goToDetails() {}

  const onSearchByTitleChange = (e) => {
    const titleSearch = e.target.value;
    setSearchByTitle(titleSearch);
  };

  const handlePageSizeChange = (e) => {
    setPageSize(e.target.value);
    setPageNumber(1);
  };

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  return (
    <>
      <Container className="primaryContainer">
        <h1>RECIPES PAGE</h1>
        <CustomButton
          label="Create new"
          variant="primary"
          onClick={() => createRecipe()}
        ></CustomButton>
        <Row>
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
        </Row>
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
                        entity.members.map((author, index) => (
                          <li key={index}>
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
                      onClick={() => {
                        navigate(
                          RouteNames.RECIPES_UPDATE.replace(":id", entity.id)
                        );
                      }}
                      label="UPDATE"
                    ></CustomButton>
                    <CustomButton
                      variant="secondary"
                      onClick={goToDetails}
                      label="DETAILS"
                    ></CustomButton>
                    <CustomButton
                      variant="danger"
                      onClick={() => (
                        setEntityId(entity.id), handleDelete(entity.id)
                      )}
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
        <CustomPagination
          pageNumber={pageNumber}
          totalPages={totalPages}
          handlePageChange={handlePageChange}
        ></CustomPagination>
      </Container>
    </>
  );
}
