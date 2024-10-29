using FamilyCookbook.Common;
namespace FamilyCookbook.Service.Common
{
    public interface ITagService : IService<Tag, TagFilter>
    {

        Task<MessageResponse> CreateAsync(List<Tag> entities);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<MessageResponse> ConnectRecipeAndTag(RecipeTagArray dto);


        Task<MessageResponse> UpdateAsync(int id, Tag tag);

    }
}
