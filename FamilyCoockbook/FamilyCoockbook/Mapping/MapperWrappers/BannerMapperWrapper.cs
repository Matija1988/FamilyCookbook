using FamilyCookbook.Model;
using static FamilyCookbook.REST_Models.Banner.BannerDTO;

namespace FamilyCookbook.Mapping.MapperWrappers
{
    public class BannerMapperWrapper : IMapper<Banner, BannerRead, BannerCreate>
    {
        private readonly BannerMapper _mapper = new();
        public BannerRead MapReadToDto(Banner entity)
        {
            return _mapper.MapSingle(entity);
        }

        public Banner MapToEntity(BannerCreate dto)
        {
            return _mapper.BannerCreateToBanner(dto);
        }

        public List<BannerRead> MapToReadList(List<Banner> entities)
        {
            return _mapper.MapAllToBannerRead(entities);
        }
    }
}
