import { useEffect, useState } from "react";
import CommentService from "../services/CommentService";
import useLoading from "../hooks/useLoading";
import useError from "../hooks/useError";
import { Button, Collapse, Container, ListGroup } from "react-bootstrap";
import CommentCreate from "./CommentCreate";
import { useParams } from "react-router-dom";
import { useUser } from "../contexts/UserContext";
import { FaEdit, FaSave, FaTrashAlt } from "react-icons/fa";

export default function CommentList({ recipeId }) {
  const commentInitialState = {
    memberId: 0,
    recipeId: 0,
    text: "",
    rating: 0,
  };
  const [comments, setComments] = useState([]);
  const [openComentId, setOpenComentId] = useState(null);
  const [editCommentId, setEditCommentId] = useState(null);
  const [editedText, setEditedText] = useState({});
  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrormodal, errors, hideError } = useError();

  const { userId, userFirstName, userLastName } = useUser();

  const routeParams = useParams();

  async function fetchComments() {
    showLoading();
    const response = await CommentService.getRecipeComments(recipeId);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    setComments(response.data);
    hideLoading();
  }

  async function deleteComment(id) {
    const response = await CommentService.setNotActive(
      "comment/softDelete/" + id
    );
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
  }

  async function updateComment(id, entity) {
    showLoading();
    const response = await CommentService.update("comment/update", id, entity);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    hideLoading();
  }

  useEffect(() => {
    fetchComments();
  }, []);

  const renderStars = (rating) => {
    const stars = [];

    for (let i = 0; i < 5; i++) {
      stars.push(
        <span key={i} style={{ color: i < rating ? "#FFD700" : "#CCCCCC" }}>
          ★
        </span>
      );
    }
    return stars;
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);

    const day = String(date.getUTCDate()).padStart(2, "0");
    const month = String(date.getUTCMonth() + 1).padStart(2, "0");
    const year = date.getUTCFullYear();

    const hours = String(date.getUTCHours()).padStart(2, "0");
    const minutes = String(date.getUTCMinutes()).padStart(2, "0");

    return `${day}-${month}-${year}  ${hours}:${minutes}`;
  };

  const toggleText = (id) => {
    setOpenComentId(openComentId === id ? null : id);
  };

  const handleEdit = (id) => {
    setEditCommentId(id);
  };

  const handleDelete = (id) => {
    deleteComment(id);
    setComments((commentList) =>
      commentList.filter((comment) => comment.id !== id)
    );
  };

  const handleSave = (comment) => {
    const updatedText = editedText[comment.id];

    let entity = {
      memberId: userId,
      recipeId: routeParams.id,
      text: updatedText,
      rating: comment.rating,
    };

    updateComment(comment.id, entity);
    console.log("Saving comment:", entity);
    setEditCommentId(null);
  };

  const handleTextChange = (id, value) => {
    setEditedText((commentList) => ({ ...commentList, [id]: value }));
  };

  return (
    <Container>
      <h4>COMMENTS</h4>
      <CommentCreate
        recipeId={routeParams.id}
        fetchComments={fetchComments}
      ></CommentCreate>
      <ListGroup>
        {comments ? (
          comments.map((comment) => (
            <ListGroup.Item className="lg-item" key={comment.id}>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <div>
                  <strong>
                    Comment by {comment.memberFirstName}{" "}
                    {comment.memberLastName}
                  </strong>
                </div>
                Date created - {formatDate(comment.dateCreated)}
              </div>
              <div>
                {renderStars(comment.rating)} ({comment.rating} / 5)
              </div>
              <Button
                variant="link"
                onClick={() => toggleText(comment.id)}
                aria-controls={`text-${comment.id}`}
                aria-expanded={openComentId === comment.id}
              >
                {openComentId === comment.id ? "Hide comment" : "Show comment"}
              </Button>

              <Collapse in={openComentId === comment.id}>
                <div id={`Comment${comment.id}`} style={{ marginTop: "1 %" }}>
                  <textarea
                    className="comment-textarea"
                    value={
                      editCommentId === comment.id
                        ? editedText[comment.id]
                        : comment.text
                    }
                    readOnly={editCommentId !== comment.id}
                    onChange={(e) =>
                      handleTextChange(comment.id, e.target.value)
                    }
                  ></textarea>

                  {comment.memberFirstName === userFirstName &&
                    comment.memberLastName === userLastName && (
                      <div className="comment-icons">
                        {editCommentId === comment.id ? (
                          <FaSave
                            className="comment-icon cmt-save-icon"
                            onClick={() => handleSave(comment)}
                          />
                        ) : (
                          <FaEdit
                            className="comment-icon cmt-edit-icon"
                            onClick={() => handleEdit(comment.id)}
                          />
                        )}
                        <FaTrashAlt
                          className="comment-icon cmt-delete-icon"
                          onClick={() => handleDelete(comment.id)}
                        />
                      </div>
                    )}
                </div>
              </Collapse>
            </ListGroup.Item>
          ))
        ) : (
          <div>There are no comments for this recipe</div>
        )}
      </ListGroup>
    </Container>
  );
}
