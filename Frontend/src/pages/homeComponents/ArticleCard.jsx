import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";

import "./articleCardStyle.css";
import { Route, useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

function ArticleCard({ recipe }) {
  const URL = "https://localhost:7170/";

  const navigate = useNavigate();

  return (
    <Card key={recipe.id} className="article-card">
      <Card.Img variant="top" src={URL + recipe.pictureLocation} />
      <Card.Body>
        <Card.Title>{recipe.title}</Card.Title>
        <Card.Text className="card-subtitle">{recipe.subtitle}</Card.Text>
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
