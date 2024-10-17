using FamilyCookbook.Common.Filters;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface ICategoryService : IService<Category>
    {
        Task<MessageResponse> CreateAsync(Category entity);

        Task<RepositoryResponse<Lazy<List<Category>>>> PaginateAsync(Paging paging, CategoryFilter filter);
    }
}
