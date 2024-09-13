using Dapper;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public sealed partial class RecipeRepository
    {
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

                    if (entity.MemberIds == null)
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
