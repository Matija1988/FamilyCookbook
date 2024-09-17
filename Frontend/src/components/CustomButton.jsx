import PropTypes from "prop-types";
import { Button } from "react-bootstrap";

export default function CustomButton({
  label,
  onClick,
  variant,
  type,
  className,
}) {
  return (
    <Button
      onClick={onClick}
      variant={variant}
      type={type}
      className={className}
    >
      {label}
    </Button>
  );
}

CustomButton.propTypes = {
  label: PropTypes.string.isRequired,
  variant: PropTypes.string,
  type: PropTypes.string,
};
