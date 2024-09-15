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
import RichTextEditor from "../../components/RichTextEditor";
import UplaodPictureModal from "../../components/UploadPictureModal";
import { all } from "axios";

export default function CreateRecipe() {
  const [recipe, setRecipe] = useState({
    title: "",
    subtitle: "",
    text: "",
    categoryId: null,
    memberIds: [],
    pictureName: "",
    picture: null,
  });

  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setCategoryId] = useState();
  const [recipeText, setRecipeText] = useState("");
  const [members, setMembers] = useState([]);
  const [foundMembers, setFoundMembers] = useState([]);

  const [entity, setEntity] = useState({});
  const [showModal, setShowModal] = useState(false);

  const [uploadedPicture, setUploadedPicture] = useState(null);

  const [error, setError] = useState("");

  const maxPictureSize = 1 * 1024 * 1024;

  const quillRef = useRef(null);

  const typeaheadRef = useRef(null);

  const navigate = useNavigate();

  async function fetchCategories() {
    try {
      const response = await CategoriesService.readAll("category");
      if (response.ok) {
        setCategories(response.data.items);
      }
    } catch (error) {
      setError(error.message);
      alert(error);
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
      const response = await RecipeService.create("recipe/create", entity, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
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

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    const authorIds = recipe.memberIds.map((id) => id);

    postRecipe({
      title: information.get("Title"),
      subtitle: information.get("Subtitle"),
      text: recipe.text,
      categoryId: parseInt(selectedCategoryId),
      memberIds: authorIds,
      pictureName: information.get("Picture name"),
      picture: uploadedPicture,
    });
  }

  async function assignMemberToRecipe(member) {
    const updatedMembers = [...recipe.memberIds, member.id];
    setRecipe({ ...recipe, memberIds: updatedMembers });
    setFoundMembers([]);
  }

  function handleCancel() {
    navigate(RouteNames.RECIPES);
  }

  const handlePictureChange = (event) => {
    const file = event.target.files[0];

    console.log(file);
    const allowedTypes = ["image/jpeg", "image/jpg", "image/png"];

    if (file && !allowedTypes.includes(file.type)) {
      setError("Only JPEG, JPG and PNG files are allowed!!!");
      setUploadedPicture(null);
      return;
    }

    if (file && file.size > maxPictureSize) {
      setError(
        "Maximum file size is " + (maxPictureSize / (1024 * 1024) + " MB!!!")
      );
      setUploadedPicture(null);
      return;
    }

    setError("");

    setUploadedPicture(file);
    console.log("Picture ", uploadedPicture);
  };

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
                onChanged={(e) => setCategoryId(e.target.value)}
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
              </ListGroup>
            </Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Subtitle" value=""></InputText>
            </Col>
            <Col></Col>
          </Row>
          <Row>
            <Col>
              <InputText atribute="Picture name" value=""></InputText>
            </Col>
            <Col></Col>
          </Row>
          <Row>
            <Col>
              <div>
                <Form.Label>Upload image</Form.Label>
                <input type="file" onChange={handlePictureChange}></input>
                {error && <p style={{ color: "red" }}>{error}</p>}
              </div>
            </Col>
          </Row>

          <RichTextEditor
            value={recipe.text}
            setValue={(text) => setRecipe({ ...recipe, text })}
            ref={quillRef}
          />
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
