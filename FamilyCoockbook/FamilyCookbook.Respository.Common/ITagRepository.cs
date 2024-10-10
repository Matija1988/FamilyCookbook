using FamilyCookbook.Common;
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
    public interface ITagRepository : IRepository<Tag>
    {
        
        Task<MessageResponse> CreateAsync(Tag entity);

        Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text);

        Task<MessageResponse> ConnectRecipeAndTag(RecipeTag dto);

        Task<RepositoryResponse<List<Tag>>> PaginateAsync(Paging paging, string text);

        Task<MessageResponse> UpdateAsync(int id, Tag tag);

    }
}
