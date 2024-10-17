using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
