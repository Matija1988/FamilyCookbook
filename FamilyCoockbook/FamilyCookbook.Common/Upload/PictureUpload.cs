using Autofac.Core;
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
    public static class PictureUpload
    {
        private const long MaxPictureSize = 1 * 1024 * 1024;

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public static IActionResult ValidatePictureSize(IFormFile file)
        {
            if(file.Length > MaxPictureSize)
            {
                return new 
                    BadRequestObjectResult($"File size exceeds the {MaxPictureSize / (1024 * 1024)} MB limit");
            }
            return new OkResult();
        }

        public static Func<string, bool> ValidatePictureSizeFunc = (base64String) =>
        {
            var dataPrefix = "base64,";
            var base64Data = base64String.Substring(base64String.IndexOf(dataPrefix)  + dataPrefix.Length);
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            return imageBytes.Length > MaxPictureSize;
        };
        public static IActionResult ValidateFileExtension(IFormFile picture)
        {
            var fileExtension = Path.GetExtension(picture.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
            {
                return new BadRequestObjectResult("Only JPG, JPEG and PNG picture formats are allowed!!!");

            }
            return new OkResult();
        }

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
        public static async Task<string> SavePictureAsync(IFormFile picture, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return filePath;
        }
        public static async Task<string> ChcPictureNullThenUpload(Picture? image, string pictureName, string fileExtension, string uploadsFolder, string relativePath, byte[]? imageBytes)
        {
            if (image is null && imageBytes != null)
            {
                var fileName = pictureName + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);
                relativePath = Path.Combine("uploads", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                return relativePath;
            }
            else if (image is not null)
            {
                relativePath = image.Location;
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
    }
}
