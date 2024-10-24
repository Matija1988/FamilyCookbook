﻿using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IBannerPositionService
    {
        Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition);
        Task<RepositoryResponse<Banner>> GetBannerForPosition(int position);
        Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions();
    }
}
