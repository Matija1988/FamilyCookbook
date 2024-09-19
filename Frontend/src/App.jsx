import { useState } from "react";
import { Routes, Route } from "react-router-dom";

import "./App.css";
import { RouteNames } from "./constants/constants";

import "bootstrap/dist/css/bootstrap.min.css";

import Home from "../src/pages/Home";
import NavBar from "./components/NavBar";
import Members from "./pages/member/Members";
import CreateMember from "./pages/member/CreateMember";
import UpdateMember from "./pages/member/UpdateMember";
import Categories from "./pages/category/Categories";
import CategoryCreate from "./pages/category/CategoryCreate";
import CategoryUpdate from "./pages/category/CategoryUpdate";
import Recipe from "./pages/recipe/Recipe";
import CreateRecipe from "./pages/recipe/CreateRecipe";
import UpdateRecipe from "./pages/recipe/UpdateRecipe";
import RecipeDetails from "./pages/recipe/RecipeDetails";
import useError from "./hooks/useError";
import ErrorModal from "./components/ErrorModal";
function App() {
  const { errors, showErrorModal, hideErrorModal } = useError();

  return (
    <>
      <NavBar />
      <ErrorModal
        show={showErrorModal}
        onHide={hideErrorModal}
        errors={errors}
      ></ErrorModal>
      <Routes>
        <Route path={RouteNames.HOME} element={<Home />}></Route>
        <Route path={RouteNames.MEMBERS} element={<Members />}></Route>
        <Route
          path={RouteNames.MEMBER_CREATE}
          element={<CreateMember />}
        ></Route>
        <Route
          path={RouteNames.MEMBER_UPDATE}
          element={<UpdateMember />}
        ></Route>
        <Route path={RouteNames.CATEGORIES} element={<Categories />}></Route>
        <Route
          path={RouteNames.CATEGORIES_CREATE}
          element={<CategoryCreate />}
        ></Route>
        <Route
          path={RouteNames.CATEGORIES_UPDATE}
          element={<CategoryUpdate />}
        ></Route>
        <Route path={RouteNames.RECIPES} element={<Recipe />}></Route>
        <Route
          path={RouteNames.RECIPES_CREATE}
          element={<CreateRecipe />}
        ></Route>
        <Route
          path={RouteNames.RECIPES_UPDATE}
          element={<UpdateRecipe />}
        ></Route>
        <Route
          path={RouteNames.RECIPE_DETAILS}
          element={<RecipeDetails />}
        ></Route>
      </Routes>
    </>
  );
}

export default App;
