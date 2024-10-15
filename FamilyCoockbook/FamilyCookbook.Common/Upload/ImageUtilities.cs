using Autofac.Core;
using FamilyCookbook.Common.Enums;
using FamilyCookbook.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyCookbook.Common.Upload
{
    public static class ImageUtilities
    {

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public static IActionResult ValidatePictureSize(IFormFile file, int size)
        {
            var maxPictureSize  = size * 1024 * 1024;
            if(file.Length > maxPictureSize)
            {
                return new 
                    BadRequestObjectResult($"File size exceeds the {maxPictureSize / (1024 * 1024)} MB limit");
            }
            return new OkResult();
        }

        public static IActionResult ValidateFileExtension(IFormFile picture)
        {
            var fileExtension = Path.GetExtension(picture.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
            {
                return new BadRequestObjectResult("Only JPG, JPEG and PNG picture formats are allowed!!!");

            }
            return new OkResult();
        }

        public static async Task<string> SavePictureAsync(IFormFile picture, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return filePath;
        }
        public static async Task<string> ChcPictureNullThenUpload(Image? chkPicture, string pictureName, string fileExtension, string uploadsFolder, string relativePath, byte[]? imageBytes)
        {
            if (chkPicture is null && imageBytes != null)
            {
                var fileName = pictureName + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);
                relativePath = Path.Combine("uploads", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                return relativePath;
            }
            else if (chkPicture is not null)
            {
                relativePath = chkPicture.Location;
                return relativePath;
            }

            return relativePath;
        }
        public static Picture IntermediaryPicture(string pictureName, string relativePath)
        {
            Picture intermeadiaryPicture = new Picture();

            intermeadiaryPicture.Name = pictureName;
            intermeadiaryPicture.Location = relativePath;

            return intermeadiaryPicture;
        }

        public static Func<bool, string?, long, bool> ValidatePictureSizeFunc = (isNullOrEmpty, base64String, size) =>
        {
            if (!isNullOrEmpty)
            {
                string dataPrefix = "base64,";
                string base64Data = base64String?.Substring(base64String.IndexOf(dataPrefix) + dataPrefix.Length);
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                return imageBytes.Length > size;
            }
            return false;
        };

        public static Func<ImageEnum, string> DetermineUploadFolder = (imageType) =>
        {
            var folderName = imageType switch
            {
                ImageEnum.Picture => "uploads",
                ImageEnum.SmallBox => "boxbanners",
                ImageEnum.LargeBanner => "largeBanners",
                _ => throw new NotImplementedException("Cannot determine upload folder!!!")
            };
            return folderName;
        };

        public static Func<string?, string[]?> Base64DataParts = (base64String) =>
        {
            if (!string.IsNullOrEmpty(base64String)) { return base64String.Split(','); }
            return null;
        };

        public static Func<string[], int, byte[]> ConvertBase64ToByteArray = (dataParts, index) =>
        {
            var base64Data = dataParts[index];
            return Convert.FromBase64String(base64Data);
        };

        public static Func<string[], int, string> GetMimeType = (base64DataParts, index) =>
        {
            return base64DataParts[index];
        };

        public static Func<string?, string> ValidateFileExtensionFunc = (base64String) =>
        {
            var fileExtension = base64String switch
            {
                "data:image/jpg;base64" => ".jpg",
                "data:image/png;base64" => ".png",
                "data:image/jpeg;base64" => ".jpg",
                _ => throw new InvalidDataException("Only JPG, PNG or JPEG can be uploaded!!!")
            };
            return fileExtension;
        };

        public static Func<string, string, string> GetUploadsFolder = (webRootPath, folderName) =>
        {
            var uploadsFolder = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            return uploadsFolder;
        };

        public static Func<string, string, Banner> IntermediaryBanner = (bannerName, relativePath) =>
        {
            Banner banner = new();
            banner.Name = bannerName;
            banner.Location = relativePath;
            return banner;
        };

    }
}
