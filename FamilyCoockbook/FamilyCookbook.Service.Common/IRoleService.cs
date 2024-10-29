namespace FamilyCookbook.Service.Common
{
    public interface IRoleService
    {
        Task<RepositoryResponse<List<Role>>> GetAllAsync();

        Task<RepositoryResponse<Role>> GetByIdAsync(int id);
    }
}
