import { Container, Form } from "react-bootstrap";
import InputText from "../../components/InputText";
import { useEffect, useState } from "react";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import TagsService from "../../services/TagsService";
import CustomButton from "../../components/CustomButton";

export default function TagsUpsert({ entity, paginate }) {
  const [tagText, setTagText] = useState("");
  const [tags, setTags] = useState([]);

  const { showLoading, hideLoading } = useLoading();
  const { showError, errors, showErrorModal, hideError } = useError();

  useEffect(() => {
    if (entity) {
      setTagText(entity.text || "");
    }
  }, [entity]);

  async function handleSubmit(e) {
    e.preventDefault();
    showLoading();
    const requestBody = {
      entities: tags,
    };

    let response;

    let updatedText = tagText;

    const updateBody = {
      text: updatedText,
    };

    if (entity && entity.id) {
      response = await TagsService.update("tag/update", entity.id, updateBody);
    } else {
      response = await TagsService.create("tag", requestBody.entities);
    }
    if (!response.ok) {
      showError(response.data);
    }
    setTags([]);
    paginate();
    hideLoading();
  }
  const handleAdd = () => {
    if (tagText.trim() !== "") {
      setTags((prevTags) => [...prevTags, { text: tagText.trim() }]);
      setTagText("");
    }
  };

  return (
    <>
      <Container>
        <h4>{entity && entity.text ? "Update Tag" : "Create Tag"}</h4>
        <Form onSubmit={handleSubmit}>
          <Form.Group>
            <InputText
              atribute="Text"
              value={tagText}
              required={true}
              onChange={(e) => setTagText(e.target.value)}
            ></InputText>
          </Form.Group>
          {entity && entity.id ? (
            <CustomButton
              label="submit"
              type="submit"
              variant="primary m-3"
            ></CustomButton>
          ) : (
            <div>
              <CustomButton
                label="Add"
                onClick={(e) => handleAdd(e)}
                variant="primary m-3"
                type="button"
              ></CustomButton>
              <CustomButton
                label="submit"
                type="submit"
                variant="primary m-3"
              ></CustomButton>
              <div>
                {tags.length > 0 && (
                  <ul>
                    {tags.map((t, index) => (
                      <li key={index}>{t.text}</li>
                    ))}
                  </ul>
                )}
              </div>
            </div>
          )}
        </Form>
      </Container>
    </>
  );
}
