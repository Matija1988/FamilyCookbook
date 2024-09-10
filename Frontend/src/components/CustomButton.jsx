import PropTypes from "prop-types";
import { Button } from "react-bootstrap";

export default function CustomButton({ label, onClick, variant, type }) {
  return (
    <Button onClick={onClick} variant={variant} type={type}>
      {label}
    </Button>
  );
}

CustomButton.propTypes = {
  label: PropTypes.string.isRequired,
  variant: PropTypes.string,
  type: PropTypes.string,
};
