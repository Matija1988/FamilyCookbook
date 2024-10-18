import { Col, Container, Form, ListGroup, Row, Table } from "react-bootstrap";
import InputText from "../../components/InputText";
import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import RecipeService from "../../services/RecipeService";
import InputTextArea from "../../components/InputTextArea";
import CategoriesService from "../../services/CategoriesService";
import SelectionDropdown from "../../components/SelectionDropdown";
import CustomButton from "../../components/CustomButton";
import { App, RouteNames } from "../../constants/constants";
import MembersService from "../../services/MembersService";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import RichTextEditor from "../../components/RichTextEditor";
import PictureService from "../../services/PictureService";
import ImageGallery from "../../components/ImageGallery";
import Sidebar from "../AdminPanel/Sidebar";
import TagsService from "../../services/TagsService";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";
import ErrorModal from "../../components/ErrorModal";
import PaginationForTags from "./components/PaginationForTags";
import TagList from "./components/TagList";

export default function UpdateRecipe() {
  const initialState = {
    title: "",
    subtitle: "",
    text: "",
    categoryId: null,
    members: [
      {
        id: "",
        firstName: "",
        lastName: "",
        bio: "",
      },
    ],
    pictureName: "",
    pictureLocation: "",
    pictureId: "",
    tags: [{ id: "", text: "" }],
  };
  const [recipe, setRecipe] = useState(initialState);
  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState("");
  const [members, setMembers] = useState([]);
  const [foundMembers, setFoundMembers] = useState([]);
  const [foundTags, setFoundTags] = useState([]);
  const { showError, errors, hideError, showErrorModal } = useError();
  const { showLoading, hideLoading } = useLoading();
  const [oldPictureName, setOldPictureName] = useState("");
  const [title, setTitle] = useState("");
  const [subtitle, setSubtitle] = useState("");
  const [tags, setTags] = useState([]);
  const [recipeCategory, setRecipeCategory] = useState(null);

  const [newPicture, setNewPicture] = useState(null);
  const [newPictureName, setNewPictureName] = useState("");
  const [imageFromGallery, setImageFromGallery] = useState(null);
  const quillRef = useRef(null);

  const [isImageGalleryOpen, setIsImageGalleryOpen] = useState(false);

  const maxPictureSize = 1 * 1024 * 1024;

  const typeaheadRef = useRef(null);
  const tagTypeaheadRef = useRef(null);

  const routeParams = useParams();
  const navigate = useNavigate();
  const URL = App.URL;

  const [error, setError] = useState("");

  async function fetchAllData() {
    showLoading();
    const [recipeResponse, categoriesResponse] = await Promise.all([
      RecipeService.getById("recipe", routeParams.id),
      CategoriesService.readAll("category"),
    ]);

    if (!recipeResponse.ok || !categoriesResponse.ok) {
      hideLoading();
      showError(response.data);
    }

    setRecipe(recipeResponse.data);
    setTitle(recipeResponse.data.title);
    setSubtitle(recipeResponse.data.subtitle);
    setMembers(recipeResponse.data.members);
    setTags(recipeResponse.data.tags);
    setRecipeCategory(recipeResponse.data.categoryId);
    setCategories(categoriesResponse.data.items);
    setOldPictureName(recipeResponse.data.pictureName);
    setImageFromGallery({ location: recipeResponse.data.pictureLocation });
    hideLoading();
  }

  useEffect(() => {
    fetchAllData();
  }, []);

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

  function handleCancel() {
    navigate(RouteNames.RECIPES);
  }

  async function removeMemberFromRecipe(member) {
    try {
      const response = await RecipeService.removeMemberFromRecipe(
        member.id,
        routeParams.id
      );
      if (response.ok) {
        location.reload();
      }
    } catch (error) {
      alert(error.message);
    }
  }

  async function UpdateRecipe(entity) {
    try {
      const response = await RecipeService.update(
        "recipe/update",
        routeParams.id,
        entity
      );
      if (response.ok) {
        navigate(RouteNames.RECIPES);
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

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    const authorIds = members.map((member) => member.id);

    const postTagIds = tags.map((tag) => tag.id);

    if (!newPictureName || newPictureName.trim() === "") {
      setNewPictureName(oldPictureName);
    }

    UpdateRecipe({
      title: information.get("Title"),
      subtitle: information.get("Subtitle"),
      text: recipe.text,
      categoryId: parseInt(selectedCategoryId) || recipeCategory,
      memberIds: authorIds,
      tagIds: postTagIds,
      imageName: newPictureName || oldPictureName,
      imageBlob: newPicture || null,
    });
  }

  function assignMemberToRecipe(member) {
    if (!recipe.members.some((m) => m.id === member.id)) {
      const updatedMembers = [...members, member];
      setMembers(updatedMembers);
    }
    setFoundMembers([]);
  }

  const handlePictureChange = (event) => {
    const file = event.target.files[0];
    setNewPictureName(file.name);

    const allowedTypes = ["image/jpeg", "image/jpg", "image/png"];

    if (file && !allowedTypes.includes(file.type)) {
      setError("Only JPEG, JPG and PNG files are allowed");
      setNewPicture(null);
      return;
    }
    if (file && file.size > maxPictureSize) {
      setError(
        "Maximum file size is " + maxPictureSize / (1024 * 1024) + " MB!!!"
      );
      setNewPicture(null);
      return;
    }

    const reader = new FileReader();
    reader.onloadend = () => {
      setNewPicture(reader.result);
    };
    reader.readAsDataURL(file);
  };

  const setMainImage = async (image) => {
    setNewPictureName(image.name);
    setImageFromGallery(image);
  };

  const openImageModal = () => {
    setIsImageGalleryOpen(true);
  };
  const closeImageModal = () => {
    setIsImageGalleryOpen(false);
  };

  const assignTagToRecipe = (tag) => {
    if (!recipe.tags.some((t) => t.id === tag.id)) {
      const updatedTags = [...tags, tag];
      setTags(updatedTags);
    }
    setFoundTags([]);
  };

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={8}>
          <Container className="primaryContainer">
            <h1>UPDATE RECIPE</h1>
            <Form onSubmit={handleSubmit}>
              <Row>
                <Col>
                  <SelectionDropdown
                    atribute="Select category"
                    initValue={recipeCategory}
                    entities={categories}
                    onChanged={(e) => {
                      setSelectedCategoryId(e.target.value);
                    }}
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
                      <span
                        key={tag.id}
                        onClick={() => assignTagToRecipe(tag)}
                        style={{ cursor: "pointer" }}
                      >
                        {tag.text}
                      </span>
                    )}
                    ref={tagTypeaheadRef}
                  ></AsyncTypeahead>
                </Col>
              </Row>
              <Row>
                <Col>
                  <InputText atribute="Title" value={title}></InputText>
                </Col>
                <Col>
                  <Table>
                    <thead>
                      <tr>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {members && members.length > 0 ? (
                        members.map((member) => {
                          if (member) {
                            return (
                              <tr key={member.id}>
                                <td>{member.firstName}</td>
                                <td>{member.lastName}</td>
                                <td>
                                  <CustomButton
                                    label="REMOVE"
                                    onClick={() =>
                                      removeMemberFromRecipe(member)
                                    }
                                    type="button"
                                  ></CustomButton>
                                </td>
                              </tr>
                            );
                          }
                          return null;
                        })
                      ) : (
                        <tr>
                          <td colSpan="3">
                            No members assigned to this recipe.
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </Table>
                </Col>
                <Col>
                  <h5 className="mt-3">Recipe tags:</h5>
                  <ListGroup>
                    {tags && tags.length > 0 ? (
                      tags.map((tag) => {
                        return (
                          <ListGroup.Item key={tag.id}>
                            {tag.text}
                          </ListGroup.Item>
                        );
                      })
                    ) : (
                      <ListGroup.Item>
                        No tags assigned to this recipe
                      </ListGroup.Item>
                    )}
                  </ListGroup>
                </Col>
              </Row>
              <Row>
                <Col>
                  <InputText atribute="Subtitle" value={subtitle}></InputText>
                </Col>
                <Col></Col>
                <Col></Col>
              </Row>
              <Row>
                <Row>
                  <Col>
                    <Form.Label>Picture</Form.Label>
                  </Col>
                </Row>
                <Col>
                  <div>
                    <img
                      src={newPicture}
                      style={{ width: "300px" }}
                      alt="Existing picture"
                    ></img>
                    {/* <div>
                      {newPicture ? (
                        <img
                          src={newPicture}
                          alt="New uploaded picture"
                          style={{ width: "300px" }}
                        ></img>
                      ) : null}
                    </div> */}
                    {imageFromGallery ? (
                      <img
                        src={URL + imageFromGallery.location}
                        style={{ width: "300px" }}
                        alt="Existing picture"
                      ></img>
                    ) : (
                      <div>
                        <p>nothing</p>
                      </div>
                    )}
                  </div>
                </Col>
                <Row>
                  <Col>
                    <Form.Label>Set new picture</Form.Label>
                  </Col>
                  <Col>
                    <Form.Label>Select image from gallery</Form.Label>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <div>
                      <input
                        type="file"
                        onChange={(e) => handlePictureChange(e)}
                      ></input>
                      {error && <p style={{ color: red }}>{error}</p>}
                    </div>
                  </Col>
                  <Col>
                    <CustomButton
                      label="Images"
                      onClick={openImageModal}
                      type="button"
                    ></CustomButton>
                  </Col>
                </Row>
              </Row>
              <Row>
                <RichTextEditor
                  value={recipe.text}
                  setValue={(text) => setRecipe({ ...recipe, text })}
                  ref={quillRef}
                />
              </Row>
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
                    type="button"
                  ></CustomButton>
                </Col>
                <Col></Col>
              </Row>
            </Form>
          </Container>
        </Col>
        <Col>
          <TagList></TagList>
        </Col>
      </Row>
      <ImageGallery
        isOpen={isImageGalleryOpen}
        closeModal={closeImageModal}
        setMainImage={setMainImage}
      ></ImageGallery>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
