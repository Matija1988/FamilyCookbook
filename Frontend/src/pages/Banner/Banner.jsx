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

export default function Banner() {
  const [banners, setBanners] = useState([]);
  const [entityToDelete, setEntityToDelete] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const { showLoading, hideLoading } = useLoading();

  const { showError, showErrorModal, hideError, errors } = useError();
  const navigate = useNavigate();

  async function fetchBanners() {
    showLoading();
    const response = await BannerService.readAll("banner");
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    setBanners(response.data.items);
    hideLoading();
  }
  async function deleteBanner(id) {}

  useEffect(() => {
    fetchBanners();
  }, []);

  function createBanner() {
    navigate(RouteNames.BANNER_CREATE);
  }

  async function deleteTag() {}

  const handleUpdate = () => {};

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
                ></CustomButton>
              </Col>
            </Row>
            <GenericTable
              onUpdate={handleUpdate}
              onDelete={(banner) => {
                setEntityToDelete(banner), setShowDeleteModal(true);
              }}
              dataArray={banners}
              cutRange={1}
            ></GenericTable>
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
