using FamilyCookbook.Common.Upload;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Picture;
using FamilyCookbook.REST_Models.Recipe;

namespace FamilyCookbook.Strategy
{
    public class PictureStrategy : IImageStrategy
    {
        public async Task<Picture> UploadImage(ImageDTO imageToUpload, Picture? chkPicture, string webRootPath)
        {
            var dto = imageToUpload as RecipeCreate;
            var folderName = "uploads";
            var fileExtension = "";
            var relativePath = "";
            byte[] imageBytes = null;

            bool chkBlob = string.IsNullOrEmpty(dto.ImageBlob);

            if (ImageUtilities.ValidatePictureSizeFunc(chkBlob, dto.ImageBlob, 1))
            {
                return null;
            }
                
            if(!chkBlob) 
            {
                var dataParts = ImageUtilities.Base64DataParts(dto.ImageBlob);
                var mimeType = ImageUtilities.GetMimeType(dataParts, 0);
                fileExtension = ImageUtilities.ValidateFileExtensionFunc(mimeType);
                imageBytes = ImageUtilities.ConvertBase64ToByteArray(dataParts, 1);
            }
            
            var uploadsFolder = ImageUtilities.GetUploadsFolder(webRootPath, folderName);

            var intermediaryPicture = ImageUtilities.IntermediaryPicture(imageToUpload.ImageName, 
                await ImageUtilities.ChcPictureNullThenUpload(chkPicture, 
                imageToUpload.ImageName, fileExtension, uploadsFolder, relativePath, imageBytes));

            return intermediaryPicture;

        }
    }
}
