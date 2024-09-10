import React, { forwardRef } from "react";
import { Container } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

const RichTextEditor = forwardRef(({ value, setValue }, ref) => {
  const handleChange = (content) => {
    setValue(content); // Update the value using the setValue prop
  };

  return (
    <Container>
      <ReactQuill
        ref={ref} // Attach the ref to ReactQuill
        theme="snow"
        value={value}
        onChange={handleChange}
      />
    </Container>
  );
});

export default RichTextEditor;
