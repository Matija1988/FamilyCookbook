using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IRecipeService : IService<Recipe, RecipeFilter>
    {
        Task<RepositoryResponse<Recipe>> AddMemberToRecipe(MemberRecipe entity);

        Task<MessageResponse> CreateAsync(RecipeCreateDTO entityDto);

        Task<MessageResponse> UpdateAsync(int id, RecipeCreateDTO dto);

        Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId);

        Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id);

        Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors();

        Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId);

    }
}
