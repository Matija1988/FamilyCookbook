import { useContext } from "react";
import { authContext } from "../contexts/AuthContext";
import { PiTrademarkRegisteredLight } from "react-icons/pi";

export default function useAuth() {
  const context = useContext(authContext);
  if (!context) {
    throw new Error("useAuth must be used within the AuthProvider");
  }
  return context;
}
