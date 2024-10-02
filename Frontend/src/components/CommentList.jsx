import { useEffect, useState } from "react";
import CommentService from "../services/CommentService";
import useLoading from "../hooks/useLoading";
import useError from "../hooks/useError";
import { Button, Collapse, Container, ListGroup } from "react-bootstrap";

export default function CommentList({ recipeId }) {
  const [comments, setComments] = useState([]);
  const [openComentId, setOpenComentId] = useState(null);

  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrormodal, errors, hideError } = useError();

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

  return (
    <Container>
      <h4>COMMENTS</h4>
      <ListGroup>
        {comments ? (
          comments.map((comment) => (
            <ListGroup.Item className="lg-item" key={comment.id}>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <div>
                  <strong>
                    Created by {comment.memberFirstName}{" "}
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
                  <strong>Comment</strong>
                  <p>{comment.text}</p>
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
