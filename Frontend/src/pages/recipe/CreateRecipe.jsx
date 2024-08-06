import { Col, Container, Form, Row } from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import { useEffect, useState } from "react";
import CategoriesService from "../../services/CategoriesService";
import SelectionDropdown from "../../components/SelectionDropdown";
import { AsyncTypeahead } from "react-bootstrap-typeahead";

export default function CreateRecipe() {
  const [categories, setCategories] = useState([]);

  const [members, setMembers] = useState();
  const [foundMembers, setFoundMembers] = useState([]);

  const [searchCondition, setSearchCondition] = useState("");

  const [recipe, setRecipe] = useState({});

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

  async function fetchMembers() {
    const response = await MembersService.readAll("member");

    if (!response.ok) {
      alert("Error!");
      return;
    }

    setMembers(response.data.items);
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
            <Col>
              <AsyncTypeahead
                className="autocomplete"
                id="condition"
                emptyLabel="No result"
                searchText="Searching"
              ></AsyncTypeahead>
            </Col>
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
