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

export default function Tags() {
  const [tags, setTags] = useState([]);
  const [newText, setText] = useState([]);
  const [searchByText, setSearchByText] = "";
  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(0);

  const [entityToDelete, setEntityToDelete] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(null);

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
      params["Text"] = text;
    }
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
  }

  useEffect(() => {
    paginateTags();
  }, [pageNumber, pageSize]);

  async function handleUpdate(tag) {}

  async function deleteTag(tag) {}

  const handlePageSizeChange = (event) => {};

  return (
    <>
      <Sidebar></Sidebar>
      <Container className="primaryContainer">
        <h1>Tags</h1>
        <Row>
          <Col></Col>
          <Col></Col>
          <Col>
            <PageSizeDropdown
              initValue={pageSize}
              onChanged={handlePageSizeChange}
            ></PageSizeDropdown>
          </Col>
        </Row>
        <GenericTable
          dataArray={tags}
          onDelete={(tag) => {
            setEntityToDelete(tag), setShowDeleteModal(true);
          }}
          onUpdate={handleUpdate}
          className="gen-tbl"
        ></GenericTable>
      </Container>
      <DeleteModal
        show={showDeleteModal}
        handleClose={() => setShowDeleteModal(false)}
        hanldeDelete={deleteTag}
        entity={entityToDelete}
      ></DeleteModal>
    </>
  );
}
