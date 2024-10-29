namespace FamilyCookbook.Service.Common
{
    public interface ICategoryService : IService<Category, CategoryFilter>
    {
        Task<MessageResponse> CreateAsync(Category entity);

    }
}
