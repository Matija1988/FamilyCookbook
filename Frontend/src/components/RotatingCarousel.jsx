import Carousel from "react-bootstrap/Carousel";
import RecipeService from "../services/RecipeService";
import { useEffect, useState } from "react";

function RotatingCarousel({}) {
  const [recipes, setRecipes] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [countPage, setCount] = useState(0);
  const [activityStatus, setActivityStatus] = useState(1);
  const [totalPages, setTotalPages] = useState(0);

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
            className="d-block w-100"
            src={"https://localhost:7170/" + recipe.pictureLocation}
            alt={recipe.title}
          />
        </Carousel.Item>
      ))}
    </Carousel>
  );
}

export default RotatingCarousel;
