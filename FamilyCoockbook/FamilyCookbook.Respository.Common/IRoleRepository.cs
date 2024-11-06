namespace FamilyCookbook.Repository.Common
{
    public interface IRoleRepository 
    {
        Task<RepositoryResponse<Lazy<List<Role>>>> GetAllAsync();

        Task<RepositoryResponse<Role>> GetByIdAsync(int id);
    }
}
