using FamilyCookbook.Common.Enums;
namespace FamilyCookbook.Strategy
{
    public interface IImageStrategy
    {
        Task<Image> UploadImage(ImageDTO image, Image? picture, string webRootPath, ImageEnum imageType, long imageSize);
    }
}
