using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IRoleService
    {
        Task<RepositoryResponse<List<Role>>> GetAllAsync();

        Task<RepositoryResponse<Role>> GetByIdAsync(int id);
    }
}
