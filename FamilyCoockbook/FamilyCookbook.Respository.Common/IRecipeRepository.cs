using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;

using System.Collections.Immutable;

namespace FamilyCookbook.Repository.Common
{
    public interface IRecipeRepository : IRepository<Recipe>
    {

        Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId);  

        Task<RepositoryResponse<Recipe>> AddMemberToRecipeAsync(MemberRecipe entity);

        Task<MessageResponse> CreateAsyncTransaction(RecipeCreateDTO entity);


        Task<MessageResponse> UpdateAsync(int id, RecipeCreateDTO entity);


        Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors();

        Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging, RecipeFilter filter);

        Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId);

    }
}
