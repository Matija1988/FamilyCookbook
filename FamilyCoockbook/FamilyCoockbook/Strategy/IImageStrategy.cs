using FamilyCookbook.Model;

namespace FamilyCookbook.Strategy
{
    public interface IImageStrategy
    {
        Task<Picture> UploadImage(ImageDTO image, Picture picture, string webRootPath);
    }
}
