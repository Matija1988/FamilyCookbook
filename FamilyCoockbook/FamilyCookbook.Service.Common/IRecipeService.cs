using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IRecipeService : IService<Recipe>
    {
        Task<RepositoryResponse<Recipe>> AddMemberToRecipe(MemberRecipe entity);

        Task<MessageResponse> CreateAsync(RecipeCreateDTO entityDto);

        Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId);

        Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging, RecipeFilter filter);

        Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id);

        Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors();

        Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId);

    }
}
