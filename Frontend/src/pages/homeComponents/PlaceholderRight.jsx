import { useEffect, useState } from "react";
import BannerService from "../../services/BannerService";
import { App } from "../../constants/constants";

export default function PlaceholderRight() {
  const bannerState = {
    id: "",
    location: "",
    destination: "",
    name: "",
    bannerType: "",
  };
  const [banner, setBanner] = useState(bannerState);

  const position = 1;
  const URL = App.URL;

  async function getBanner(position) {
    try {
      const response = await BannerService.getBannerForPosition(1);
      setBanner(response.data);
    } catch (e) {
      console.log("BANNER ERROR ", e);
    }
  }

  useEffect(() => {
    getBanner(1);
  }, []);

  return (
    <>
      <div className="banner-placeholder">
        <img
          src={URL + banner.location}
          style={{ cursor: "pointer" }}
          onClick={() => window.open(banner.destination, "_blank")}
        ></img>
      </div>
    </>
  );
}
