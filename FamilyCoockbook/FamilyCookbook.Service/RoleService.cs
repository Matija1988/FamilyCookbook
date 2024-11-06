namespace FamilyCookbook.Service
{
    public sealed class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<Lazy<List<Role>>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RepositoryResponse<Role>> GetByIdAsync(int id)
        {
           return await _repository.GetByIdAsync(id);
        }
    }
}
