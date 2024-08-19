import { Col, Container, Form, Row, Table } from "react-bootstrap";
import { httpService } from "../../services/HttpService";
import MembersService from "../../services/MembersService";
import { useEffect, useState } from "react";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";

import "../style.css";
import GenericTable from "../../components/GenericTable";
import CustomPagination from "../../components/CustomPagination";
import PageSizeDropdown from "../../components/PageSizeDropdown";
import SelectionDropdown from "../../components/SelectionDropdown";
import ActivityStatusSelection from "../../components/ActivityStatusSelection";
import Pagination from "materialui-pagination-component";

export default function Members() {
  const [members, setMembers] = useState();
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [countPage, setCount] = useState(0);

  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 0, name: "Not active" },
  ];

  const [activityStatus, setActivityStatus] = useState(1);
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const getRequestParams = (pageSize, pageNumber, activityStatus) => {
    let params = {};
    if (pageSize) {
      params["PageSize"] = pageSize;
    }
    if (pageNumber) {
      params["PageNumber"] = pageNumber;
    }
    if (activityStatus) {
      params["SearchByActivityStatus"] = activityStatus;
    }
    return params;
  };

  async function paginateMembers() {
    const params = getRequestParams(pageSize, pageNumber, activityStatus);

    const response = await MembersService.paginate(params)
      .then((response) => {
        const { items, pageCount } = response.data;
        setMembers(items);
        setCount(pageCount);
      })
      .catch((e) => {
        setError("Error fetching users" + e.message);
      });
  }

  async function fetchMembers() {
    const response = await MembersService.readAll("member");

    if (!response.ok) {
      alert("Error!");
      return;
    }

    setMembers(response.data.items);
  }

  useEffect(() => {
    //    fetchMembers();
    paginateMembers();
  }, [pageNumber, pageSize]);

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

  const handlePageSizeChange = (event) => {
    setPageSize(event.target.value);
    setPageNumber(1);
    console.log("PageSize" + pageSize);
  };

  const handlePageChange = (event, value) => {
    setPageNumber(value);
  };

  console.log("Activity status " + activityStatus);

  return (
    <>
      <Container className="primaryContainer">
        <Row>
          <h1>MEMBERS PAGE</h1>
        </Row>
        <Row>
          <Col>
            <CustomButton
              label="Create new"
              variant="primary"
              onClick={() => createMember()}
            ></CustomButton>
          </Col>
          <Col>
            <PageSizeDropdown
              onChanged={handlePageSizeChange}
              initValue={pageSize}
            ></PageSizeDropdown>
          </Col>
          <Col>
            <ActivityStatusSelection
              entities={statusOptions || []}
              value={statusOptions.indexOf(1)}
              onChanged={(e) => setActivityStatus(e.target.value)}
              atribute="Search by activity status"
            ></ActivityStatusSelection>
          </Col>
        </Row>
        <Row>
          <Col></Col>
          <Col></Col>
        </Row>
        <GenericTable
          dataArray={members}
          onDelete={deleteMember}
          onUpdate={handleUpdate}
          cutRange={2}
        ></GenericTable>
        <Pagination
          className="my-3"
          count={countPage}
          page={pageNumber}
          siblingCount={1}
          boundaryCount={1}
          variant="outlined"
          shape="rounded"
          onChange={handlePageChange}
        />
      </Container>
    </>
  );
}
