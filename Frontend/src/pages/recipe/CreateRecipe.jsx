import { Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import { useEffect, useState } from "react";
import CategoriesService from "../../services/CategoriesService";
import SelectionDropdown from "../../components/SelectionDropdown";

export default function CreateRecipe() {
  const [categories, setCategories] = useState([]);

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

  function handleSelect() {}

  function handleSubmit() {}

  return (
    <>
      <Container className="primaryContainer">
        <h1>Create recipe</h1>
        <Form onSubmit={handleSubmit} className="createForm">
          <Row>
            <Col>
              <SelectionDropdown
                atribute="Select category"
                entities={categories}
                onSelect={handleSelect}
              ></SelectionDropdown>
            </Col>
            <Col></Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Title" value=""></InputText>
            </Col>
            <Col></Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Subtitle" value=""></InputText>
            </Col>
            <Col></Col>
          </Row>
          <InputTextArea atribute="Text" rows={12} value=""></InputTextArea>
        </Form>
      </Container>
    </>
  );
}
