import { Container } from "react-bootstrap";
import PictureService from "../services/PictureService";
import { useEffect, useState } from "react";

import "./ImageGallery.css";

export default function ImageGallery({ isOpen, closeModal, setMainImage }) {
  const [pictures, setPictures] = useState([]);
  const [error, setError] = useState("");
  const [selectedImage, setSelectedImage] = useState(null);
  const [fullScreenImage, setFullScreenImage] = useState(null);
  const URL = "https://localhost:7170/";

  async function fetchPictures() {
    try {
      const response = await PictureService.readAll("picture");
      if (response.ok) {
        setPictures(response.data.items);
      }
    } catch (error) {
      setError(error.message);
    }
  }

  useEffect(() => {
    if (isOpen) {
      fetchPictures();
    }
  }, [isOpen]);

  if (!isOpen) {
    return null;
  }

  const openFullscreen = (image) => {
    setFullScreenImage(image);
  };

  const closeFullscreen = () => {
    setFullScreenImage(null);
  };

  const handleSetMainImage = () => {
    if (selectedImage) {
      setMainImage(selectedImage);
    }
  };

  return (
    <>
      {fullScreenImage && (
        <div className="fullscreen-overlay" onClick={closeFullscreen}>
          <img
            src={URL + fullScreenImage.location}
            className="fullscreen-image"
            alt="Fullscreen"
            onClick={(e) => e.stopPropagation()}
          ></img>
        </div>
      )}
      <div className="modal-overlay" onClick={(e) => e.stopPropagation()}>
        <div
          className="modal-contnent"
          onClick={(e) => e.stopPropagation()}
        ></div>
        <div className="image-glry-sdbr">
          <h3>Choose main image</h3>
          <button className="set-main-image-btn" onClick={handleSetMainImage}>
            Set main image
          </button>
        </div>

        <h2>Image gallery</h2>
        <div className="gallery">
          {pictures.map((picture, index) => (
            <img
              src={URL + picture.location}
              key={index}
              className={`gallery-image ${
                selectedImage === picture ? "selected" : ""
              }`}
              alt={`Gallery ${index}`}
              onClick={() => setSelectedImage(picture)}
              onDoubleClick={() => openFullscreen(picture)}
            ></img>
          ))}
        </div>
        <button className="close-modal-btn" onClick={closeModal}>
          x
        </button>
      </div>
    </>
  );
}
