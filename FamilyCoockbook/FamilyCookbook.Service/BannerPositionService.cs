using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class BannerPositionService : IBannerPositionService
    {
        private readonly IBannerPositionRepository _repository;

        public BannerPositionService(IBannerPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition)
        {
            return await _repository.AssignBannerToPosition(bannerPosition);
        }

        public async Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions()
        {
            return await _repository.GetAllBannerPositions();
        }

        public async Task<RepositoryResponse<Banner>> GetBannerForPosition(int position)
        {
            return await _repository.GetBannerForPosition(position);
        }
    }
}
