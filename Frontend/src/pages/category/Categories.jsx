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

export default function Categories() {
  const [categories, setCategories] = useState();
  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrorModal, errors, hideError } = useError();
  const [entityToDelete, setEntityToDelete] = useState(null);

  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const navigate = useNavigate();

  async function fetchCategories() {
    showLoading();
    const response = await CategoriesService.readAll("category");
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    setCategories(response.data.items);
    hideLoading();
  }

  useEffect(() => {
    fetchCategories();
  }, []);

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
              <Col></Col>
              <Col></Col>
              <Col></Col>
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
