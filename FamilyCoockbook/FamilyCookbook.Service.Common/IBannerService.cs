using FamilyCookbook.Model;

namespace FamilyCookbook.Service.Common
{
    public interface IBannerService : IService<Banner>
    {
        Task<MessageResponse> CreateAsync(Banner banner);
    }
}
