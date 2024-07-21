import { useState } from "react";
import { Routes, Route } from "react-router-dom";

import "./App.css";
import { RouteNames } from "./constants/constants";

import "bootstrap/dist/css/bootstrap.min.css";

import Home from "../src/pages/Home";
import NavBar from "./components/NavBar";
import Members from "./pages/member/Members";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <NavBar />
      <Routes>
        <Route path={RouteNames.HOME} element={<Home />}></Route>
        <Route path={RouteNames.MEMBERS} element={<Members />}></Route>
      </Routes>
    </>
  );
}

export default App;
