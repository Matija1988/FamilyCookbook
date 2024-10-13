using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Banner
{
    public class BannerDTO
    {
        public sealed record BannerRead(int Id, string Location, string Destination,
            string DestinationName, string BannerType);

    }
}
