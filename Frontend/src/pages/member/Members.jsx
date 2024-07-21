import { Container, Table } from "react-bootstrap";
import { httpService } from "../../services/HttpService";
import MembersService from "../../services/MembersService";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";

export default function Members() {
  const [members, setMembers] = useState();

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

  return (
    <>
      <Container>
        <h1>MEMBERS PAGE</h1>
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
                  <td>{index}</td>
                  <td>{member.firstName}</td>
                  <td>{member.lastName}</td>
                  <td>{member.dateOfBirth}</td>
                  <td>
                    <CustomButton
                      label="Delete"
                      className="btn-delete"
                      onClick={() => deleteMember(member.id)}
                    ></CustomButton>
                    <CustomButton
                      label="Update"
                      className="btn-update"
                      onClick={() => updateMember(member.id)}
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
