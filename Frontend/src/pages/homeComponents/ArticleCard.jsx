import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";

import "./articleCardStyle.css";

function ArticleCard({ recipe }) {
  const URL = "https://localhost:7170/";

  return (
    <Card key={recipe.id} className="article-card">
      <Card.Img variant="top" src={URL + recipe.pictureLocation} />
      <Card.Body>
        <Card.Title>{recipe.title}</Card.Title>
        <Card.Text className="card-subtitle">{recipe.subtitle}</Card.Text>
        <div className="card-button">
          <Button variant="primary">Go to recipe</Button>
        </div>
      </Card.Body>
    </Card>
  );
}

export default ArticleCard;
