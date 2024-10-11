import React, { forwardRef } from "react";
import { Container } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

const RichTextEditor = forwardRef(({ value, setValue }, ref) => {
  const handleChange = (content) => {
    setValue(content);
  };

  return (
    <Container>
      <ReactQuill
        ref={ref}
        theme="snow"
        value={value}
        onChange={handleChange}
        style={{ heigth: "50%" }}
      />
    </Container>
  );
});

export default RichTextEditor;
