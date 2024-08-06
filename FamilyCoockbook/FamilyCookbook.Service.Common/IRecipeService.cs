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

        Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging);

        Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id);
    }
}
