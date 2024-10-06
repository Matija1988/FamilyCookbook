import { Container } from "react-bootstrap";
import Sidebar from "../AdminPanel/Sidebar";

export default function Tags() {
  return (
    <>
      <Sidebar></Sidebar>
      <Container className="primaryContainer">
        <h1>Tags</h1>
      </Container>
    </>
  );
}
