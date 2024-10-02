import { Container, Nav } from "react-bootstrap";
import { Route, useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import { useUser } from "../../contexts/UserContext";

export default function Sidebar() {
  const navigate = useNavigate();

  const { userFirstName, userLastName, userRole } = useUser();

  return (
    <>
      <div className="sidebar">
        <h4>Admin panel</h4>
        <h4>
          Welcome <br></br> {userFirstName} {userLastName}
        </h4>

        <Nav className="flex-column">
          {userRole === "Admin" && (
            <Nav.Link onClick={() => navigate(RouteNames.MEMBERS)}>
              MEMBER
            </Nav.Link>
          )}
          {(userRole === "Admin" || userRole === "Moderator") && (
            <Nav.Link onClick={() => navigate(RouteNames.CATEGORIES)}>
              CATEGORY
            </Nav.Link>
          )}
          {(userRole === "Admin" ||
            userRole === "Moderator" ||
            userRole === "Contributor") && (
            <Nav.Link onClick={() => navigate(RouteNames.RECIPES)}>
              RECIPES
            </Nav.Link>
          )}
        </Nav>
      </div>
    </>
  );
}
