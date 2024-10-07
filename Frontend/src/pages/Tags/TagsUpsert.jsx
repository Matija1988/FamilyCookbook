import { Container, Form } from "react-bootstrap";
import InputText from "../../components/InputText";
import { useEffect, useState } from "react";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import TagsService from "../../services/TagsService";
import CustomButton from "../../components/CustomButton";

export default function TagsUpsert({ entity }) {
  const [tagText, setTagText] = useState("");
  const [tags, setTags] = useState([]);

  const { showLoading, hideLoading } = useLoading();
  const { showError } = useError();

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

    if (entity && entity.id) {
      response = await TagsService.update("");
    } else {
      response = await TagsService.create("tag", requestBody.entities);
    }
    if (!response.ok) {
      showError(response.data);
    }
    hideLoading();
  }

  const handleAdd = () => {
    if (tagText.trim() !== "") {
      setTags((prevTags) => [...prevTags, { text: tagText.trim() }]);
      setTagText("");
    }
    console.log("Tags", tags);
  };

  return (
    <>
      <Container>
        <h4>{entity && entity.id ? "Update Tag" : "Create Tag"}</h4>
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
