import { Button, Collapse, Form, ListGroup } from "react-bootstrap";
import { useUser } from "../contexts/UserContext";
import InputTextArea from "./InputTextArea";
import { FormText } from "react-bootstrap";
import CustomButton from "./CustomButton";
import { useState } from "react";
import CommentService from "../services/CommentService";
import useError from "../hooks/useError";
import { useNavigate, useParams } from "react-router-dom";
import { RouteNames } from "../constants/constants";

export default function CommentCreate({ recipeId, fetchComments }) {
  const commentState = {
    memberId: "",
    recipeId: "",
    text: "",
    rating: 0,
  };

  const { userId, userRole } = useUser();
  const { showError, showErrorModal, errors, hideError } = useError();
  const [openCommentCreate, setOpenCommentCreate] = useState(false);
  const [rating, setRating] = useState(0);
  const routeParams = useParams();

  const navigate = useNavigate();

  async function postComment(entity) {
    const response = await CommentService.create("comment/create", entity);
    if (!response.ok) {
      showError(response.data);
      return;
    }
    fetchComments();
  }

  function handleSubmit(e) {
    e.preventDefault();

    const information = new FormData(e.target);

    commentState.text = information.get("Comment");
    commentState.recipeId = routeParams.id;
    commentState.memberId = userId;
    commentState.rating = rating;

    postComment(commentState);
  }

  const toggleCommentCreate = () => {
    setOpenCommentCreate(!openCommentCreate);
  };

  const renderStars = (currentRating) => {
    const stars = [];

    for (let i = 1; i <= 5; i++) {
      stars.push(
        <span
          key={i}
          style={{
            cursor: "pointer",
            fontSize: "1.5rem",
            color: i <= currentRating ? "#FFD700" : "#CCCCCC",
          }}
          onClick={() => setRating(i)}
        >
          ★
        </span>
      );
    }
    return stars;
  };

  return (
    <div className="cmt-create-div">
      <h6>Create new comment</h6>
      <ListGroup>
        <Button
          variant="link"
          onClick={() => toggleCommentCreate()}
          className="cmt-create-link"
        >
          {openCommentCreate ? "Cancel" : "Create comment"}
        </Button>
        {userId ? (
          <ListGroup.Item>
            <Collapse in={openCommentCreate === true}>
              <Form onSubmit={handleSubmit}>
                <Form.Label>Rating:</Form.Label>
                <div>{renderStars(rating)}</div>
                <Form.Control
                  as="textarea"
                  rows={3}
                  name="Comment"
                  placeholder="Write your comment here"
                />
                <CustomButton
                  label="Create new comment"
                  type="submit"
                ></CustomButton>
              </Form>
            </Collapse>
          </ListGroup.Item>
        ) : (
          <div>Only logged in users can post comments</div>
        )}
      </ListGroup>
    </div>
  );
}
