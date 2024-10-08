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
    public class SearchRepository : ISearchRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public SearchRepository(DapperDBContext dbContext, 
            IErrorMessages errorMessages, 
            ISuccessResponses successResponses)
        {
            _dbContext = dbContext;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }
        public async Task<RepositoryResponse<ImmutableList<Recipe>>> GetAllBySearchText(string searchText)
        {
            var response = new RepositoryResponse<ImmutableList<Recipe>>();

            try
            {
                StringBuilder query = new("SELECT a.Id, a.Title, a.Subtitle, a.Text, " +
                    "c.Id, c.FirstName, c.LastName, " +
                    "d.Id, d.Name, " +
                    "e.* " +
                    "FROM Recipe a " +
                    "JOIN MemberRecipe b on a.Id = b.RecipeId " +
                    "JOIN Member c on c.Id = b.MemberId " +
                    "LEFT JOIN Category d on d.Id = a.CategoryId " +
                    "JOIN Picture e on e.Id = a.PictureId " +
                    "JOIN RecipeTags f on f.RecipeId = a.Id " +
                    "JOIN Tag g on g.Id = f.TagId " +
                    $"WHERE g.Text LIKE '%{searchText}%' " +
                    "AND a.IsActive = 1 " +
                    "ORDER BY a.DateCreated DESC");

                using var connection = _dbContext.CreateConnection();

                var entityDictionary = new Dictionary<int, Recipe>();

                var entities = await connection.QueryAsync<Recipe, Member, Category, Picture, Recipe>(
                    query.ToString(), (recipe, member, category, picture) =>
                    {
                        if (!entityDictionary.TryGetValue(recipe.Id, out var existingEntity))
                        {
                            existingEntity = recipe;
                            existingEntity.Members = new List<Member>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }

                        if (member != null) { existingEntity.Members.Add(member); }

                        if (category != null) { existingEntity.Category = category; }

                        if (picture != null) { existingEntity.Picture = picture; }

                        return existingEntity;
                    }, splitOn: "Id");

                response.Success = true;
                response.Items = entityDictionary.Values.ToImmutableList();
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Recipe", ex);
                return response;
            }
            finally 
            {
                _dbContext.CreateConnection().Close();
            }
            



            throw new NotImplementedException();
        }
    }
}
