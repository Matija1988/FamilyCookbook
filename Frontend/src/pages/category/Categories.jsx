import { useEffect, useState } from "react";
import CategoriesService from "../../services/CategoriesService";
import { Container } from "react-bootstrap";
import CustomButton from "../../components/CustomButton";
import GenericTable from "../../components/GenericTable";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

export default function Categories() {
  const [categories, setCategories] = useState();

  const navigate = useNavigate();

  async function fetchCategories() {
    try {
      const response = await CategoriesService.readAll("category");
      if (response.ok) {
        setCategories(response.data.items);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchCategories();
  }, []);

  async function deleteCategory(category) {
    const response = await CategoriesService.setNotActive(
      "category/softDelete/" + category.id
    );
    if (response.ok) {
      fetchCategories();
    }
  }

  function updateCategory(category) {
    navigate(RouteNames.CATEGORIES_UPDATE.replace(":id", category.id));
  }

  function createCategory() {
    navigate(RouteNames.CATEGORIES_CREATE);
  }

  return (
    <>
      <Container className="primaryContainer">
        <h1>CATEGORIES PAGE</h1>
        <CustomButton
          label="Create new"
          variant="primary"
          onClick={() => createCategory()}
        ></CustomButton>

        <GenericTable
          dataArray={categories}
          onDelete={deleteCategory}
          onUpdate={updateCategory}
          cutRange={1}
          cutRangeForIsActiveStart={2}
          cutRangeForIsActiveEnd={3}
        ></GenericTable>
      </Container>
    </>
  );
}
