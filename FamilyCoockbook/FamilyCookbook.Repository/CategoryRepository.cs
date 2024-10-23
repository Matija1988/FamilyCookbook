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

        public async Task<RepositoryResponse<Lazy<List<Category>>>> PaginateAsync(Paging paging, CategoryFilter filter)
        {
            var response = new RepositoryResponse<Lazy<List<Category>>>();

            try
            {
                StringBuilder query = QueryBuilder(paging, filter);

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query.ToString(), new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize,
                });

                var entities = await multipleQuery.ReadAsync<Category>();

                response.Success = true;
                response.Items = new Lazy<List<Category>>(() => entities.ToList());
                response.TotalCount = await multipleQuery.ReadSingleAsync<int>();
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Category");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        private StringBuilder QueryBuilder(Paging paging, CategoryFilter filter)
        {
            StringBuilder query = new("SELECT * FROM Category WHERE 1 = 1 ");
            StringBuilder countQuery = new("SELECT COUNT (DISTINCT Id) FROM Category WHERE 1 = 1 ");

            if(!string.IsNullOrEmpty(filter.SearchByName))
            {
                query.Append(@$" AND Name LIKE '%{filter.SearchByName}%' ");
                countQuery.Append(@$" AND Name LIKE '%{filter.SearchByName}%' ");
            }
            if(filter.SearchByActivityStatus != null)
            {
                query.Append(@$" AND IsActive= {filter.SearchByActivityStatus} ");
                countQuery.Append(@$" AND IsActive= {filter.SearchByActivityStatus} ");
            }

            query.Append(" ORDER BY Id DESC ");
            query.Append(" OFFSET @Offset ROWS ");
            query.Append(" FETCH NEXT @PageSize ROWS ONLY;");

            query.Append(countQuery);

            return query;
        }
    }
}
