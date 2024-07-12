using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<RepositoryResponse<Recipe>> PermaDeleteAsync(int id);

        Task<RepositoryResponse<List<Recipe>>> GetNotActiveAsync();

        Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthor();

        Task<RepositoryResponse<Recipe>> AddMemberToRecipeAsync(MemberRecipe entity);

    }
}
