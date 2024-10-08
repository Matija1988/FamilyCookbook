import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { Container } from "react-bootstrap";
import FoundRecipeList from "./FoundRecipeList";

export default function SearchResult() {
  const location = useLocation();

  const [recipes, setRecipes] = useState([]);

  useEffect(() => {
    if (location.state && location.state.recipes) {
      setRecipes(location.state.recipes);
    }
  }, [location.state]);

  return (
    <>
      <Container className="mt-5">
        <FoundRecipeList recipes={recipes}></FoundRecipeList>
      </Container>
    </>
  );
}
