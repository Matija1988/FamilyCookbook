using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public RecipeRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        public async Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = new RepositoryResponse<Recipe>();

            int rowsAffected = 0;

            try
            {
                string query = "DELETE FROM MemberRecipe where MemberId = @memberId AND RecipeId = @recipeId" ;

                using var connection =_context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, new {MemberId = memberId, RecipeId = recipeId});

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityDeleted(" member from recipe").ToString();

                return response;
            }
            catch (Exception ex) 
            { 
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("MemberRecipe").ToString() + ex.Message;
                return response;
            }
            finally
            {
               _context.CreateConnection().Close();   
            }

        }

        public async Task<RepositoryResponse<Recipe>> AddMemberToRecipeAsync(MemberRecipe entity)
        {
            var response = new RepositoryResponse<Recipe>();

            int rowsAffected = 0;

            try
            {
                string query = "INSERT INTO MemberRecipe (MemberId, RecipeId)" +
                    "VALUES(@MemberId, @RecipeId)";

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, entity);

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated().ToString();

                return response;

            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("MemberRecipe").ToString();
                return response;
            } 
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Recipe>> CreateAsync(Recipe entity)
        {
            var response = new RepositoryResponse<Recipe>();

            
            try
            {
                string query = "INSERT INTO Recipe " +
                    "(Title, " +
                    "Subtitle, " +
                    "Text, " +
                    "IsActive, " +
                    "DateCreated, " +
                    "DateUpdated, " +
                    "CategoryId)" +
                    "OUTPUT INSERTED.Id, INSERTED.Title, INSERTED.Subtitle, INSERTED.Text," +
                    "INSERTED.IsActive, INSERTED.DateCreated, INSERTED.DateUpdated, INSERTED.CategoryId  " +
                    "VALUES (@Title, " +
                    "@Subtitle, " +
                    "@Text, " +
                    "@IsActive, " +
                    "@DateCreated, " +
                    "@DateUpdated," +
                    "@CategoryId); ";

                using var connection = _context.CreateConnection();

                var id = await connection.ExecuteScalarAsync<int>(query, entity);
                entity.Id = id;

                response.Success = true;
                response.Message = _successResponses.EntityCreated().ToString();
                response.Items = entity;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorCreatingEntity(" Recipe ").ToString();
                return response;
            }
            finally 
            { 
             _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Recipe>();

            int rowsAffected = 0;

            try
            {
                string query = "UPDATE Recipe " +
                    "SET IsActive = 0 " +
                    "Where Id = @Id";

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, new { Id = id });

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityUpdated().ToString();

                return response; 
            }
            catch (Exception ex) 
            { 
                response.Success= false;
                response.Message = _errorMessages.NotFound(id).ToString() + ex.Message;

                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        #region GET METHODS
        public async Task<RepositoryResponse<List<Recipe>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {
                string query = "SELECT " +
                    "a.Id, " +
                    "a.Title, " +
                    "a.Subtitle, " +
                    "a.Text," +
                    "a.CategoryId, " +
                    "c.Id, " +
                    "c.FirstName, " +
                    "c.LastName, " +
                    "d.Id, " +
                    "d.Name " +
                    "FROM Recipe a " +
                    "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "JOIN Member c on b.MemberId = c.Id " +
                    "LEFT JOIN Category d on d.Id = a.CategoryId " +
                    "WHERE a.IsActive = 1 " +
                    "order by a.Title;";

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection =  _context.CreateConnection();

                var entities = await connection.QueryAsync<Recipe, Member, Category, Recipe>
                    (query,
                    (recipe, member, category) =>
                    {
                        if (!entityDictionary.TryGetValue(recipe.Id, out var existingEntity))
                        {
                            existingEntity = recipe;
                            existingEntity.Members = new List<Member>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }
                      
                        if(member != null)
                        {
                            existingEntity.Members.Add(member);
                        }

                        if (category != null)
                        {
                            existingEntity.Category = category;
                        }

                        return existingEntity;
                    },
                    splitOn: "Id");

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe").ToString() + ex.Message;
                return response;
            }
            finally 
            { 
                _context.CreateConnection().Close();    
            }

        }

        public  async Task<RepositoryResponse<Recipe>> GetByIdAsync(int id)
        {
            var response = new RepositoryResponse<Recipe>();

            try
            {
                string query = "SELECT " +
                    "a.Id, " +
                    "a.Title, " +
                    "a.Subtitle, " +
                    "a.Text, " +
                    "a.CategoryId, " +
                    "c.Id, " +
                    "c.FirstName, " +
                    "c.LastName, " +
                    "d.Id, " +
                    "d.Name " +
                    "FROM Recipe a " +
                    "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "JOIN Member c on b.MemberId = c.Id " +
                    "LEFT JOIN Category d on d.Id = a.CategoryId " +
                    "WHERE a.Id = @Id " +
                    "order by a.Title;";

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Recipe, Member, Category, Recipe>
                    (query,
                    (recipe, member, category) =>
                    {
                        if (!entityDictionary.TryGetValue(recipe.Id, out var existingEntity))
                        {
                            existingEntity = recipe;
                            existingEntity.Members = new List<Member>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }

                        if (member != null)
                        {
                            existingEntity.Members.Add(member);
                        }

                        if (category != null)
                        {
                            existingEntity.Category = category;
                        }

                        return existingEntity;
                    },
                    new {Id = id },
                    splitOn: "Id");

                var result = entityDictionary.Values.FirstOrDefault();

                if(result is null)
                {
                    response.Success = false;
                    response.Message = _errorMessages.NotFound(id).ToString();

                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public Task<RepositoryResponse<List<Recipe>>> GetNotActiveAsync()
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {

            }
            catch (Exception ex) 
            { 
            
            }
            finally 
            { 
                _context.CreateConnection().Close(); 
            }

            throw new NotImplementedException();
        }

       
        public async Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging, RecipeFilter filter)
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {
                
                string query = QueryBuilder(paging, filter);

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query, new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize,
                });

                var entities =  multipleQuery.Read<Recipe, Member, Category, Recipe>
                    ((recipe, member, category) =>
                    {
                        if (!entityDictionary.TryGetValue(recipe.Id, out var existingEntity))
                        {
                            existingEntity = recipe;
                            existingEntity.Members = new List<Member>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }

                        if (member != null)
                        {
                            existingEntity.Members.Add(member);
                        }

                        if (category != null)
                        {
                            existingEntity.Category = category;
                        }

                        return existingEntity;
                    },
                    splitOn: "Id");

                 

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();
                response.TotalCount = response.Items.Count;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        private string QueryBuilder(Paging paging, RecipeFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT " +
                "a.Id, " +
                "a.Title, " +
                "a.Subtitle, " +
                "a.Text," +
                "a.CategoryId, " +
                "c.Id, " +
                "c.FirstName, " +
                "c.LastName, " +
                "d.Id, " +
                "d.Name " +
                "FROM Recipe a " +
                "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                "JOIN Member c on b.MemberId = c.Id " +
                "LEFT JOIN Category d on d.Id = a.CategoryId " +
                "WHERE a.IsActive = 1 ");

            if (!string.IsNullOrWhiteSpace(filter.SearchByTitle)) 
            {
                sb.Append($"AND a.Title LIKE '%{filter.SearchByTitle}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchBySubtitle)) 
            {
                sb.Append($"AND a.Subtitle LIKE '%{filter.SearchBySubtitle}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByAuthorName)) 
            {
                sb.Append($"AND c.FirstName LIKE '%{filter.SearchByAuthorName}%' ");
                
            }

            if(!string.IsNullOrWhiteSpace(filter.SearchByAuthorSurname))
            {
                sb.Append($"AND c.LastName LIKE '%{filter.SearchByAuthorSurname}%'");
            }

            if(filter.SearchByCategory > 0)
            {
                sb.Append($"AND a.CategoryId = {filter.SearchByCategory} ");
            }

            if(!filter.SearchByActivityStatus.Equals(null))
            {
                sb.Append($"AND a.IsActive = {filter.SearchByActivityStatus} ");
            }

            if(filter.SearchByDateCreated.HasValue)
            {
                sb.Append($"AND a.DateCreated = ${filter.SearchByDateCreated} ");
            }

            sb.Append("ORDER BY a.Title ");
            sb.Append($"OFFSET @Offset ROWS ");
            sb.Append($"FETCH NEXT @PageSize ROWS ONLY;");
           
            sb.Append($" SELECT COUNT(*) FROM Recipe WHERE IsActive = {filter.SearchByActivityStatus};");

            return sb.ToString();

        }
        #endregion



        public Task<RepositoryResponse<Recipe>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            var response = new RepositoryResponse<Recipe>();

            int rowAffected = 0;

            try
            {
                var query = "UPDATE Recipe " +
                    "SET Title = @Title, " +
                    "Subtitle = @Subtitle, " +
                    "Text = @Text, " +
                    "DateUpdated = @DateUpdated, " +
                    "IsActive = @IsActive, " +
                    "CategoryId = @CategoryId " +
                    "WHERE Id = @Id";

                using var connection = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new {
                    entity.Title,
                    entity.Subtitle,
                    entity.Text,
                    entity.DateUpdated,
                    entity.IsActive,
                    entity.CategoryId,
                    Id = id
                });

                response.Success = rowAffected > 0;
                response.Message = _successResponses.EntityUpdated().ToString();

                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe").ToString() + ex.Message;
                return response;
            } 
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        
    }
}
