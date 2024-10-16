import Carousel from "react-bootstrap/Carousel";
import RecipeService from "../services/RecipeService";
import { useEffect, useState } from "react";
import { Container, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { App, RouteNames } from "../constants/constants";

import "./Carousel.css";

function RotatingCarousel({}) {
  const [recipes, setRecipes] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [countPage, setCount] = useState(0);
  const [activityStatus, setActivityStatus] = useState(1);
  const [totalPages, setTotalPages] = useState(0);

  const navigate = useNavigate();

  const getRequestParams = (PageSize, PageNumber, SearchByActivityStatus) => {
    return {
      PageSize,
      PageNumber,
      SearchByActivityStatus,
    };
  };
  async function fectchRecipes() {
    const params = getRequestParams(pageSize, pageNumber, activityStatus);
    try {
      const response = await RecipeService.paginate("recipe/paginate", params);
      const { items, pageCount } = response.data;
      setRecipes(items);
      setTotalPages(pageCount);
    } catch (error) {
      alert(error.message);
    }
  }

  useEffect(() => {
    fectchRecipes();
  }, [pageNumber, pageSize]);

  return (
    <Container className="crsl-container">
      <Carousel className="rotating-carousel">
        {recipes.map((recipe) => (
          <Carousel.Item key={recipe.id}>
            <img
              onClick={() => {
                navigate(RouteNames.RECIPE_DETAILS.replace(":id", recipe.id));
              }}
              src={App.URL + recipe.pictureLocation}
              alt={recipe.title}
              style={{
                height: "22rem",
                objectFit: "cover",
                width: "70%",
                justifyContent: "center",
                marginLeft: "15%",
                marginTop: "6%",
              }}
            />
            <Carousel.Caption>
              <h3 className="crsl-title">{recipe.title}</h3>
              <p className="crsl-title">{recipe.subtitle}</p>
            </Carousel.Caption>
          </Carousel.Item>
        ))}
      </Carousel>
    </Container>
  );
}

export default RotatingCarousel;
