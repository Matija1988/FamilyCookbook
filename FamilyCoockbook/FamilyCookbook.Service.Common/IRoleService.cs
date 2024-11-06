namespace FamilyCookbook.Service.Common
{
    public interface IRoleService
    {
        Task<RepositoryResponse<Lazy<List<Role>>>> GetAllAsync();

        Task<RepositoryResponse<Role>> GetByIdAsync(int id);
    }
}
