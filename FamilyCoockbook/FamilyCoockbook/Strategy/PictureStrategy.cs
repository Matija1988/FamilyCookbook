using FamilyCookbook.Common.Enums;
using FamilyCookbook.Common.Upload;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Banner;
using FamilyCookbook.REST_Models.Picture;
using FamilyCookbook.REST_Models.Recipe;

namespace FamilyCookbook.Strategy
{
    public class PictureStrategy : IImageStrategy
    {
        public async Task<Image>
            UploadImage(ImageDTO imageToUpload, Image? chkPicture, string webRootPath, ImageEnum imageType, long imageSize)
        {
            var dto = imageToUpload;

            var fileExtension = "";
            byte[] imageBytes = null;

            bool chkBlob = string.IsNullOrEmpty(dto.ImageBlob);

            var folderName = ImageUtilities.DetermineUploadFolder(imageType);

            if (ImageUtilities.ValidatePictureSizeFunc(chkBlob, dto.ImageBlob, imageSize))
            {
                return null;
            }

            if (!chkBlob)
            {
                var dataParts = ImageUtilities.Base64DataParts(dto.ImageBlob);
                var mimeType = ImageUtilities.GetMimeType(dataParts, 0);
                fileExtension = ImageUtilities.ValidateFileExtensionFunc(mimeType);
                imageBytes = ImageUtilities.ConvertBase64ToByteArray(dataParts, 1);
            }

            var uploadsFolder = ImageUtilities.GetUploadsFolder(webRootPath, folderName);

            if (imageType is ImageEnum.Picture)
            {
                var intermediaryPicture = ImageUtilities.IntermediaryPicture(imageToUpload.ImageName,
                    await ImageUtilities.ChcPictureNullThenUpload(chkPicture,
                    imageToUpload.ImageName, fileExtension, uploadsFolder, folderName, imageBytes));
                return intermediaryPicture;
            }
            if (imageType is ImageEnum.SmallBox)
            {
                var imtermediaryBanner = ImageUtilities.IntermediaryBanner(imageToUpload.ImageName,
                    await ImageUtilities.ChcPictureNullThenUpload(chkPicture, imageToUpload.ImageName,
                    fileExtension, uploadsFolder, folderName, imageBytes));
                return imtermediaryBanner;
            }

            return null;
        }


    }
}
