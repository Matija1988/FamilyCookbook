using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyCookbook.Common.Upload
{
    public static class PictureUpload
    {
        private const long MaxPictureSize = 1 * 1024 * 1024;

        private static readonly string[] AllowedExrensions = { ".jpg", ".jpeg", ".png" };

        public static IActionResult ValidatePictureSize(IFormFile file)
        {
            if(file.Length > MaxPictureSize)
            {
                return new 
                    BadRequestObjectResult($"File size exceeds the {MaxPictureSize / (1024 * 1024)} MB limit");
            }
            return new OkResult();
        }

        public static IActionResult ValidateFileExtension(IFormFile picture)
        {
            var fileExtension = Path.GetExtension(picture.FileName).ToLower();
            if (!AllowedExrensions.Contains(fileExtension))
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

    }
}
