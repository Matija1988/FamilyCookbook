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
    public interface IMemberRepository : IRepository<Member>
    {
        Task<RepositoryResponse<Member>> PermaDeleteAsync(int id);

        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);

        Task<RepositoryResponse<List<Member>>> PaginateAsync(Paging paging);
    }
}
