using FamilyCookbook.Common;
using FamilyCookbook.Model;

namespace FamilyCookbook.Respository.Common
{
    public interface IRepository<T> where T : class
    {
        Task<RepositoryResponse<List<T>>> GetAllAsync();

        Task<RepositoryResponse<T>> GetByIdAsync(int id);

     

        Task<MessageResponse> UpdateAsync(int id, T entity);


        Task<MessageResponse> DeleteAsync(int id);

        Task<RepositoryResponse<T>> SoftDeleteAsync(int id);
    }
}
