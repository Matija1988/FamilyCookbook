import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import { Route, useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";
import { TbRouteScan } from "react-icons/tb";

import "../App.css";
import useAuth from "../hooks/useAuth";
import { useUser } from "../contexts/UserContext";
import CategoriesService from "../services/CategoriesService";
import useError from "../hooks/useError";
import { useEffect, useState } from "react";
import InputText from "./InputText";
import { Form } from "react-bootstrap";
import CustomButton from "./CustomButton";

function NavBar() {
  const [categories, setCategories] = useState([]);
  const [searchText, setSearchText] = useState("");
  const [recipes, setRecipes] = useState([]);
  const { showError, showErrorModal, errors, hideError } = useError();
  const navigate = useNavigate();

  const { isLoggedIn, logout, role } = useAuth();

  const rolesArray = ["Admin", "Moderator", "Contributor"];

  async function fetchCategories() {
    const response = await CategoriesService.readAll("category");
    if (!response.ok) {
      showError(response.data);
    }
    setCategories(response.data.items);
  }

  async function searchRecipes() {
    
  }

  useEffect(() => {
    fetchCategories();
  }, []);

  const searchCondition = (e) => {
    setSearchText(e.target.value);
  };

  return (
    <Navbar
      expand="lg"
      className="bg-body-tertiary"
      data-bs-theme="dark"
      fixed="top"
    >
      <Container>
        <Navbar.Brand onClick={() => navigate(RouteNames.HOME)}>
          Cookbook
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link onClick={() => navigate(RouteNames.HOME)}>HOME</Nav.Link>
            {rolesArray.includes(role) && (
              <Nav.Link onClick={() => navigate(RouteNames.ADMIN_PANEL)}>
                ADMIN
              </Nav.Link>
            )}

            <NavDropdown title="Categories" id="basic-nav-dropdown">
              {categories ? (
                categories.map((category) => (
                  <NavDropdown.Item
                    key={category.id}
                    value={category.name}
                    onClick={() =>
                      navigate(
                        RouteNames.ARTICLES_BY_CATEGORY.replace(
                          ":id",
                          category.id
                        )
                      )
                    }
                  >
                    {category.name}
                  </NavDropdown.Item>
                ))
              ) : (
                <NavDropdown.Item>No categories to load</NavDropdown.Item>
              )}
            </NavDropdown>
            <Form>
              <Form.Control
                type="input"
                placeholder="search..."
                onChange={searchCondition}
              ></Form.Control>
              <CustomButton
                label="Search"
                onClick={searchRecipes}
              ></CustomButton>
            </Form>
            {isLoggedIn ? (
              <Nav.Link className="logIn-link" onClick={logout}>
                Logout
              </Nav.Link>
            ) : (
              <Nav.Link
                className="logIn-link"
                onClick={() => navigate(RouteNames.LOGIN)}
              >
                LOGIN
              </Nav.Link>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default NavBar;
