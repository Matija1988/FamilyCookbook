import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";

import "./articleCardStyle.css";
import { Route, useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

function ArticleCard({ recipe }) {
  const URL = "https://localhost:7170/";

  const navigate = useNavigate();

  const renderStars = (rating) => {
    const stars = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

    for (let i = 0; i < fullStars; i++) {
      stars.push(
        <span key={`full-${i}`} style={{ color: "#FFD700" }}>
          ★
        </span>
      );
    }

    if (hasHalfStar) {
      stars.push(
        <span key={`half`} style={{ color: "#FFD700" }}>
          ☆
        </span>
      );
    }

    for (let i = 0; i < emptyStars; i++) {
      stars.push(
        <span key={`empty-${i}`} style={{ color: "#CCCCCC" }}>
          ☆
        </span>
      );
    }

    // for (let i = 0; i < 5; i++) {
    //   stars.push(
    //     <span key={i} style={{ color: i < rating ? "#FFD700" : "#CCCCCC" }}>
    //       ★
    //     </span>
    //   );
    // }
    return stars;
  };

  return (
    <Card key={recipe.id} className="article-card">
      <Card.Img variant="top" src={URL + recipe.pictureLocation} />
      <Card.Body>
        <Card.Title>{recipe.title}</Card.Title>
        <Card.Text className="card-subtitle">{recipe.subtitle}</Card.Text>
        <Card.Text className="card-category">{recipe.categoryName}</Card.Text>
        <div>{renderStars(recipe.averageRating)}</div>
        <div className="card-button">
          <Button
            variant="primary"
            onClick={() =>
              navigate(RouteNames.RECIPE_DETAILS.replace(":id", recipe.id))
            }
          >
            Go to recipe
          </Button>
        </div>
      </Card.Body>
    </Card>
  );
}

export default ArticleCard;
