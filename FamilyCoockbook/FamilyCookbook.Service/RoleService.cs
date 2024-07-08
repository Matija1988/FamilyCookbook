using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Respository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<List<Role>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<RepositoryResponse<Role>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
