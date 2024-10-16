using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface IBannerPositionRepository
    {
        Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition);

        Task<RepositoryResponse<Banner>> GetBannerForPosition(int position);

        Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions();
    }
}
