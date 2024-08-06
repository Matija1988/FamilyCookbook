import { Container, Form } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import CategoriesService from "../../services/CategoriesService";
import MembersService from "../../services/MembersService";

export default function CategoryCreate() {
  const navigate = useNavigate();

  async function addCategory(entity) {
    try {
      const response = await CategoriesService.create(
        "category/create",
        entity
      );
      if (response.ok) {
        navigate(RouteNames.CATEGORIES);
        return;
      }
    } catch (error) {
      alert(error.message);
    }
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
          <InputText atribute="Name" value=""></InputText>
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
    </>
  );
}
