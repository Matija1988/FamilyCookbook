using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface ITagRepository
    {
        Task<RepositoryResponse<List<Tag>>> GetAllAsync();

        Task<CreateResponse> CreateAsync(Tag entity);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<CreateResponse> ConnectRecipeAndTag(RecipeTag dto);

        Task<RepositoryResponse<ImmutableList<Tag>>> PaginateAsync(Paging paging, string text);

        Task<CreateResponse> DeleteAsync(int id);
    }
}
