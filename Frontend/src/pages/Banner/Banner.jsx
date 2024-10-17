import { Container, Row, Col } from "react-bootstrap";
import GenericTable from "../../components/GenericTable";
import Sidebar from "../AdminPanel/Sidebar";
import { useEffect, useState } from "react";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import BannerService from "../../services/BannerService";
import DeleteModal from "../../components/DeleteModal";
import CustomButton from "../../components/CustomButton";
import { useNavigate } from "react-router-dom";
import { RouteNames } from "../../constants/constants";
import PageSizeDropdown from "../../components/PageSizeDropdown";
import CustomPagination from "../../components/CustomPagination";
import InputText from "../../components/InputText";
import BannerSpan from "../Banner/bannerComponents/BannerSpan";

export default function Banner() {
  const [banners, setBanners] = useState([]);
  const [entityToDelete, setEntityToDelete] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageCount, setPageCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const statusOptions = [
    { id: 1, name: "Active" },
    { id: 2, name: "Not active" },
  ];

  const [activityStatus, setActivityStatus] = useState(1);
  const [searchByDestination, setSearchByDestination] = useState("");
  const [searchByName, setSearchByName] = useState("");

  const { showLoading, hideLoading } = useLoading();

  const { showError, showErrorModal, hideError, errors } = useError();
  const navigate = useNavigate();

  const getRequestParams = (pageSize, pageNumber, activityStatus) => {
    let params = {};

    if (pageSize) {
      params["PageSize"] = pageSize;
    }
    if (pageNumber > 0) {
      params["PageNumber"] = pageNumber;
    }
    if (activityStatus) {
      params["SearchByActivityStatus"] = activityStatus;
    }
    if (searchByDestination) {
      params["SearchByDestination"] = searchByDestination;
    }
    if (searchByName) {
      params["SearchByName"] = searchByName;
    }
    return params;
  };

  async function paginateBanners() {
    showLoading();
    const params = getRequestParams(pageSize, pageNumber, activityStatus);
    const response = await BannerService.paginate("Banner/paginate", params);

    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    const { items, pageCount } = response.data;
    setBanners(items);
    setTotalPages(pageCount);
    hideLoading();
  }

  async function deleteBanner(id) {}

  useEffect(() => {
    paginateBanners();
  }, [pageNumber, pageSize]);

  function createBanner() {
    navigate(RouteNames.BANNER_CREATE);
  }

  async function deleteTag() {}

  const handleUpdate = () => {};

  const handlePageSizeChange = (e) => {
    setPageSize(e.target.value);
    setPageNumber(1);
  };
  const handlePageChange = (value) => {
    setPageNumber(value);
  };

  const onSearchByDestination = (e) => {
    const destinationSearch = e.target.value;
    setSearchByDestination(destinationSearch);
  };

  const onSearchByName = (e) => {
    const nameSearch = e.target.value;
    setSearchByName(nameSearch);
  };

  return (
    <>
      <Row>
        <Col md={2}>
          <Sidebar></Sidebar>
        </Col>
        <Col md={9}>
          <Container className="primaryContainer">
            <h1>BANNER</h1>
            <Row>
              <Col>
                <CustomButton
                  label="Create new"
                  variant="primary"
                  onClick={() => createBanner()}
                  className="create-new-btn"
                ></CustomButton>
              </Col>
              <Col>
                <InputText
                  atribute="Search by destination..."
                  type="text"
                  value=""
                  onChange={onSearchByDestination}
                ></InputText>
              </Col>
              <Col>
                <InputText
                  atribute="Search by name"
                  type="text"
                  value=""
                  onChange={onSearchByName}
                ></InputText>
              </Col>
              <Col>
                <PageSizeDropdown
                  onChanged={handlePageSizeChange}
                  initValue={pageSize}
                ></PageSizeDropdown>
              </Col>
              <Col>
                <br></br>
                <CustomButton
                  label="Search"
                  onClick={paginateBanners}
                  className="search-btn"
                ></CustomButton>
              </Col>
              <Row>
                <br></br>
              </Row>
            </Row>
            <Row>
              <BannerSpan
                banners={banners}
                onUpdate={handleUpdate}
                onDelete={deleteBanner}
              ></BannerSpan>
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
        handleDelete={deleteBanner}
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
