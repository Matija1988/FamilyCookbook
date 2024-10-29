namespace FamilyCookbook.Service.Common
{
    public interface IBannerPositionService
    {
        Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition);
        Task<RepositoryResponse<Banner>> GetBannerForPosition(int position);
        Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions();
    }
}
