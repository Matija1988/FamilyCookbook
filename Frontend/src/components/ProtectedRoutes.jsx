import { Navigate, Outlet, useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";
import { jwtDecode } from "jwt-decode";

const ProtectedRoutes = ({ allowedRoles }) => {
  const token = localStorage.getItem("Bearer");

  const navigate = useNavigate();

  if (!token) {
    Console.log("No token found");
    return <Navigate to="/" />;
  }

  try {
    const decode = jwtDecode(token);
    const userRole = decode.role;
    console.log("Protected route User Role: ", userRole);

    if (allowedRoles.includes(userRole)) {
      return <Outlet />;
    } else {
      alert("FORBIDDEN!!!");
      return <Navigate to="/" />;
    }
  } catch (error) {
    alert("ERROR " + error);
    return <Navigate to="/" />;
  }
};

export default ProtectedRoutes;
