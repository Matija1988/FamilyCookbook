﻿using Azure;
using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public sealed class TagsRepository : AbstractRepository<Tag, TagFilter>, ITagRepository
    {
        IErrorMessages _errorMessages;
        ISuccessResponses _successResponses;
        DapperDBContext _context;

        public TagsRepository(DapperDBContext context,
            ISuccessResponses success,
            IErrorMessages errorMessages
            ) : base(context,  errorMessages, success)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = success;
        }

        #region GET METHODS

        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new("SELECT * FROM Tag");

            return query;
        }

        public async Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text)
        {
            var response = new RepositoryResponse<List<Tag>>();

            try
            {
                StringBuilder query = new();
                query.Append($"SELECT * FROM Tag WHERE Text LIKE '%{text}%';");

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Tag>(query.ToString());

                response.Success = true;
                response.Items = entities.ToList();

                return response;

            } catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Tag");
                return response;

            }
            finally
            {
                _context.CreateConnection().Close();                
            }
        }

        protected override StringBuilder PaginateQueryBuilder(Paging paging, TagFilter? filter, string tableName, string keyColumn, string keyProperty)
        {
            StringBuilder query = new("SELECT * FROM Tag WHERE 1 = 1 ");

            StringBuilder countQuery = new($" SELECT COUNT (DISTINCT Id) FROM Tag WHERE Text LIKE '%{filter.Text}%';");

            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                query.Append($"AND Text LIKE '%{filter.Text}%'");
            }

            query.Append(" ORDER BY Text ASC ");
            query.Append(" OFFSET @Offset ROWS ");
            query.Append(" FETCH NEXT @PageSize ROWS ONLY;");

            query.Append(countQuery);

            return query;
        }

        #endregion

        protected override StringBuilder BuildCreateQuery(string tableName, string columns, string properties)
        {
            StringBuilder query = new ("INSERT INTO Tag (Text) VALUES (@Text);");

            return query;
        }


        public async Task<MessageResponse> ConnectRecipeAndTag(RecipeTag dto)
        {
            var response = new MessageResponse();
            int rowsAffected = 0;

            try
            {
                StringBuilder query = new();
                query.Append("INSERT INTO RecipeTags(TagId, RecipeId) VALUES (@RecipeId, @TagId);");

                using var connection = _context.CreateConnection();


                rowsAffected = await connection.ExecuteAsync(query.ToString(), dto);

                response.IsSuccess = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.ErrorAccessingDb("Tags", ex);

                return response;
            }
            finally 
            {
                _context.CreateConnection().Close();
            }
        }

        protected override StringBuilder BuildPermaDeleteQuery(string tableName, string keyColumn, int id)
        {
            return new StringBuilder("DELETE FROM Tag WHERE Id = @Id");
        }
        
    }
}
