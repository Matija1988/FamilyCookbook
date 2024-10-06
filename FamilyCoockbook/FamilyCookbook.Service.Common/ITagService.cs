using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface ITagService
    {
        Task<RepositoryResponse<List<Tag>>> GetAllAsync();

        Task<CreateResponse> CreateAsync(List<Tag> entities);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<CreateResponse> ConnectRecipeAndTag(RecipeTagArray dto);

        Task<RepositoryResponse<ImmutableList<Tag>>> PaginateAsync(Paging paging, string text);
    }
}
