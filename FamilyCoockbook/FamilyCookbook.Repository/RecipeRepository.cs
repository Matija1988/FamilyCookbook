using Azure.Core;
using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public sealed class RecipeRepository : IRecipeRepository
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

        public async Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId)
        {
            var response = new RepositoryResponse<Recipe>();

            try
            {
                using var connection = _context.CreateConnection();

                var query = $"UPDATE Recipe SET PictureId = {pictureId} WHERE Id = {recipeId}";

                var updatedEntity = await connection.ExecuteScalarAsync(query);

                response.Success = true;
                response.Message = _successResponses.EntityUpdated().ToString();
                return response;

            }
            catch (Exception ex) 
            { 
                response.Success= false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe").ToString() + ex.Message;
                return response;

            } finally
            {
                _context.CreateConnection().Close();
            }
        }

        //public async Task<RepositoryResponse<Recipe>> CreateAsync(Recipe entity)
        //{
        //    var response = new RepositoryResponse<Recipe>();

            
        //        using var connection = _context.CreateConnection();

        //        connection.Open();

        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {

        //                var insertPictureQuery = @"INSERT INTO Picture (Name, Location, IsActive) " +
        //                                          "VALUES (@Name, @Location, @IsActive);" +
        //                                          "SELECT SCOPE_IDENTITY();";

        //                var pictureParamaters = new
        //                {
        //                    Name = entity.Picture.Name,
        //                    Location = entity.Picture.Location,
        //                    IsActive = entity.Picture.IsActive,
        //                };

        //                var pictureId =
        //                    await connection.QuerySingleAsync<int>(insertPictureQuery, pictureParamaters, transaction);

        //                var insertRecipeQuery = @"INSERT INTO Recipe " +
        //                        "(Title, Subtitle, Text, CategoryId, PictureId, IsActive, DateCreated, DateUpdated) " +
        //                        "VALUES" +
        //                        "(@Title, @Subtitle, @Text, @CategoryId, @PictureId, @IsActive, " +
        //                        "@DateCreated, @DateUpdated);" +
        //                        "SELECT SCOPE_IDENTITY();";

        //                var recipeParameters = new
        //                {
        //                    Title = entity.Title,
        //                    Subtitle = entity.Subtitle,
        //                    Text = entity.Text,
        //                    IsActive = entity.IsActive,
        //                    CategoryId = entity.CategoryId,
        //                    PictureId = pictureId,
        //                    DateCreated = entity.DateCreated,
        //                    DateUpdated = entity.DateUpdated,
        //                };

        //                var recipeId =
        //                    await connection.QuerySingleAsync<int>(insertRecipeQuery, recipeParameters, transaction);

        //            var insertMemberRecipeQuery = @"INSERT INTO MemberRecipe(RecipeId, MemberId) " +
        //                                         "VALUES(@RecipeId, @MemberId);" +
        //                                         "SELECT SCOPE_IDENTITY();";

        //            if (entity.Members == null)
        //            {
        //                response.Success = false;
        //                response.Message = "No members with selected ids in database";
        //                return response;
        //            }

        //            foreach (var member in entity.Members)
        //            {
        //                var memberRecipeParametes = new
        //                {

        //                    RecipeId = recipeId,
        //                    MemberId = member.Id
        //                };

        //                await connection
        //                    .ExecuteAsync(insertMemberRecipeQuery, memberRecipeParametes, transaction);
        //            }
        //            transaction.Commit();
        //                response.Success = true;
        //                response.Message = _successResponses.EntityCreated().ToString();
        //                return response;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                response.Success = false;
        //                response.Message = _errorMessages.ErrorCreatingEntity(" Recipe ").ToString() + ex.Message;
        //                return response;
        //            }
        //        }

        //}

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
                    "c.Bio, " +
                    "d.Id, " +
                    "d.Name, " +
                    "e.* " +
                    "FROM Recipe a " +
                    "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "JOIN Member c on b.MemberId = c.Id " +
                    "LEFT JOIN Category d on d.Id = a.CategoryId " +
                    "JOIN Picture e on e.Id = a.PictureId " +
                    "WHERE a.IsActive = 1 " +
                    "order by a.DateCreated;";

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection =  _context.CreateConnection();

                IEnumerable<Recipe> entities = await connection.QueryAsync<Recipe, Member, Category, Picture, Recipe>
                    (query,
                    (recipe, member, category, picture) =>
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
                        if(picture != null)
                        {
                            existingEntity.Picture = picture; 
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
                    "c.Bio, " +
                    "d.Id," +
                    "d.Name, " +
                    "e.* " +
                    "FROM Recipe a " +
                    "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "JOIN Member c on b.MemberId = c.Id " +
                    "LEFT JOIN Category d on d.Id = a.CategoryId " +
                    "JOIN Picture e on e.Id = a.PictureId " +
                    "WHERE a.Id = @Id ";
                    

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Recipe, Member, Category, Picture, Recipe>
                    (query,
                    (recipe, member, category, picture) =>
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

                        if(picture != null)
                        {
                            existingEntity.Picture = picture;
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

       public async Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors()
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {
                string query = "Select a.*, b.RecipeId, b.MemberId " +
                    "FROM Recipe a " +
                    "LEFT JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "LEFT JOIN Member c on b.MemberId = c.Id " +
                    "WHERE b.MemberId IS NULL";

                using var connection = _context.CreateConnection();

                IEnumerable<Recipe> recipes = (await connection.QueryAsync<Recipe>(query)).ToList();

                response.Success = true;
                response.Items = recipes.ToList();

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

                IEnumerable<Recipe> entities =  multipleQuery.Read<Recipe, Member, Category, Picture, Recipe>
                    ((recipe, member, category, picture) =>
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

                        if(picture != null)
                        {
                            existingEntity.Picture = picture;
                        }

                        return existingEntity;
                    },
                    splitOn: "Id");

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();
                response.TotalCount = multipleQuery.ReadSingle<int>();
                
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

            StringBuilder countQuery = new StringBuilder(
                $" SELECT COUNT(*) FROM Recipe a " +
                $" JOIN MemberRecipe b ON a.Id = b.RecipeId " +
                $" JOIN Member c on b.MemberId = c.Id " +
                $" LEFT JOIN Category d ON d.Id = a.CategoryId " +
                $" JOIN Picture e on e.Id = a.PictureId " +
                $" WHERE a.IsActive = {filter.SearchByActivityStatus} ");

            sb.Append("SELECT " +
                "a.Id, " +
                "a.Title, " +
                "a.Subtitle, " +
                "a.Text," +
                "a.CategoryId, " +
                "c.Id, " +
                "c.FirstName, " +
                "c.LastName, " +
                "c.Bio, " +
                "d.Id, " +
                "d.Name," +
                "e.* " +
                "FROM Recipe a " +
                "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                "JOIN Member c on b.MemberId = c.Id " +
                "LEFT JOIN Category d on d.Id = a.CategoryId " +
                "JOIN Picture e on e.Id = a.PictureId " +
                "WHERE a.IsActive = 1 ");

            if (!string.IsNullOrWhiteSpace(filter.SearchByTitle)) 
            {
                sb.Append($"AND a.Title LIKE '%{filter.SearchByTitle}%' ");
                countQuery.Append($" AND a.Title LIKE '%{filter.SearchByTitle}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchBySubtitle)) 
            {
                sb.Append($"AND a.Subtitle LIKE '%{filter.SearchBySubtitle}%' ");
                countQuery.Append($" AND a.Subtitle LIKE '%{filter.SearchBySubtitle}%' ");
            }


            if (filter.SearchByCategory > 0)
            {
                sb.Append($"AND a.CategoryId = {filter.SearchByCategory} ");
                countQuery.Append($" AND a.CategoryId = {filter.SearchByCategory} ");
            }

            if (!filter.SearchByActivityStatus.Equals(null))
            {
                sb.Append($"AND a.IsActive = {filter.SearchByActivityStatus} ");
                countQuery.Append($" AND a.IsActive = {filter.SearchByActivityStatus} ");
            }

            if (filter.SearchByDateCreated.HasValue)
            {
                sb.Append($"AND a.DateCreated = {filter.SearchByDateCreated} ");
                countQuery.Append($" AND a.DateCreate = {filter.SearchByDateCreated} ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByAuthorName)) 
            {
                sb.Append($" AND c.FirstName LIKE '%{filter.SearchByAuthorName}%' ");
                countQuery.Append($" AND c.FirstName LIKE '%{filter.SearchByAuthorName}%' ");
                
            }

            if(!string.IsNullOrWhiteSpace(filter.SearchByAuthorSurname))
            {
                sb.Append($"AND c.LastName LIKE '%{filter.SearchByAuthorSurname}%'");
                countQuery.Append($" AND c.LastName LIKE '%{filter.SearchByAuthorSurname}% '");
            }


            sb.Append("ORDER BY a.DateCreated ");
            sb.Append($"OFFSET @Offset ROWS ");
            sb.Append($"FETCH NEXT @PageSize ROWS ONLY;");
           
            sb.Append(countQuery);

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

        public async Task<RepositoryResponse<Recipe>> CreateAsyncTransaction(RecipeCreateDTO entity)
        {
            var response = new RepositoryResponse<Recipe>();


            using var connection = _context.CreateConnection();

            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    var insertPictureQuery = @"INSERT INTO Picture (Name, Location, IsActive) " +
                                              "VALUES (@Name, @Location, @IsActive);" +
                                              "SELECT SCOPE_IDENTITY();";

                    var pictureParamaters = new
                    {
                        Name = entity.Picture.Name,
                        Location = entity.Picture.Location,
                        IsActive = entity.Picture.IsActive,
                    };

                    var pictureId =
                        await connection.QuerySingleAsync<int>(insertPictureQuery, pictureParamaters, transaction);

                    var insertRecipeQuery = @"INSERT INTO Recipe " +
                            "(Title, Subtitle, Text, CategoryId, PictureId, IsActive, DateCreated, DateUpdated) " +
                            "VALUES" +
                            "(@Title, @Subtitle, @Text, @CategoryId, @PictureId, @IsActive, " +
                            "@DateCreated, @DateUpdated);" +
                            "SELECT SCOPE_IDENTITY();";

                    var recipeParameters = new
                    {
                        Title = entity.Title,
                        Subtitle = entity.Subtitle,
                        Text = entity.Text,
                        IsActive = entity.IsActive,
                        CategoryId = entity.CategoryId,
                        PictureId = pictureId,
                        DateCreated = entity.DateCreated,
                        DateUpdated = entity.DateUpdated,
                    };

                    var recipeId =
                        await connection.QuerySingleAsync<int>(insertRecipeQuery, recipeParameters, transaction);

                    var insertMemberRecipeQuery = @"INSERT INTO MemberRecipe(RecipeId, MemberId) " +
                                                   "VALUES(@RecipeId, @MemberId);" +
                                                   "SELECT SCOPE_IDENTITY();";

                    if(entity.MemberIds == null)
                    {
                        response.Success = false;
                        response.Message = "No members with selected ids in database";
                        return response;
                    }

                    foreach (var memberId in entity.MemberIds)
                    {
                        var memberRecipeParametes = new
                        {
                            
                            RecipeId = recipeId,
                            MemberId = memberId
                        };

                        await connection
                            .ExecuteAsync(insertMemberRecipeQuery, memberRecipeParametes, transaction);
                    }
                    transaction.Commit();
                    response.Success = true;
                    response.Message = _successResponses.EntityCreated().ToString();
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = _errorMessages.ErrorCreatingEntity(" Recipe ").ToString() + ex.Message;
                    return response;
                }
            }

        }
    }
}
