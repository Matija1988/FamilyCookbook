using FamilyCookbook.Common.Enums;
using FamilyCookbook.Model;

namespace FamilyCookbook.Strategy
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageStrategy _imageStrategy;
        public ImageProcessor(IImageStrategy imageStrategy)
        {
            _imageStrategy = imageStrategy;
        }
        public async Task<Image> DelegateStrategy
            (ImageDTO imageDTO, Image image, string weebRootPath, ImageEnum imageType)
        {
            var maxSize = 1 * 1024 * 1024; ;
            var type = imageType switch
            {
                ImageEnum.Picture => await _imageStrategy.UploadImage
                (imageDTO, image, weebRootPath, ImageEnum.Picture, maxSize),
              
                ImageEnum.SmallBox => await _imageStrategy.UploadImage
                (imageDTO, image, weebRootPath, ImageEnum.SmallBox, maxSize / 4),
                
                ImageEnum.LargeBanner => await _imageStrategy.UploadImage
                (imageDTO, image, weebRootPath, ImageEnum.LargeBanner, maxSize / 4),
                _ => throw new NotImplementedException("Unknown image as input in image processor!")
            };

            return type;
        }
    }
}
