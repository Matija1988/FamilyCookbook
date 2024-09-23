import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../constants/constants";

export const authContext = createContext();

export function AuthProvider({ children }) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [authToken, setAuthToken] = useState("");
  const { resetUser, setUserId, setUserRole, setUserOnLogin } = useUser();
  const [role, setRole] = useState();

  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("Bearer");

    if (token) {
      setAuthToken(token);
      setIsLoggedIn(true);
    } else {
      navigate(RouteNames.HOME);
    }
  }, []);

  async function login(userData) {
    const response = await logInService(userData);

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
    navigate(RoutesName.HOME);
  }

  const value = {
    isLoggedIn,
    authToken,
    login,
    logout,
    role,
  };

  return (
    <AuthContext.AuthProvider value={value}>
      {children}
    </AuthContext.AuthProvider>
  );
}
