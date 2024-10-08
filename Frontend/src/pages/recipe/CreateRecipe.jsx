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
import ImageGallery from "../../components/ImageGallery";
import PictureService from "../../services/PictureService";
import { httpService } from "../../services/HttpService";

import "./createForm.css";
import Sidebar from "../AdminPanel/Sidebar";
import TagsService from "../../services/TagsService";
import useError from "../../hooks/useError";

export default function CreateRecipe() {
  const [recipe, setRecipe] = useState({
    title: "",
    subtitle: "",
    text: "",
    categoryId: null,
    memberIds: [],
    pictureName: "",
    tagIds: [],
  });

  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setCategoryId] = useState();
  const [recipeText, setRecipeText] = useState("");
  const [members, setMembers] = useState([]);
  const [foundMembers, setFoundMembers] = useState([]);

  const [pictureName, setPictureName] = useState("");
  const [uploadedPicture, setUploadedPicture] = useState(null);

  const [tags, setTags] = useState([]);
  const [foundTags, setFoundTags] = useState([]);

  const [error, setError] = useState("");

  const { showError, showErrorModal, errors, hideError } = useError();

  const [imageFromGallery, setImageFromGallery] = useState(null);

  const maxPictureSize = 1 * 1024 * 1024;

  const quillRef = useRef(null);

  const typeaheadRef = useRef(null);
  const tagTypeaheadRef = useRef(null);

  const navigate = useNavigate();

  const URL = "https://localhost:7170/";

  const [isImageGalleryOpen, setIsImageGalleryOpen] = useState(false);

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

  async function tagSearchCondition(input) {
    const response = await TagsService.getByText(input);
    if (!response.ok) {
      showError(response.data);
    }
    setFoundTags(response.data);
  }

  async function fetchMembers() {
    const response = await MembersService.readAll("member");
    if (!response.ok) {
      alert("Error!");
      return;
    }
    setMembers(response.data.items);
  }

  async function fetchTags() {
    const response = await TagsService.readAll("tag");
    if (!response.ok) {
      showError();
      return;
    }
    setTags(response.data.items);
  }

  async function postRecipe(entity, isMultiPart = false) {
    try {
      const response = await RecipeService.create(
        "recipe/create",
        entity,
        isMultiPart
      );
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
    fetchTags();
  }, []);

  function handleSubmit(e) {
    e.preventDefault();

    const formData = new FormData(e.target);

    recipe.title = formData.get("Title");
    recipe.subtitle = formData.get("Subtitle");

    const information = new FormData();

    const authorIds = recipe.memberIds.map((id) => id);

    const postTagIds = recipe.tagIds.map((id) => id);

    if (authorIds.length == 0) {
      alert("Plsease select a member as the author");
      return;
    }

    postRecipe({
      title: e.target.Title.value,
      subtitle: e.target.Subtitle.value,
      text: recipe.text,
      categoryId: parseInt(selectedCategoryId),
      memberIds: authorIds,
      pictureName: pictureName,
      pictureBlob: uploadedPicture,
      tagIds: postTagIds,
    });
  }

  async function assignMemberToRecipe(member) {
    const updatedMembers = [...recipe.memberIds, member.id];
    setRecipe({ ...recipe, memberIds: updatedMembers });
    setFoundMembers([]);
  }

  async function assignTagToRecipe(tag) {
    const updatedTags = [...recipe.tagIds, tag.id];
    setRecipe({ ...recipe, tagIds: updatedTags });
    setFoundTags([]);
  }

  function handleCancel() {
    navigate(RouteNames.RECIPES);
  }

  const handlePictureChange = (event) => {
    const file = event.target.files[0];
    setPictureName(file.name);
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

    const reader = new FileReader();

    reader.onloadend = () => {
      setUploadedPicture(reader.result);
    };

    reader.readAsDataURL(file);
    console.log("Picture: ", uploadedPicture);
  };

  const setMainImage = async (image) => {
    setPictureName(image.name);
    console.log("Image name " + image.name);
    console.log("Image location " + image.location);
    setImageFromGallery(image);
  };

  const openImageGallery = () => {
    setIsImageGalleryOpen(true);
  };

  return (
    <>
      <Row>
        <Col xs={12} md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col xs={12} md={8}>
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
                    labelKey={(member) =>
                      `${member.firstName} ${member.lastName}`
                    }
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
                <Col>
                  <Form.Label>Search existing tags</Form.Label>
                  <AsyncTypeahead
                    className="autocomplete"
                    id="tagCondition"
                    emptyLabel="No tags selected!"
                    searchText="Searching..."
                    labelKey={(tag) => `${tag.text}`}
                    minLength={3}
                    options={foundTags}
                    onSearch={tagSearchCondition}
                    renderMenuItemChildren={(tag) => (
                      <>
                        <span
                          key={tag.id}
                          onClick={() => assignTagToRecipe(tag)}
                        >
                          {tag.text}
                        </span>
                      </>
                    )}
                    ref={tagTypeaheadRef}
                  ></AsyncTypeahead>
                </Col>
              </Row>
              <Row>
                <Col>
                  <InputText
                    atribute="Title"
                    value=""
                    required={true}
                  ></InputText>
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
                <Col>
                  <h5 className="mt-3">Selected tags: </h5>
                  <ListGroup>
                    {recipe.tagIds.map((id) => {
                      const ta = tags.find((ta) => ta.id === id);
                      if (ta) {
                        return (
                          <ListGroup.Item key={ta.id}>{ta.text}</ListGroup.Item>
                        );
                      }
                    })}
                  </ListGroup>
                </Col>
              </Row>
              <Row>
                <Col>
                  <InputText
                    atribute="Subtitle"
                    value=""
                    required={true}
                  ></InputText>
                </Col>
                <Col></Col>
                <Col></Col>
              </Row>

              <Row>
                <Col>
                  <Form.Label>Upload image</Form.Label>
                </Col>
                <Col>Select image from galley</Col>
                <Col></Col>
              </Row>
              <Row>
                <Col>
                  <div>
                    <input
                      type="file"
                      onChange={(e) => handlePictureChange(e)}
                    ></input>
                    {error && <p style={{ color: "red" }}>{error}</p>}
                  </div>
                </Col>
                <Col>
                  <CustomButton
                    label="Images"
                    type="button"
                    onClick={() => setIsImageGalleryOpen(true)}
                  ></CustomButton>
                </Col>
                <Col></Col>
              </Row>
              <Row>
                <Col>
                  <img src={uploadedPicture} style={{ width: "300px" }}></img>
                  {imageFromGallery ? (
                    <img
                      src={URL + imageFromGallery.location}
                      style={{ width: "300px" }}
                    ></img>
                  ) : (
                    <div></div>
                  )}
                </Col>
              </Row>
              <RichTextEditor
                value={recipe.text}
                setValue={(text) => setRecipe({ ...recipe, text })}
                ref={quillRef}
                className="ql-editor"
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
            <ImageGallery
              isOpen={isImageGalleryOpen}
              closeModal={() => setIsImageGalleryOpen(false)}
              setMainImage={setMainImage}
            ></ImageGallery>
          </Container>
        </Col>
        <Col></Col>
      </Row>
    </>
  );
}
