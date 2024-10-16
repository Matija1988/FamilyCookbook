using FamilyCookbook.Common.Filters;
using FamilyCookbook.Common;
using FamilyCookbook.Model;

namespace FamilyCookbook.Service.Common
{
    public interface IBannerService : IService<Banner>
    {
        Task<MessageResponse> CreateAsync(Banner banner);
        Task<RepositoryResponse<List<Banner>>> PaginateAsync(Paging paging, BannerFilter filter);
    }
}
