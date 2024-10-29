namespace FamilyCookbook.Repository.Common
{
    public interface ICategoryRepository : IRepository<Category, CategoryFilter>
    {
        Task<MessageResponse> CreateAsync(Category entity);
        Task<MessageResponse> UpdateAsync(int id, Category entity);
        

    }
}
