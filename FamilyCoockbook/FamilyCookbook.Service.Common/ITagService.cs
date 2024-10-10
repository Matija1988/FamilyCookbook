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
    public interface ITagService : IService<Tag>
    {

        Task<MessageResponse> CreateAsync(List<Tag> entities);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<MessageResponse> ConnectRecipeAndTag(RecipeTagArray dto);

        Task<RepositoryResponse<List<Tag>>> PaginateAsync(Paging paging, string text);

        Task<MessageResponse> UpdateAsync(int id, Tag tag);

    }
}
