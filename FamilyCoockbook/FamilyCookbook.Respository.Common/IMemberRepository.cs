namespace FamilyCookbook.Repository.Common
{
    public interface IMemberRepository : IRepository<Member, MemberFilter>
    {
        Task<MessageResponse> CreateAsync(Member entity);
        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);
        Task<RepositoryResponse<Lazy<Member>>> FindByUsernameAsync(string username);    
    }
}
