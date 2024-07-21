import PropTypes from "prop-types";

export default function CustomButton({ label, onClick, className }) {
  return (
    <button onClick={onClick} className={className}>
      {label}
    </button>
  );
}

CustomButton.PropTypes = {
  label: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  className: PropTypes.string,
};
