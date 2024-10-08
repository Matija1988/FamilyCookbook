import { Col, Container, Row } from "react-bootstrap";
import Sidebar from "../AdminPanel/Sidebar";
import { useEffect, useState } from "react";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import TagsService from "../../services/TagsService";
import GenericTable from "../../components/GenericTable";
import { useNavigate } from "react-router-dom";
import DeleteModal from "../../components/DeleteModal";
import PageSizeDropdown from "../../components/PageSizeDropdown";
import CustomPagination from "../../components/CustomPagination";
import ErrorModal from "../../components/ErrorModal";
import GenericInputs from "../../components/GenericInputs";
import CustomButton from "../../components/CustomButton";
import TagsUpsert from "./TagsUpsert";

export default function Tags() {
  const [tags, setTags] = useState([]);
  const [newText, setText] = useState([]);
  const [searchByText, setSearchByText] = useState("");
  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [entityToDelete, setEntityToDelete] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(null);
  const [selectedTag, setSelectedTag] = useState(null);
  const navigate = useNavigate();

  const { showLoading, hideLoading } = useLoading();

  const { showError, hideError, showErrorModal, errors } = useError();

  const getRequestParams = (pageSize, pageNumber, text) => {
    let params = {};
    if (pageSize) {
      params["PageSize"] = pageSize;
    }
    if (pageNumber) {
      params["PageNumber"] = pageNumber;
    }
    if (text) {
      params["text"] = text;
    }
    return params;
  };

  async function paginateTags() {
    const params = getRequestParams(pageSize, pageNumber, searchByText);

    const response = await TagsService.paginate(params);
    showLoading();
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    const { items, pageCount } = response.data;

    setTags(items);
    setTotalPages(pageCount);
    hideLoading();
  }

  useEffect(() => {
    paginateTags();
  }, [pageNumber, pageSize]);

  async function handleUpdate(tag) {
    setSelectedTag(tag);
    console.log("Selected tag:", selectedTag);
  }

  async function deleteTag(tag) {
    showLoading();
    const response = await TagsService.deleteEntity("tag/delete/" + tag.id);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    setShowDeleteModal(false);
    paginateTags();
    hideLoading();
  }

  const handlePageSizeChange = (e) => {
    setPageSize(e.target.value);
    setPageNumber(1);
  };

  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  const onSearchTextChange = (e) => {
    setSearchByText(e.target.value);
  };

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={8}>
          <Container className="primaryContainer">
            <h1>Tags</h1>
            <Row>
              <Col>
                <PageSizeDropdown
                  initValue={pageSize}
                  onChanged={handlePageSizeChange}
                ></PageSizeDropdown>
              </Col>
              <Col>
                <GenericInputs
                  atribute="Text"
                  type="text"
                  value=""
                  onChange={onSearchTextChange}
                ></GenericInputs>
              </Col>
              <Col>
                <CustomButton
                  label="Search"
                  onClick={paginateTags}
                  className="search-btn"
                ></CustomButton>
              </Col>
            </Row>
            <Row>
              <Col>
                <GenericTable
                  dataArray={tags}
                  onUpdate={handleUpdate}
                  onDelete={(tag) => {
                    setEntityToDelete(tag), setShowDeleteModal(true);
                  }}
                  className="gen-tbl"
                  cutRange={1}
                ></GenericTable>
              </Col>
              <Col>
                <TagsUpsert
                  entity={selectedTag}
                  paginate={paginateTags}
                ></TagsUpsert>
              </Col>
            </Row>
            <CustomPagination
              pageNumber={pageNumber}
              totalPages={totalPages}
              handlePageChange={handlePageChange}
            ></CustomPagination>
          </Container>
        </Col>
      </Row>
      <DeleteModal
        show={showDeleteModal}
        handleClose={() => setShowDeleteModal(false)}
        handleDelete={deleteTag}
        entity={entityToDelete}
      ></DeleteModal>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
