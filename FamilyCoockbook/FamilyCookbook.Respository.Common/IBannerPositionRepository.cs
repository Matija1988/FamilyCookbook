namespace FamilyCookbook.Repository.Common
{
    public interface IBannerPositionRepository
    {
        Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition);
        Task<RepositoryResponse<Banner>> GetBannerForPosition(int position);
        Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions();
    }
}
