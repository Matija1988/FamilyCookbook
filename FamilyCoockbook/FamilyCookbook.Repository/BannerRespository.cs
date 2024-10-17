using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class BannerRespository : AbstractRepository<Banner>, IBannerRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public BannerRespository(DapperDBContext context, 
            IErrorMessages errorMessages, 
            ISuccessResponses successResponses) : base(context, errorMessages, successResponses)
        {
            _dbContext = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        public async Task<RepositoryResponse<List<Banner>>> PaginateAsync(Paging paging, BannerFilter filter)
        {
            var response = new RepositoryResponse<List<Banner>>();

            try
            {
                StringBuilder query = QueryBuilder(paging, filter);

                using var connection = _dbContext.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query.ToString(), new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize,
                });

                IEnumerable<Banner> entities = await multipleQuery.ReadAsync<Banner>();

                response.Success = true;
                response.Items = entities.ToList();
                response.TotalCount = await multipleQuery.ReadSingleAsync<int>();
                return response;

            } 
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Banner", ex);
                return response;
            }
            finally
            {
                _dbContext.CreateConnection().Close();
            }

        }

        private StringBuilder QueryBuilder(Paging paging, BannerFilter filter)
        {
            StringBuilder query = new StringBuilder("SELECT * FROM Banner a WHERE 1 = 1 ");

            StringBuilder countQuery = new StringBuilder("SELECT COUNT(DISTINCT a.Id) FROM Banner a " +
                "WHERE 1 = 1 ");

            if(!string.IsNullOrWhiteSpace(filter.SearchByName))
            {
                query.Append(@$"AND Name LIKE '%{filter.SearchByName}%' ");
                countQuery.Append($"AND Name LIKE '%{filter.SearchByName}%' ");
            }

            if(!string.IsNullOrEmpty(filter.SearchByDestination))
            {
                query.Append(@$"AND Destination LIKE '%{filter.SearchByDestination}%' ");
                countQuery.Append(@$"AND Destination LIKE '%{filter.SearchByDestination}%' ");
            }

            if (!filter.SearchByActivityStatus.Equals(null))
            {
                query.Append(@$"AND IsActive = {filter.SearchByActivityStatus} ");
                countQuery.Append(@$"AND IsActive = {filter.SearchByActivityStatus} ");
            }

            query.Append("ORDER BY DateCreated DESC ");
            query.Append("OFFSET @Offset ROWS ");
            query.Append("FETCH NEXT @PageSize ROWS ONLY; ");

            query.Append(countQuery);

            return query;
        }
    }
}
