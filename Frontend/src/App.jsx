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
function App() {
  return (
    <>
      <NavBar />
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
      </Routes>
    </>
  );
}

export default App;
