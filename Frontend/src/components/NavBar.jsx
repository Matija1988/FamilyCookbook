import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import { Route, useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";

function NavBar() {
  const navigate = useNavigate();

  return (
    <Navbar
      expand="lg"
      className="bg-body-tertiary"
      data-bs-theme="dark"
      fixed="top"
    >
      <Container>
        <Navbar.Brand onClick={() => navigate(RouteNames.HOME)}>
          React-Bootstrap
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="#home">Home</Nav.Link>
            <Nav.Link href="#link">Link</Nav.Link>
            <NavDropdown title="Dropdown" id="basic-nav-dropdown">
              <NavDropdown.Item onClick={() => navigate(RouteNames.MEMBERS)}>
                Members
              </NavDropdown.Item>
              <NavDropdown.Item onClick={() => navigate(RouteNames.CATEGORIES)}>
                Categoeries
              </NavDropdown.Item>
              <NavDropdown.Item href="#action/3.3">Something</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item href="#action/3.4">
                Separated link
              </NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default NavBar;
