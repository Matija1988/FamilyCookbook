using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Validations
{
    public static class PictureValidations
    {
        public static IActionResult ValidatePicture(IFormFile picture)
        {
            if (picture == null || picture.Length == 0)
            {
                return new BadRequestObjectResult("No picture uploaded");
            }

            long maxFileSize = 1 * 1024 * 1024;
            if (picture.Length > maxFileSize)
            {
                return new BadRequestObjectResult("File size exceeds the 1 MB limit!!!");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(picture.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return new BadRequestObjectResult("Only JPG, JPEG and PNG files are supported!");
            }

            return new OkResult();

        }
    }
}
