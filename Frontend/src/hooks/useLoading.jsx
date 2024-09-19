import { useContext } from "react";
import { LoadingContext } from "../contexts/LoadingContext";

export default function useLoading() {
  const context = useContext(LoadingContext);

  if (!context) {
    throw new Error(
      "useLoading hook must be used within the LoadingProvider!!!"
    );
  }
  return context;
}
