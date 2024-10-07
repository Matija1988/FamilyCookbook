import { createContext, useState } from "react";
import PropTypes from "prop-types";

export const ErrorContext = createContext();

export const ErrorProvider = ({ children }) => {
  const [errors, setErrors] = useState([]);
  const [showErrorModal, setShowErrorModal] = useState(false);

  function showError(errorsMessages) {
    if (!Array.isArray(errorsMessages)) {
      errorsMessages = [errorsMessages];
    }
    setErrors(errorsMessages);
    setShowErrorModal(true);
  }

  function hideError() {
    setErrors([]);
    setShowErrorModal(false);
  }

  return (
    <ErrorContext.Provider
      value={{ errors, showErrorModal, showError, hideError }}
    >
      {children}
    </ErrorContext.Provider>
  );
};
ErrorProvider.propTypes = {
  children: PropTypes.node.isRequired,
};
