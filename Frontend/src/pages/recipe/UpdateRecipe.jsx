import { Col, Container, Form, ListGroup, Row, Table } from "react-bootstrap";
import InputText from "../../components/InputText";
import { useEffect, useRef, useState } from "react";
import { useLoaderData, useNavigate, useParams } from "react-router-dom";
import RecipeService from "../../services/RecipeService";
import InputTextArea from "../../components/InputTextArea";
import CategoriesService from "../../services/CategoriesService";
import SelectionDropdown from "../../components/SelectionDropdown";
import CustomButton from "../../components/CustomButton";
import { RouteNames } from "../../constants/constants";
import MembersService from "../../services/MembersService";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import { TbWashDryP } from "react-icons/tb";
import RichTextEditor from "../../components/RichTextEditor";
import PictureService from "../../services/PictureService";
import ImageGallery from "../../components/ImageGallery";
import Sidebar from "../AdminPanel/Sidebar";
import TagsService from "../../services/TagsService";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";
import ErrorModal from "../../components/ErrorModal";

export default function UpdateRecipe() {
  const [recipe, setRecipe] = useState({
    title: "",
    subtitle: "",
    text: "",
    categoryId: null,
    members: [
      {
        id: "",
        firstName: "",
        lastName: "",
      },
    ],
    pictureName: "",
    pictureLocation: "",
    pictureId: "",
    tags: [{ id: "", text: "" }],
  });
  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState("");
  const [members, setMembers] = useState([]);
  const [foundMembers, setFoundMembers] = useState([]);
  const [foundTags, setFoundTags] = useState([]);
  const { showError, errors, hideError, showErrorModal } = useError();
  const { showLoading, hideLoading } = useLoading();
  const [oldPictureName, setOldPictureName] = useState("");

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
  const URL = "https://localhost:7170/";

  const [error, setError] = useState("");

  async function fetchRecipe() {
    showLoading();
    const response = await RecipeService.getById("recipe", routeParams.id);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    setRecipe(response.data);
    setMembers(response.data.members);

    setSelectedCategoryId(response.data.categoryId);
    setOldPictureName(response.data.pictureName);
    setImageFromGallery({ location: response.data.pictureLocation });
    hideLoading();
  }

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

  useEffect(() => {
    fetchRecipe();
    fetchCategories();
  }, []);

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

    const authorIds = recipe.members.map((member) => member.id);

    const postTagIds = recipe.tags.map((tag) => tag.id);

    console.log("AuthorIds " + authorIds);

    if (!newPictureName || newPictureName.trim() === "") {
      setNewPictureName(oldPictureName);
    }

    UpdateRecipe({
      title: information.get("Title"),
      subtitle: information.get("Subtitle"),
      text: recipe.text,
      categoryId: parseInt(selectedCategoryId),
      memberIds: authorIds,
      tagsIds: postTagIds,
      pictureName: newPictureName || oldPictureName,
      pictureBlob: newPicture || null,
    });
  }

  function assignMemberToRecipe(member) {
    const updatedMembers = Array.isArray(recipe.members)
      ? [...recipe.members, member]
      : [member];

    setRecipe({ ...recipe, members: updatedMembers });
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
      const updatedTags = [...recipe.tags, tag];
      setRecipe({ ...recipe, tags: updatedTags });
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
                    initValue={selectedCategoryId}
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
                  <InputText atribute="Title" value={recipe.title}></InputText>
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
                      {recipe.members &&
                      recipe.members.length > 0 &&
                      members.length > 0 ? (
                        recipe.members.map((member) => {
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
                    {recipe.tags && recipe.tags.length > 0 ? (
                      recipe.tags.map((tag) => {
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
                  <InputText
                    atribute="Subtitle"
                    value={recipe.subtitle}
                  ></InputText>
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
                  ></CustomButton>
                </Col>
                <Col></Col>
              </Row>
            </Form>
          </Container>
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
