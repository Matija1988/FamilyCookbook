using FamilyCookbook.Common.Filters;
using FamilyCookbook.Common;
using FamilyCookbook.Model;

namespace FamilyCookbook.Service.Common
{
    public interface IBannerService : IService<Banner, BannerFilter>
    {
        Task<MessageResponse> CreateAsync(Banner banner);
        Task<RepositoryResponse<Lazy<List<Banner>>>> PaginateAsync(Paging paging, BannerFilter filter);
    }
}
