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
import LoadingSpinner from "./components/LoadingSpinner";
import LogIn from "./pages/logIn/LogIn";
import AdminPanel from "./pages/AdminPanel/AdminPanel";
import ProtectedRoutes from "./components/ProtectedRoutes";
import ArticlesByCategory from "./pages/ArticlePages/ArticlesByCategory";
import Tags from "./pages/Tags/Tags";
import SearchResult from "./pages/SearchResult";
import Register from "./pages/logIn/Register";
import Impresum from "./pages/Impersum/Impesum";
import Banner from "./pages/Banner/Banner";
import BannerCreate from "./pages/Banner/BannerCreate";
function App() {
  const { errors, showErrorModal, hideErrorModal } = useError();

  return (
    <>
      <NavBar />
      <LoadingSpinner />
      <ErrorModal
        show={showErrorModal}
        onHide={hideErrorModal}
        errors={errors}
      ></ErrorModal>

      <Routes>
        <Route path={RouteNames.HOME} element={<Home />}></Route>

        <Route element={<ProtectedRoutes allowedRoles={["Admin"]} />}>
          <Route path={RouteNames.MEMBERS} element={<Members />}></Route>
          <Route
            path={RouteNames.MEMBER_CREATE}
            element={<CreateMember />}
          ></Route>
          <Route
            path={RouteNames.MEMBER_UPDATE}
            element={<UpdateMember />}
          ></Route>
          <Route path={RouteNames.BANNER} element={<Banner />}></Route>
          <Route
            path={RouteNames.BANNER_CREATE}
            element={<BannerCreate />}
          ></Route>
        </Route>
        <Route
          element={<ProtectedRoutes allowedRoles={["Admin", "Moderator"]} />}
        >
          <Route path={RouteNames.CATEGORIES} element={<Categories />}></Route>
          <Route
            path={RouteNames.CATEGORIES_CREATE}
            element={<CategoryCreate />}
          ></Route>
          <Route
            path={RouteNames.CATEGORIES_UPDATE}
            element={<CategoryUpdate />}
          ></Route>
        </Route>
        <Route
          element={
            <ProtectedRoutes
              allowedRoles={["Admin", "Moderator", "Contributor"]}
            />
          }
        >
          <Route path={RouteNames.RECIPES} element={<Recipe />}></Route>
          <Route
            path={RouteNames.RECIPES_CREATE}
            element={<CreateRecipe />}
          ></Route>
          <Route
            path={RouteNames.RECIPES_UPDATE}
            element={<UpdateRecipe />}
          ></Route>
          <Route path={RouteNames.ADMIN_PANEL} element={<AdminPanel />}></Route>
          <Route path={RouteNames.TAGS} element={<Tags />}></Route>
        </Route>
        <Route
          path={RouteNames.RECIPE_DETAILS}
          element={<RecipeDetails />}
        ></Route>

        <Route path={RouteNames.LOGIN} element={<LogIn />}></Route>

        <Route
          path={RouteNames.ARTICLES_BY_CATEGORY}
          element={<ArticlesByCategory />}
        ></Route>
        <Route
          path={RouteNames.SEARCH_RESULTS}
          element={<SearchResult />}
        ></Route>
        <Route path={RouteNames.REGISTER} element={<Register />}></Route>
        <Route path={RouteNames.IMPRESUM} element={<Impresum />}></Route>
      </Routes>
    </>
  );
}

export default App;
