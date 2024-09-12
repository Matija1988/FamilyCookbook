import Carousel from "react-bootstrap/Carousel";
import RecipeService from "../services/RecipeService";
import { useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";

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
      const response = await RecipeService.paginate(params);
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
    <Carousel>
      {recipes.map((recipe) => (
        <Carousel.Item>
          <img
            onClick={() => {
              navigate(RouteNames.RECIPE_DETAILS.replace(":id", recipe.id));
            }}
            className="d-block w-80"
            src={"https://localhost:7170/" + recipe.pictureLocation}
            alt={recipe.title}
            style={{
              height: "500px",
              objectFit: "cover",
              width: "70%",
              justifyContent: "center",
              marginLeft: "15%",
            }}
          />
          <Carousel.Caption>
            <h3 className="crsl-title">{recipe.title}</h3>
            <p className="crsl-title">{recipe.subtitle}</p>
          </Carousel.Caption>
        </Carousel.Item>
      ))}
    </Carousel>
  );
}

export default RotatingCarousel;
