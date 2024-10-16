import { Dropdown } from "bootstrap";
import { useEffect, useState } from "react";
import { DropdownButton } from "react-bootstrap";
import CustomButton from "../../../components/CustomButton";
import { App } from "../../../constants/constants";
import BannerService from "../../../services/BannerService";
import useLoading from "../../../hooks/useLoading";
import useError from "../../../hooks/useError";

export default function BannerSpan({ banners, onUpdate, onDelete }) {
  const [selectedPosition, setSelectedPosition] = useState({});
  const [bannerPositions, setBannerPositions] = useState([]);
  const positions = [
    { id: 0, name: "No position set" },
    { id: 1, name: "Right no 1" },
    { id: 2, name: "Left no 1" },
  ];

  const { showLoading, hideLoading } = useLoading();

  const { showError, showErrorModal, hideError, errors } = useError();

  async function fetchAllBannerPositions() {
    showLoading();
    const response = await BannerService.getAllBannerPositions();
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    setBannerPositions(response.data.items);
    hideLoading();
  }

  useEffect(() => {
    fetchAllBannerPositions();
  }, []);

  const getCurrentPosition = (bannerId) => {
    const foundPosition = bannerPositions.find((position) => {
      position.BannerId === bannerId;
    });
    return foundPosition ? foundPosition.Position : 0;
  };

  const URL = App.URL;
  const handlePositionChange = (bannerId, newPosition) => {
    setSelectedPosition({ ...selectedPosition, [bannerId]: newPosition });
  };

  async function setBannerToPosition(bannerId) {
    showLoading();
    const position = selectedPosition[bannerId] || getCurrentPosition(bannerId);
    const params = {
      BannerId: bannerId,
      Position: position,
    };
    const response = await BannerService.assignBannerToPosition(params);
    if (!response.ok) {
      hideLoading();
      showError(response.data);
    }
    hideLoading();
    fetchAllBannerPositions();
  }

  if (!banners || banners.length === 0) {
    return <p>No data to load</p>;
  }

  return (
    <>
      <div className="banner-table">
        {banners.map((banner) => (
          <div key={banner.id} className="banner-row">
            <div className="banner-image">
              <img src={URL + banner.location} alt={banner.name}></img>
            </div>

            <div className="banner-info">
              <span className="banner-name">{banner.name}</span>
              <a
                href={banner.destination}
                target="_blank"
                rel="noopener noreferrer"
              >
                {banner.destination}
              </a>
            </div>
            <div className="banner-type">Type: {banner.bannerType}</div>

            <div className="banner-position">
              <select
                value={
                  selectedPosition[banner.id] ?? getCurrentPosition(banner.id)
                }
                onChange={(e) =>
                  handlePositionChange(banner.id, parseInt(e.target.value))
                }
              >
                {positions.map((pos) => (
                  <option key={pos.id} value={pos.id}>
                    Postion {pos.name}
                  </option>
                ))}
              </select>
            </div>
            <div className="banner-actions">
              <CustomButton
                label="Update"
                onClick={() => setBannerToPosition(banner.id)}
                variant="primary"
              ></CustomButton>
              <CustomButton
                label="Delete"
                variant="danger"
                onClick={() => onDelete(banner.id)}
              ></CustomButton>
            </div>
          </div>
        ))}
      </div>
    </>
  );
}
