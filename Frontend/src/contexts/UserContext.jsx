import { jwtDecode } from "jwt-decode";
import {
  Children,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import MembersService from "../services/MembersService";

const UserContext = createContext();

export const useUser = () => {
  return useContext(UserContext);
};

export const UserProvider = ({ children }) => {
  const [userId, setUserId] = useState(null);
  const [userRole, setUserRole] = useState(null);
  const [userFirstName, setUserFirstName] = useState(null);
  const [userLastName, setUserLastName] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("Bearer");
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        const username = decodedToken.Id;
        if (username) {
          fetchUserId(username);
        } else {
          console.log("No username found in the token");
        }
        const role = decodedToken.role;
        setUserRole(role);
      } catch (error) {
        console.log("Error in userContext", error);
      }
    }
  }, []);

  const fetchUserId = async (username) => {
    if (!username) {
      console.error("Cannot fetch user");
      return;
    }
    try {
      const response = await MembersService.getByUsername(username);
      if (response.ok) {
        setUserId(response.data.id);
        setUserFirstName(response.data.firstName);
        setUserLastName(response.data.lastName);
      }
    } catch (error) {
      console.log("Error Usercontext", error);
    }
  };

  const setUserOnLogin = async (input) => {
    const token = localStorage.getItem("Bearer");
    const decodedToken = jwtDecode(token);
    const username = decodedToken.Id;
    fetchUserId(username);
  };

  const resetUser = async () => {
    setUserId(null);
    setUserRole(null);
    setUserFirstName(null);
    setUserLastName(null);
  };

  return (
    <UserContext.Provider
      value={{
        userId,
        userRole,
        userFirstName,
        userLastName,
        setUserId,
        setUserRole,
        resetUser,
        setUserOnLogin,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};
