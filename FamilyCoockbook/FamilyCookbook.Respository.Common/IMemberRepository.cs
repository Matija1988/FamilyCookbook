using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface IMemberRepository : IRepository<Member, MemberFilter>
    {
        Task<MessageResponse> CreateAsync(Member entity);
        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);

        Task<RepositoryResponse<Lazy<Member>>> FindByUsernameAsync(string username);    
    }
}
