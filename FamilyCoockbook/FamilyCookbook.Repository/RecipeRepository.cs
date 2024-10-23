using Azure;
using Azure.Core;
using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public sealed partial class RecipeRepository : AbstractRepository<Recipe, RecipeFilter>, IRecipeRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public RecipeRepository
            (DapperDBContext context,
            IErrorMessages errorMessages,
            ISuccessResponses successResponses) : base(context, errorMessages, successResponses)
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
                StringBuilder query = new("DELETE FROM MemberRecipe where MemberId = @memberId AND RecipeId = @recipeId");

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), new { MemberId = memberId, RecipeId = recipeId });

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityDeleted(" member from recipe");

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("MemberRecipe");
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
                StringBuilder query = new("INSERT INTO MemberRecipe (MemberId, RecipeId)");
                query.Append(" VALUES(@MemberId, @RecipeId)");

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), entity);

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("MemberRecipe");
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

                StringBuilder query = new($"UPDATE Recipe SET PictureId = {pictureId} WHERE Id = {recipeId}");

                var updatedEntity = await connection.ExecuteScalarAsync(query.ToString());

                response.Success = true;
                response.Message = _successResponses.EntityUpdated();
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe");
                return response;

            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }



        #region GET METHODS

        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new("SELECT a.Id, a.Title, a.Subtitle, a.Text, a.CategoryId, ");
            query.Append("c.Id, c.FirstName, c.LastName, c.Bio, d.Id, d.Name, e.*, g.* ");
            query.Append("FROM Recipe a JOIN MemberRecipe b on a.Id = b.RecipeId ");
            query.Append("JOIN Member c on b.MemberId = c.Id LEFT JOIN Category d on d.Id = a.CategoryId ");
            query.Append("JOIN Picture e on e.Id = a.PictureId JOIN RecipeTags f on f.RecipeId = a.Id ");
            query.Append("LEFT JOIN Tag g on g.Id = f.TagId WHERE a.IsActive = 1 ");

            return query.Append("ORDER BY a.DateCreated;");
        }

        protected override async Task<List<Recipe>> BuildQueryCommand(string query, IDbConnection connection)
        {
            var entityDictionary = new Dictionary<int, Recipe>();

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

                    if (member != null)
                    {
                        existingEntity.Members.Add(member);
                    }

                    if (category != null)
                    {
                        existingEntity.Category = category;
                    }
                    if (picture != null)
                    {
                        existingEntity.Picture = picture;
                    }

                    return existingEntity;
                },
                splitOn: "Id");

            return entities.ToList();

        }

        protected override StringBuilder BuildQueryReadSingle(int id)
        {
            StringBuilder query = new("SELECT a.Id, a.Title, a.Subtitle, a.Text, a.CategoryId, ");
            query.Append("c.Id, c.FirstName, c.LastName, c.Bio, ");
            query.Append("d.Id, d.Name, d.Description, e.*, g.* ");
            query.Append("FROM Recipe a JOIN MemberRecipe b on a.Id = b.RecipeId ");
            query.Append(" JOIN Member c on b.MemberId = c.Id ");
            query.Append(" LEFT JOIN Category d on d.Id = a.CategoryId ");
            query.Append("JOIN Picture e on e.Id = a.PictureId ");
            query.Append("LEFT JOIN RecipeTags f on f.RecipeId = a.Id ");
            query.Append("LEFT JOIN Tag g on g.Id = f.TagId ");
            query.Append("WHERE a.Id = @Id ");

            return query;
        }

        protected override async Task<Recipe> BuildQueryCommand(string query, IDbConnection connection, int id)
        {
            var entityDictionary = new Dictionary<int, Recipe>();

            var entities = await connection.QueryAsync<Recipe, Member, Category, Picture, Tag, Recipe>
                (query,
                (recipe, member, category, picture, tag) =>
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

                    if (picture != null)
                    {
                        existingEntity.Picture = picture;
                    }

                    if (tag != null)
                    {
                        existingEntity.Tags.Add(tag);
                    }

                    return existingEntity;
                },
                new { Id = id },
                splitOn: "Id");

            return entityDictionary.Values.FirstOrDefault();

        }

        public async Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors()
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {
                StringBuilder query = new("Select a.*, b.RecipeId, b.MemberId FROM Recipe a ");
                query.Append("LEFT JOIN MemberRecipe b on a.Id = b.RecipeId ");
                query.Append("LEFT JOIN Member c on b.MemberId = c.Id ");
                query.Append("WHERE b.MemberId IS NULL");

                using var connection = _context.CreateConnection();

                IEnumerable<Recipe> recipes = (await connection.QueryAsync<Recipe>(query.ToString())).ToList();

                response.Success = true;
                response.Items = recipes.ToList();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        protected override async Task<List<Recipe>> BuildPaginationCommand(StringBuilder query, GridReader multipleQuery)
        {

            var entityDictionary = new Dictionary<int, Recipe>();

            IEnumerable<Recipe> entities = multipleQuery.Read<Recipe, Member, Category, Picture, Recipe>
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

                    if (picture != null)
                    {
                        existingEntity.Picture = picture;
                    }

                    return existingEntity;
                },
                splitOn: "Id");


            return entities.ToList();
        }

        public async Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging, RecipeFilter? filter)
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {
                var incrrasedPageSize = paging.PageSize * 3;
                StringBuilder query = PaginateQueryBuilder(paging, filter, "Recipe", "Id", "Id");

                var entityDictionary = new Dictionary<int, Recipe>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query.ToString(), new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = incrrasedPageSize,
                });

                IEnumerable<Recipe> entities = multipleQuery.Read<Recipe, Member, Category, Picture, Recipe>
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

                        if (picture != null)
                        {
                            existingEntity.Picture = picture;
                        }

                        return existingEntity;
                    },
                    splitOn: "Id");

                response.Success = true;
                response.Items = entityDictionary.Values.Take(paging.PageSize).ToList();
                response.TotalCount = await multipleQuery.ReadSingleAsync<int>();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        protected override StringBuilder PaginateQueryBuilder
            (Paging paging, RecipeFilter? filter, string tableName, string keyColumn, string keyPropery)
        {
            StringBuilder sb = new StringBuilder();

            StringBuilder countQuery = new StringBuilder(" SELECT COUNT(DISTINCT a.Id) FROM Recipe a ");
            countQuery.Append(" JOIN MemberRecipe b ON a.Id = b.RecipeId ");
            countQuery.Append(" JOIN Member c on b.MemberId = c.Id ");
            countQuery.Append(" LEFT JOIN Category d ON d.Id = a.CategoryId ");
            countQuery.Append(" JOIN Picture e on e.Id = a.PictureId ");
            countQuery.Append($" WHERE a.IsActive = {filter.SearchByActivityStatus} ");

            sb.Append("SELECT a.Id, a.Title, a.Subtitle, a.Text,a.CategoryId, c.Id, ");
            sb.Append("c.FirstName, c.LastName, c.Bio, d.Id, d.Name, d.Description, ");
            sb.Append("e.* FROM Recipe a JOIN MemberRecipe b on a.Id = b.RecipeId ");
            sb.Append(" JOIN Member c on b.MemberId = c.Id ");
            sb.Append("LEFT JOIN Category d on d.Id = a.CategoryId  JOIN Picture e on e.Id = a.PictureId ");
            sb.Append("WHERE a.IsActive = 1 ");

            if (!string.IsNullOrWhiteSpace(filter.SearchByTitle))
            {
                sb.Append(@$"AND a.Title LIKE '%{filter.SearchByTitle}%' ");
                countQuery.Append(@$" AND a.Title LIKE '%{filter.SearchByTitle}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchBySubtitle))
            {
                sb.Append(@$"AND a.Subtitle LIKE '%{filter.SearchBySubtitle}%' ");
                countQuery.Append(@$" AND a.Subtitle LIKE '%{filter.SearchBySubtitle}%' ");
            }


            if (filter.SearchByCategory > 0)
            {
                sb.Append(@$"AND a.CategoryId = {filter.SearchByCategory} ");
                countQuery.Append(@$" AND a.CategoryId = {filter.SearchByCategory} ");
            }

            if (!filter.SearchByActivityStatus.Equals(null))
            {
                sb.Append(@$"AND a.IsActive = {filter.SearchByActivityStatus} ");
                countQuery.Append(@$" AND a.IsActive = {filter.SearchByActivityStatus} ");
            }

            if (filter.SearchByDateCreated.HasValue)
            {
                sb.Append(@$"AND a.DateCreated = {filter.SearchByDateCreated} ");
                countQuery.Append(@$" AND a.DateCreate = {filter.SearchByDateCreated} ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByAuthorName))
            {
                sb.Append(@$" AND c.FirstName LIKE '%{filter.SearchByAuthorName}%' ");
                countQuery.Append(@$" AND c.FirstName LIKE '%{filter.SearchByAuthorName}%' ");

            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByAuthorSurname))
            {
                sb.Append(@$"AND c.LastName LIKE '%{filter.SearchByAuthorSurname}%'");
                countQuery.Append(@$" AND c.LastName LIKE '%{filter.SearchByAuthorSurname}% '");
            }


            sb.Append("ORDER BY a.DateCreated DESC ");
            sb.Append("OFFSET @Offset ROWS ");
            sb.Append("FETCH NEXT @PageSize ROWS ONLY;");

            sb.Append(countQuery);

            return sb;

        }
        #endregion


    }
}
