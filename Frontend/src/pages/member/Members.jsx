import { Container, Table } from "react-bootstrap";
import { httpService } from "../../services/HttpService";
import MembersService from "../../services/MembersService";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

import "../style.css";

export default function Members() {
  const [members, setMembers] = useState();

  const navigate = useNavigate();

  async function fetchMembers() {
    const response = await MembersService.readAll("member");

    if (!response.ok) {
      alert("Error!");
      return;
    }
    setMembers(response.data.items);
  }

  useEffect(() => {
    fetchMembers();
  }, []);

  function deleteMember(id) {}

  function updateMember(id) {}

  function createMember() {
    navigate(RouteNames.MEMBER_CREATE);
  }

  return (
    <>
      <Container className="primaryContainer">
        <h1>MEMBERS PAGE</h1>
        <CustomButton
          label="Create new"
          variant="primary"
          onClick={() => createMember()}
        ></CustomButton>
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>No</th>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Date of birth</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {members &&
              members.map((member, index) => (
                <tr key={index}>
                  <td>{index + 1}</td>
                  <td>{member.firstName}</td>
                  <td>{member.lastName}</td>
                  <td>{member.dateOfBirth}</td>
                  <td>
                    <CustomButton
                      label="Update"
                      variant="primary"
                      onClick={() => updateMember(member.id)}
                    ></CustomButton>

                    <CustomButton
                      label="Delete"
                      variant="danger"
                      onClick={() => deleteMember(member.id)}
                    ></CustomButton>
                  </td>
                </tr>
              ))}
          </tbody>
        </Table>
      </Container>
    </>
  );
}
