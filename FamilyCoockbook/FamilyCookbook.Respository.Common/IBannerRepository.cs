using FamilyCookbook.Common.Filters;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface IBannerRepository : IRepository<Banner>
    {
        Task<MessageResponse> CreateAsync(Banner banner);

        Task<RepositoryResponse<List<Banner>>> PaginateAsync(Paging paging, BannerFilter filter);
    }
}
