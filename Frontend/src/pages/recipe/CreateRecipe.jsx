import {
  Col,
  Container,
  Form,
  ListGroup,
  ListGroupItem,
  Row,
} from "react-bootstrap";
import InputText from "../../components/InputText";
import InputTextArea from "../../components/InputTextArea";
import { useEffect, useRef, useState } from "react";
import CategoriesService from "../../services/CategoriesService";
import SelectionDropdown from "../../components/SelectionDropdown";
import { AsyncTypeahead, TypeaheadRef } from "react-bootstrap-typeahead";
import MembersService from "../../services/MembersService";
import RecipeService from "../../services/RecipeService";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import CustomButton from "../../components/CustomButton";

export default function CreateRecipe() {
  const [recipe, setRecipe] = useState({
    title: "",
    subtitle: "",
    text: "",
    categoryId: null,
    memberIds: [],
  });

  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setCategoryId] = useState();

  const [members, setMembers] = useState([]);
  const [foundMembers, setFoundMembers] = useState([]);

  const [searchCondition, setSearchCondition] = useState("");
  const typeaheadRef = useRef(null);

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

  async function SearchByCondition(input) {
    try {
      const response = await MembersService.searchMemberByCondition(input);
      if (response.ok) {
        setFoundMembers(response.data.items);
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

  async function postRecipe(entity) {
    try {
      const response = await RecipeService.create("recipe/create", entity);
      if (response.ok) {
        navigate(RouteNames.RECIPES);
      }
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fetchCategories();
    fetchMembers();
  }, []);

  function handleSelect(selectedCategory) {
    setCategoryId(selectedCategory.id);
    console.log(selectedCategory);
  }

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    const authorIds = recipe.memberIds.map((id) => id);

    postRecipe({
      title: information.get("Title"),
      subtitle: information.get("Subtitle"),
      text: information.get("Text"),
      categoryId: parseInt(selectedCategoryId),
      memberIds: authorIds,
    });
  }

  function assignMemberToRecipe(member) {
    const updatedMembers = [...recipe.memberIds, member.id];
    setRecipe({ ...recipe, memberIds: updatedMembers });
    setFoundMembers([]);
  }

  function handleCancel() {
    navigate(RouteNames.RECIPES);
  }

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
                onSelect={(r) => setCategoryId(r.target.value)}
              ></SelectionDropdown>
            </Col>
            <Col>
              <Form.Label>Search members by first and last name</Form.Label>
              <AsyncTypeahead
                className="autocomplete"
                id="condition"
                emptyLabel="No result!"
                searchText="Searching...."
                labelKey={(member) => `${member.firstName} ${member.lastName}`}
                minLength={3}
                options={foundMembers}
                onSearch={SearchByCondition}
                renderMenuItemChildren={(member) => (
                  <>
                    <span
                      key={member.id}
                      onClick={() => assignMemberToRecipe(member)}
                    >
                      {member.firstName} {member.lastName}
                    </span>
                  </>
                )}
                //onChange={() => assignMemberToRecipe()}
                ref={typeaheadRef}
              ></AsyncTypeahead>
            </Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Title" value=""></InputText>
            </Col>
            <Col>
              <h5 className="mt-3">Selected Members:</h5>
              <ListGroup>
                {recipe.memberIds.map((id) => {
                  const member = members.find((m) => m.id === id);
                  if (member) {
                    return (
                      <ListGroup.Item key={member.id}>
                        {member.firstName} {member.lastName}
                      </ListGroup.Item>
                    );
                  }
                  return null;
                })}
              </ListGroup>{" "}
            </Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Subtitle" value=""></InputText>
            </Col>
            <Col></Col>
          </Row>
          <InputTextArea atribute="Text" rows={12} value=""></InputTextArea>
          <Row>
            <Col>
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
            </Col>
          </Row>
        </Form>
      </Container>
    </>
  );
}
