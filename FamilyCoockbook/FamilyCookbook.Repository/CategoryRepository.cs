using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.AspNetCore.Http.Extensions;
using System.Runtime.CompilerServices;
using System.Text;

namespace FamilyCookbook.Repository
{
    public sealed class CategoryRepository : AbstractRepository<Category, CategoryFilter>, ICategoryRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public CategoryRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses) 
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

    }
}
