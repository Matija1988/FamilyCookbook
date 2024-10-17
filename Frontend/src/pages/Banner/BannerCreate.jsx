import { Col, Container, Form, Row } from "react-bootstrap";
import Sidebar from "../AdminPanel/Sidebar";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import useLoading from "../../hooks/useLoading";
import useError from "../../hooks/useError";
import ErrorModal from "../../components/ErrorModal";
import SelectionDropdown from "../../components/SelectionDropdown";
import InputText from "../../components/InputText";
import CustomButton from "../../components/CustomButton";
import { RouteNames } from "../../constants/constants";
import BannerService from "../../services/BannerService";

export default function BannerCreate() {
  const [banner, setBanner] = useState({
    destination: "",
    imageName: "",
    imageBlob: "",
    bannerType: 0,
  });

  const [uploadedBanner, setUploadedBanner] = useState(null);

  const maxPictureSize = (1 * 1024 * 1024) / 4;
  const bannerTypes = [{ id: 1, name: "Small box right" }];

  const [error, setError] = useState(null);
  const { showLoading, hideLoading } = useLoading();
  const { showError, showErrorModal, hideError, errors } = useError();
  const [bannerName, setBannerName] = useState("");
  const [bannerTypeId, setBannerTypeId] = useState(null);
  const navigate = useNavigate();

  async function postBanner(entity, isMultiPart = false) {
    showLoading();
    const response = await BannerService.create(
      "Banner/create",
      entity,
      isMultiPart
    );
    if (!response.ok) {
      hideLoading();
      showError(response.data);
      return;
    }
    hideLoading();
    navigate(RouteNames.BANNER);
  }

  function handleSubmit(e) {
    e.preventDefault();

    const formData = new FormData(e.target);

    postBanner({
      destination: formData.get("Destination"),
      imageName: bannerName,
      bannerType: bannerTypeId,
      imageBlob: uploadedBanner,
    });
  }

  const handleBannerChange = (e) => {
    const file = e.target.files[0];
    setBannerName(file.name);

    const allowedTypes = ["image/jpeg", "image/jpg", "image/png"];
    const maxWidth = 300;
    const maxHeight = 300;

    if (file && !allowedTypes.includes(file.type)) {
      setError("Only JPEG, JPG and PNG files are allowed");
      setUploadedBanner(null);
      return;
    }

    const img = new Image();
    const objectURL = URL.createObjectURL(file);

    img.src = objectURL;

    img.onload = () => {
      const width = img.naturalWidth;
      const height = img.naturalHeight;

      if (width !== maxWidth || height !== maxHeight) {
        setError(`Invalid image resolution, must be ${maxWidth}x${maxHeight}}`);
        setUploadedBanner(null);
        URL.revokeObjectURL(objectURL);
        return;
      }
    };

    if (file && file.size > maxPictureSize) {
      setError(
        "Maximum file size is " + (maxPictureSize / (1024 * 1024) + " MB")
      );
      setUploadedBanner(null);
      return;
    }

    const reader = new FileReader();

    reader.onloadend = () => {
      setUploadedBanner(reader.result);
    };
    reader.readAsDataURL(file);

    URL.revokeObjectURL(objectURL);
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
            <Form onSubmit={handleSubmit} className="createForm">
              <Row>
                <Col>
                  <SelectionDropdown
                    atribute="Banner type"
                    entities={bannerTypes}
                    onChanged={(e) => setBannerTypeId(e.target.value)}
                  ></SelectionDropdown>
                </Col>
                <Col>
                  <InputText
                    atribute="Destination"
                    required={true}
                    value=""
                  ></InputText>
                  {error && <p style={{ color: "red" }}>{error}</p>}
                </Col>
                <Col></Col>
              </Row>
              <Row>
                <Col>
                  <input
                    type="file"
                    onChange={(e) => handleBannerChange(e)}
                  ></input>
                </Col>
                <Col>
                  <img src={uploadedBanner} style={{ width: "300px" }}></img>
                </Col>
                <Col></Col>
              </Row>
              <Row>
                <Col>
                  <CustomButton
                    label="SUBMIT"
                    type="submit"
                    variant="primary m3"
                  ></CustomButton>
                  <CustomButton
                    label="CANCEL"
                    onClick={() => navigate(RouteNames.BANNER)}
                    variant="secondary m-3"
                  ></CustomButton>
                </Col>
              </Row>
            </Form>
          </Container>
        </Col>
      </Row>
      <ErrorModal
        show={showErrorModal}
        onHide={hideError}
        errors={errors}
      ></ErrorModal>
    </>
  );
}
