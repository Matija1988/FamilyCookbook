import { Container, Form, Row, Col } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import CustomButton from "../../components/CustomButton";
import { useNavigate, useParams } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import CategoriesService from "../../services/CategoriesService";
import { useEffect, useState } from "react";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import Sidebar from "../AdminPanel/Sidebar";

export default function CategoryUpdate() {
  const [category, setCategory] = useState({});

  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrorModal, errors, hideError } = useError();

  const routeParamas = useParams();
  const navigate = useNavigate();

  async function fetchCategory() {
    showLoading();

    const response = await CategoriesService.getById(
      "category",
      routeParamas.id
    );
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    setCategory(response.data);
    hideLoading();
  }

  useEffect(() => {
    fetchCategory();
  }, []);

  async function updateCategory(category) {
    showLoading();

    const response = await CategoriesService.update(
      "category/update",
      routeParamas.id,
      category
    );
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    hideLoading();
    navigate(RouteNames.CATEGORIES);
  }

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    updateCategory({
      name: information.get("Name"),
      description: information.get("Description"),
    });
  }

  function handleCancel() {
    navigate(RouteNames.CATEGORIES);
  }

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={8}>
          <Container className="primaryContainer">
            <h1>CATEGORY UPDATE</h1>
            <Form onSubmit={handleSubmit} className="createForm">
              <InputText atribute="Name" value={category.name}></InputText>
              <InputTextArea
                atribute="Description"
                rows={3}
                value={category.description}
              ></InputTextArea>
              <CustomButton
                label="SUBMIT"
                type="submit"
                variant="primary  m-3"
              ></CustomButton>
              <CustomButton
                label="CANCEL"
                onClick={handleCancel}
                variant="secondary  m-3"
              ></CustomButton>
            </Form>
          </Container>
        </Col>
      </Row>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
