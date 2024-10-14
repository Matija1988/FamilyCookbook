using FamilyCookbook.Common.Enums;
using FamilyCookbook.Model;

namespace FamilyCookbook.Strategy
{
    public interface IImageProcessor
    {
        Task<Image> DelegateStrategy(ImageDTO imageDTO, Image image, string weebRootPath, ImageEnum imageType);
    }
}
