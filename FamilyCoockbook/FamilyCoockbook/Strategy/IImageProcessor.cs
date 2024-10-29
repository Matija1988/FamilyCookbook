namespace FamilyCookbook.Strategy
{
    public interface IImageProcessor
    {
        Task<Image> DelegateStrategy(ImageDTO imageDTO, Image image, string weebRootPath, ImageEnum imageType);
    }
}
