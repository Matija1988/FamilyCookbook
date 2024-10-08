using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface ISearchService
    {
        Task<RepositoryResponse<ImmutableList<Recipe>>> GetAllBySearchText(string searchText);
    }
}
