using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
