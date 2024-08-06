using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IService<T> where T : class
    {
        Task<RepositoryResponse<List<T>>> GetAllAsync();

        Task<RepositoryResponse<T>> GetByIdAsync(int id);

        Task<RepositoryResponse<T>> CreateAsync(T entity);

        Task<RepositoryResponse<T>> UpdateAsync(int id, T entity);

        Task<RepositoryResponse<T>> DeleteAsync(int id);

        Task<RepositoryResponse<T>> SoftDeleteAsync(int id);    
    }
}
