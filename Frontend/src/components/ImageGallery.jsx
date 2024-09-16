import { Container } from "react-bootstrap";
import PictureService from "../services/PictureService";
import { useEffect, useState } from "react";

import "./ImageGallery.css";

export default function ImageGallery({ isOpen, closeModal }) {
  const [pictures, setPictures] = useState([]);
  const [error, setError] = useState("");
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

  return (
    <>
      <div className="modal-overlay" onClick={closeModal}>
        <div
          className="modal-contnent"
          onClick={(e) => e.stopPropagation()}
        ></div>
        <button className="close-modal-btn" onClick={closeModal}>
          x
        </button>
        <h2>Image gallery</h2>
        <div className="gallery">
          {pictures.map((picture, index) => (
            <img
              src={URL + picture.location}
              key={index}
              className="gallery-image"
              alt={`Gallery ${index}`}
            ></img>
          ))}
        </div>
      </div>
    </>
  );
}
