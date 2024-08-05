import { Container, Table } from "react-bootstrap";
import { httpService } from "../../services/HttpService";
import MembersService from "../../services/MembersService";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

import "../style.css";
import GenericTable from "../../components/GenericTable";

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

  async function deleteMember(member) {
    const response = await MembersService.setNotActive(
      "member/delete/" + member.id
    );
    if (response.ok) {
      fetchMembers();
    }
  }

  function createMember() {
    navigate(RouteNames.MEMBER_CREATE);
  }

  function handleUpdate(member) {
    navigate(RouteNames.MEMBER_UPDATE.replace(":id", member.id));
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

        <GenericTable
          dataArray={members}
          onDelete={deleteMember}
          onUpdate={handleUpdate}
          cutRange={2}
        ></GenericTable>
      </Container>
    </>
  );
}
