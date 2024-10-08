import { Col, Container, Form, Pagination, Row, Table } from "react-bootstrap";
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

import "bootstrap/dist/css/bootstrap.min.css";
import InputText from "../../components/InputText";
import GenericInputs from "../../components/GenericInputs";
import RoleService from "../../services/RoleService";

import "./memberWorkspace.css";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import DeleteModal from "../../components/DeleteModal";
import Sidebar from "../AdminPanel/Sidebar";
import { useUser } from "../../contexts/UserContext";

export default function Members() {
  const [members, setMembers] = useState();
  const [roles, setRoles] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [countPage, setCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [searchByFirstName, setSearchByFirstName] = useState("");
  const [searchByLastName, setSearchByLastName] = useState("");
  const [searchByRoleId, setSearchByRoleId] = useState();
  const [searchByBio, setSearchByBio] = useState("");
  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 0, name: "Not active" },
  ];

  const [activityStatus, setActivityStatus] = useState(1);
  const { hideLoading, showLoading } = useLoading();
  const { showError, showErrorModal, errors, hideError } = useError();
  const [entityToDelete, setEntityToDelete] = useState(null);

  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const { userId } = useUser();

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
    if (searchByFirstName) {
      params["SearchByFirstName"] = searchByFirstName;
    }
    if (searchByLastName) {
      params["SearchByLastName"] = searchByLastName;
    }
    if (searchByRoleId) {
      params["SearchByRoleId"] = searchByRoleId;
    }
    if (searchByBio) {
      params["SearchByBio"] = searchByBio;
    }
    return params;
  };

  async function paginateMembers() {
    const params = getRequestParams(pageSize, pageNumber, activityStatus);

    const response = await MembersService.paginate(params);
    showLoading();
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    const { items, pageCount } = response.data;

    const formattedItems = items.map((member) => ({
      ...member,
      dateOfBirth: formatDate(member.dateOfBirth),
    }));

    setMembers(formattedItems);
    setTotalPages(pageCount);
    hideLoading();
  }

  async function fetchRoles() {
    const response = await RoleService.readAll("role");
    if (!response.ok) {
      return;
    }
    const tempRoles = response.data;
    tempRoles.splice(3);
    setRoles(tempRoles);
  }

  useEffect(() => {
    paginateMembers();
    fetchRoles();
  }, [pageNumber, pageSize]);

  async function deleteMember(member) {
    if (userId == member.id) {
      alert("User cannot delete himself");
      return;
    }

    const response = await MembersService.setNotActive(
      "member/softDelete",
      member.id
    );
    if (response.ok) {
      paginateMembers();
      setShowDeleteModal(false);
    } else {
      showError(response.data);
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

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  const onChangeSearchFirstName = (e) => {
    const searchFirstName = e.target.value;
    setSearchByFirstName(searchFirstName);
  };

  const onChangeSearchLastName = (e) => {
    const searchLastName = e.target.value;
    setSearchByLastName(searchLastName);
  };

  const onChangeSearchBio = (e) => {
    const bio = e.target.value;
    setSearchByBio(bio);
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  };

  return (
    <>
      <Sidebar></Sidebar>
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
              className="create-new-btn"
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
          <Col>
            <GenericInputs
              type="text"
              atribute="Search by first name"
              value=""
              onChange={onChangeSearchFirstName}
            ></GenericInputs>
          </Col>
        </Row>
        <Row>
          <Col>
            <GenericInputs
              type="text"
              atribute="Search by last name"
              value=""
              onChange={onChangeSearchLastName}
            ></GenericInputs>
          </Col>
          <Col>
            <SelectionDropdown
              atribute="Search by role"
              entities={roles || []}
              onChanged={(e) => setSearchByRoleId(e.target.value)}
            ></SelectionDropdown>
          </Col>
          <Col>
            <GenericInputs
              type="text"
              atribute="Search by bio"
              value=""
              onChange={onChangeSearchBio}
            ></GenericInputs>
          </Col>
          <Col>
            <CustomButton
              label="Search"
              onClick={paginateMembers}
              className="search-btn-2"
            ></CustomButton>
          </Col>
        </Row>

        <GenericTable
          dataArray={members}
          onDelete={(member) => (
            setEntityToDelete(member), setShowDeleteModal(true)
          )}
          onUpdate={handleUpdate}
          cutRange={2}
          className="gen-tbl"
        ></GenericTable>

        <CustomPagination
          pageNumber={pageNumber}
          totalPages={totalPages}
          handlePageChange={handlePageChange}
        ></CustomPagination>
      </Container>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
      <DeleteModal
        show={showDeleteModal}
        handleClose={() => setShowDeleteModal(false)}
        handleDelete={deleteMember}
        entity={entityToDelete}
      ></DeleteModal>
    </>
  );
}
