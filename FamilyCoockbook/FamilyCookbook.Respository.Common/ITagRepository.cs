namespace FamilyCookbook.Repository.Common
{
    public interface ITagRepository : IRepository<Tag, TagFilter>
    {
        
        Task<MessageResponse> CreateAsync(Tag entity);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<MessageResponse> ConnectRecipeAndTag(RecipeTag dto);

        Task<MessageResponse> UpdateAsync(int id, Tag tag);

    }
}
