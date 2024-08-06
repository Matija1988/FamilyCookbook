import { Container, Form } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import CustomButton from "../../components/CustomButton";
import { useNavigate, useParams } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import CategoriesService from "../../services/CategoriesService";
import { useEffect, useState } from "react";

export default function CategoryUpdate() {
  const [category, setCategory] = useState({});

  const routeParamas = useParams();
  const navigate = useNavigate();

  async function fetchCategory() {
    try {
      const response = await CategoriesService.getById(
        "category",
        routeParamas.id
      );
      if (response.ok) {
        setCategory(response.data);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchCategory();
  }, []);

  async function updateCategory(category) {
    try {
      const response = await CategoriesService.update(
        "category/update",
        routeParamas.id,
        category
      );
      if (response.ok) {
        navigate(RouteNames.CATEGORIES);
      }
    } catch (error) {
      alert(error.message);
    }
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
    </>
  );
}
