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

export default function CategoryCreate() {
  const navigate = useNavigate();
  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrorModal, errors, hideError } = useError();

  async function addCategory(entity) {
    showLoading();
    const response = await CategoriesService.create("category/create", entity);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    hideLoading();
    navigate(RouteNames.CATEGORIES);
  }

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    addCategory({
      name: information.get("Name"),
      description: information.get("Description"),
    });
  }

  function handleCancel() {
    navigate(RouteNames.CATEGORIES);
  }

  return (
    <>
      <Container className="primaryContainer">
        <h1>CATEGORY CREATE</h1>
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
      </Container>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
