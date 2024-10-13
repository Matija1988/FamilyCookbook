using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Banner;
using Riok.Mapperly.Abstractions;
using static FamilyCookbook.REST_Models.Banner.BannerDTO;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class BannerMapper
    {
        public partial List<BannerRead> MapAllToBannerRead(List<Banner> banners);

        public partial BannerRead MapSingle(Banner banner);

        public partial Banner BannerCreateToBanner(BannerCreate bannerCreate);
    }
}
