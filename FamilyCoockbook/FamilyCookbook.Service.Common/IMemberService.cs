using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IMemberService : IService<Member>
    {
        Task<RepositoryResponse<Member>> PermaDeleteAsync(int id);

        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);
    }
}
