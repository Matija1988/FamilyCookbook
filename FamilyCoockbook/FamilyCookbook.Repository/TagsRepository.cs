using Azure;
using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public sealed class TagsRepository : ITagRepository
    {
        IErrorMessages _errorMessages;
        ISuccessResponses _successResponses;
        DapperDBContext _context;

        public TagsRepository(IErrorMessages errorMessages, ISuccessResponses success, DapperDBContext context)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = success;
        }

        #region GET METHODS
        public async Task<RepositoryResponse<List<Tag>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Tag>>(); 
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM Tags");

                using var connection = _context.CreateConnection();

                var entity = await connection.QueryAsync<Tag>(query.ToString());

                response.Items = entity.ToList();
                response.Success = true;
                
                return response;

            }
            catch (Exception ex)
            {
                response.Message = _errorMessages.ErrorAccessingDb("Tags", ex);
                response.Success = false;
                return response;
            } finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text)
        {
            var response = new RepositoryResponse<List<Tag>>();

            try
            {
                StringBuilder query = new();
                query.Append($"SELECT * FROM Tags WHERE Text LIKE '%{text}%';");

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

        public async Task<RepositoryResponse<ImmutableList<Tag>>> PaginateAsync(Paging paging, string text)
        {
            var response = new RepositoryResponse<ImmutableList<Tag>>();

            try
            {
                StringBuilder query = PaginationQueryBuilder(paging, text);

                var entityDictionary = new Dictionary<int, Tag>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query.ToString(), new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize
                });

                IEnumerable<Tag> entities = await multipleQuery.ReadAsync<Tag>();

                response.TotalCount = await multipleQuery.ReadSingleAsync<int>();

                response.Success = true;
                response.Items = entities.ToImmutableList();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Tags", ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        private StringBuilder PaginationQueryBuilder(Paging paging, string text)
        {
            StringBuilder query = new("SELECT * FROM Tags WHERE 1 = 1 ");

            StringBuilder countQuery = new($" SELECT COUNT (DISTINCT Id) FROM Tags WHERE Text LIKE '%{text}%';");

            if (!string.IsNullOrWhiteSpace(text))
            {
                query.Append("AND Text = @Text");
            }

            query.Append(" ORDER BY Text ASC ");
            query.Append(" OFFSET @Offset ROWS ");
            query.Append(" FETCH NEXT @PageSize ROWS ONLY;");

            query.Append(countQuery);

            return query;

        }

        #endregion

        public async Task<CreateResponse> CreateAsync(Tag entity)
        {
            var response = new CreateResponse();

            int rowsAffected = 0;

            try
            {
                StringBuilder query = new();
                query.Append("INSERT INTO Tags (Text) VALUES (@Text);");

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), entity);

                response.IsSuccess = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.ErrorCreatingEntity("Tag");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<CreateResponse> ConnectRecipeAndTag(RecipeTag dto)
        {
            var response = new CreateResponse();
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
    }
}
