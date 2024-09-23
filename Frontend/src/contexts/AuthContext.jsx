import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";
import AuthService from "../services/AuthService";
import { useUser } from "./UserContext";

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [authToken, setAuthToken] = useState("");
  const { resetUser, setUserId, setUserRole, setUserOnLogin } = useUser();
  const [role, setRole] = useState();

  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("Bearer");

    if (token && isLoggedIn) {
      setAuthToken(token);
      setIsLoggedIn(true);
    } else {
      navigate(RouteNames.HOME);
    }
  }, [isLoggedIn]);

  async function login(userData) {
    const response = await AuthService.logInService(userData);

    if (response.ok) {
      localStorage.setItem("Bearer", response.data);
      setAuthToken(response.data);
      setIsLoggedIn(true);
      await setUserOnLogin(response.data.id);
      navigate(RouteNames.HOME);
    } else {
      localStorage.setItem("Bearer", "");
      setAuthToken("");
      setIsLoggedIn(false);
    }
  }

  function logout() {
    localStorage.setItem("Bearer", "");
    setAuthToken("");
    setIsLoggedIn(false);
    resetUser();
    setUserRole(null);
    navigate(RouteNames.HOME);
  }

  const value = {
    isLoggedIn,
    authToken,
    login,
    logout,
    role,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
