import { useState } from "react";
import { Container } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

export default function RichTextEditor(atribute, value, setValue) {
  //const [value, setValue] = useState("");

  return (
    <Container>
      <ReactQuill
        atribute={atribute}
        theme="snow"
        value={value}
        onChange={setValue}
      />
    </Container>
  );
}
