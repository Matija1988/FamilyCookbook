import { createContext, useState } from "react";
import PropTypes from "prop-types";

export const LoadingContext = createContext();

export function LoadingProvider({ children }) {
  const [loading, setLoading] = useState(false);

  function showLoading() {
    setLoading(true);
  }
  function hideLoading() {
    setLoading(false);
  }

  const value = {
    loading,
    showLoading,
    hideLoading,
  };

  return (
    <LoadingContext.Provider value={value}>{children}</LoadingContext.Provider>
  );
}

LoadingProvider.propTypes = {
  children: PropTypes.node.isRequired,
};
