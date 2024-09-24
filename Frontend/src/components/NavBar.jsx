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

function NavBar() {
  const navigate = useNavigate();

  const { isLoggedIn, logout, role } = useAuth();

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
            <Nav.Link onClick={() => navigate(RouteNames.ADMIN_PANEL)}>
              ADMIN
            </Nav.Link>
            {role === "Admin" && (
              <NavDropdown title="Admin" id="basic-nav-dropdown">
                <NavDropdown.Item onClick={() => navigate(RouteNames.MEMBERS)}>
                  Members
                </NavDropdown.Item>
                <NavDropdown.Item
                  onClick={() => navigate(RouteNames.CATEGORIES)}
                >
                  Categories
                </NavDropdown.Item>
                <NavDropdown.Item onClick={() => navigate(RouteNames.RECIPES)}>
                  Recipes
                </NavDropdown.Item>
              </NavDropdown>
            )}
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
