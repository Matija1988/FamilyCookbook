using FamilyCookbook.Common;
using FamilyCookbook.Model;

namespace FamilyCookbook.Respository.Common
{
    public interface IRepository<T> where T : class
    {
        Task<RepositoryResponse<List<T>>> GetAllAsync();

        Task<RepositoryResponse<T>> GetByIdAsync(int id);

     

        Task<CreateResponse> UpdateAsync(int id, T entity);


        Task<RepositoryResponse<T>> DeleteAsync(int id);

        Task<RepositoryResponse<T>> SoftDeleteAsync(int id);
    }
}
