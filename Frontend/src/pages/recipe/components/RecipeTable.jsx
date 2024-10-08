import { Table } from "react-bootstrap";
import CustomButton from "../../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../../constants/constants";
import { useState } from "react";

export default function RecipeTable({
  recipes,
  goToDetails,
  handleDelete,
  onUpdate,
}) {
  const navigate = useNavigate();

  const [entityId, setEntityId] = useState();

  return (
    <>
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>Title</th>
            <th>Subtitle</th>
            <th>Category</th>
            <th>Author</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {recipes.length > 0 ? (
            recipes.map((entity) => (
              <tr key={entity.id}>
                <td>{entity.title}</td>
                <td>{entity.subtitle}</td>
                <td>{entity.categoryName}</td>
                <td>
                  <ul>
                    {entity.members && entity.members.length > 0 ? (
                      entity.members.map((author, index) => (
                        <li key={index}>
                          {author.firstName} {author.lastName}
                        </li>
                      ))
                    ) : (
                      <li>No authors available</li>
                    )}
                  </ul>
                </td>
                <td>
                  <CustomButton
                    variant="primary"
                    onClick={() => {
                      navigate(
                        RouteNames.RECIPES_UPDATE.replace(":id", entity.id)
                      );
                    }}
                    label="UPDATE"
                  ></CustomButton>
                  <CustomButton
                    variant="secondary"
                    onClick={() => {
                      navigate(
                        RouteNames.RECIPE_DETAILS.replace(":id", entity.id)
                      );
                    }}
                    label="DETAILS"
                  ></CustomButton>
                  <CustomButton
                    variant="danger"
                    onClick={() => (setEntityId(entity), handleDelete(entity))}
                    label="DELETE"
                  ></CustomButton>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="4">No recipes to load...</td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
