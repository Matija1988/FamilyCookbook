import { useEffect, useState } from "react";
import CategoriesService from "../../services/CategoriesService";
import { Col, Container, Row } from "react-bootstrap";
import CustomButton from "../../components/CustomButton";
import GenericTable from "../../components/GenericTable";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";
import DeleteModal from "../../components/DeleteModal";
import ErrorModal from "../../components/ErrorModal";
import Sidebar from "../AdminPanel/Sidebar";
import InputText from "../../components/InputText";
import ActivityStatusSelection from "../../components/ActivityStatusSelection";
import CustomPagination from "../../components/CustomPagination";
import PageSizeDropdown from "../../components/PageSizeDropdown";

export default function Categories() {
  const [categories, setCategories] = useState();
  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrorModal, errors, hideError } = useError();
  const [entityToDelete, setEntityToDelete] = useState(null);

  const [searchByName, setSearchByName] = useState("");
  const [activityStatus, setActivityStatus] = useState(1);

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [countPage, setCountPage] = useState(0);

  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const navigate = useNavigate();

  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 2, name: "Not active" },
  ];

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
    if (searchByName) {
      params["SearchByName"] = searchByName;
    }
    return params;
  };

  async function paginateCategory() {
    showLoading();
    const params = getRequestParams(pageSize, pageNumber, activityStatus);
    const response = await CategoriesService.paginate(
      "category/paging",
      params
    );
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    const { items, pageCount } = response.data;
    setCategories(items);
    setTotalPages(pageCount);
    hideLoading();
  }

  useEffect(() => {
    paginateCategory();
  }, [pageNumber, pageSize]);

  async function deleteCategory(category) {
    const response = await CategoriesService.setNotActive(
      "category/softDelete",
      category.id
    );
    if (!response.ok) {
      showError(response.data);
    }
    setShowDeleteModal(false);
    fetchCategories();
  }

  function updateCategory(category) {
    navigate(RouteNames.CATEGORIES_UPDATE.replace(":id", category.id));
  }

  function createCategory() {
    navigate(RouteNames.CATEGORIES_CREATE);
  }

  const onSearchByNameChange = (e) => {
    setSearchByName(e.target.value);
  };

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  const handlePageSizeChange = (e) => {
    setPageSize(e.target.value);
    setPageNumber(1);
  };

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={8}>
          <Container className="primaryContainer">
            <h1>CATEGORIES PAGE</h1>
            <Row>
              <Col>
                <CustomButton
                  label="Create new"
                  variant="primary"
                  onClick={() => createCategory()}
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
                <InputText
                  atribute="Search by name"
                  type="text"
                  value=""
                  onChange={onSearchByNameChange}
                ></InputText>
              </Col>
              <Col>
                <ActivityStatusSelection
                  atribute="Search by activity status"
                  entities={statusOptions || []}
                  onChanged={(e) => setActivityStatus(e.target.value)}
                  value={statusOptions.indexOf(1)}
                ></ActivityStatusSelection>
              </Col>
            </Row>
            <Row>
              <Col></Col>
              <Col></Col>
              <Col></Col>
              <Col>
                <CustomButton
                  label="Search"
                  onClick={paginateCategory}
                  className="search-btn"
                ></CustomButton>
              </Col>
            </Row>

            <GenericTable
              dataArray={categories}
              onDelete={(category) => (
                setEntityToDelete(category), setShowDeleteModal(true)
              )}
              onUpdate={updateCategory}
              cutRange={1}
              cutRangeForIsActiveStart={2}
              cutRangeForIsActiveEnd={3}
            ></GenericTable>
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
        handleDelete={deleteCategory}
        entity={entityToDelete}
      ></DeleteModal>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
