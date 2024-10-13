using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Banner
{
    public class BannerDTO
    {
        public sealed record BannerRead(int Id, string Location, string Destination,
            string DestinationName, string BannerType);

        public sealed record BannerCreate(
            [Required, StringLength(255, ErrorMessage = "The location url exceeds maximum allowed number of characters: 255")]
            string Location,
            [Required, StringLength(255, ErrorMessage = "The destination url exceeds maximum allowed number of characters: 255")]
            string Destination,
            [Required, StringLength(30, ErrorMessage = "The input exceeds maximum allowed number of characters: 30")]
            string DestinationName,
            string PictureBlob,
            [Required]
            int BannerType);
    }
}
