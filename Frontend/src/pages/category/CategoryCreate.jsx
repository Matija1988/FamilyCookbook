import { Container, Form } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import CategoriesService from "../../services/CategoriesService";
import MembersService from "../../services/MembersService";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import { useState } from "react";

export default function CategoryCreate() {
  const initialState = {
    name: "",
    description: "",
  };

  const navigate = useNavigate();
  const { showLoading, hideLoading } = useLoading();
  const [category, setCategory] = useState(initialState);
  const { showError, showErrorModal, errors, hideError } = useError();
  const [submited, setSubmited] = useState(false);

  async function addCategory(entity) {
    showLoading();
    const response = await CategoriesService.create("category/create", entity);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    hideLoading();
    setSubmited(true);
  }

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    category.name = information.get("Name");
    category.description = information.get("Description");
    addCategory({
      name: category.name,
      description: category.description,
    });
  }

  function handleCancel() {
    navigate(RouteNames.CATEGORIES);
  }

  const newCategory = () => {
    setCategory(initialState);
    setSubmited(false);
  };

  const handleReturn = () => {
    navigate(RouteNames.CATEGORIES);
  };

  return (
    <>
      <Container className="primaryContainer">
        <h1>CATEGORY CREATE</h1>

        {submited ? (
          <div>
            <h4>Category submitted successfully</h4>
            <CustomButton
              variant="primary"
              label="ADD ANOTHER"
              onClick={newCategory}
            >
              ADD ANTOHER
            </CustomButton>
            <CustomButton
              label="RETURN"
              onClick={handleReturn}
              variant="secondary"
              className="btn-secondary"
            ></CustomButton>
          </div>
        ) : (
          <Form onSubmit={handleSubmit} className="createForm">
            <InputText atribute="Name" value="" required={true}></InputText>
            <InputTextArea
              atribute="Description"
              rows={3}
              value=""
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
        )}
      </Container>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
