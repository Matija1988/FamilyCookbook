using FamilyCookbook.Common;
using FamilyCookbook.Model;


namespace FamilyCookbook.Service.Common
{
    public interface IMemberService : IService<Member>
    {
        
        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);

        Task<RepositoryResponse<List<Member>>> PaginateAsync(Paging paging, MemberFilter filter);

        Task<RepositoryResponse<List<Member>>> SearchMemberByCondition(string condition);
    }
}
