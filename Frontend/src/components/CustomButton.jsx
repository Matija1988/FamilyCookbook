import PropTypes from "prop-types";
import { Button } from "react-bootstrap";

export default function CustomButton({ label, onClick, variant }) {
  return (
    <Button onClick={onClick} variant={variant}>
      {label}
    </Button>
  );
}

CustomButton.propTypes = {
  label: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  Variant: PropTypes.string,
};
