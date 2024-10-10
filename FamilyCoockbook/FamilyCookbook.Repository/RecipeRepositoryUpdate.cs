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
        public async Task<MessageResponse> UpdateAsync(int id, RecipeCreateDTO entity)
        {
            var response = new MessageResponse();

            int rowAffected = 0;

            using var connection = _context.CreateConnection();

            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var updatePictureQuery = UpdatePictureQuery(entity).ToString();


                    var pictureParameters = new
                    {
                        Name = entity.Picture.Name,
                        Location = entity.Picture.Location,
                        IsActive = entity.Picture.IsActive
                    };

                    await connection.ExecuteAsync(updatePictureQuery, pictureParameters, transaction);

                    var updateRecipeQuery = UpdateRecipeQuery(entity, id).ToString();

                    int? pictureId = entity.Picture?.Id;

                    if (entity.Picture.Id == null || entity.Picture.Id == 0)
                    {
                        pictureId = await connection.QuerySingleAsync<int>(InsertPictureQuery(entity).ToString(),
                            pictureParameters, transaction);

                    }

                    var recipeParameters = new
                    {
                        Title = entity.Title,
                        Subtitle = entity.Subtitle,
                        Text = entity.Text,
                        IsActive = entity.IsActive,
                        CategoryId = entity.CategoryId,
                        PictureId = pictureId ?? entity.Picture.Id,
                        DateUpdated = entity.DateUpdated,
                        Id = id,
                    };

                    await connection.ExecuteAsync(updateRecipeQuery, recipeParameters, transaction);

                    var deleteMemberRecipeQuery = "DELETE FROM MemberRecipe WHERE RecipeId = @RecipeId";

                    await connection.ExecuteAsync(deleteMemberRecipeQuery, new { RecipeId = id }, transaction);

                    var insertMemberRecipeQuery = InsertMemberRecipeQuery(entity).ToString();

                    if (entity.MemberIds == null)
                    {
                        response.IsSuccess = false;
                        response.Message = _errorMessages.NestedEntityWithIdFound("Recipe", "Member");
                        return response;
                    }

                    foreach (var memberId in entity.MemberIds)
                    {
                        var memberRecipeParameters = new
                        {
                            RecipeId = id,
                            MemberId = memberId
                        };

                            await connection
                            .ExecuteAsync(insertMemberRecipeQuery, memberRecipeParameters, transaction);
                    }

                    if (entity.TagIds != null)
                    {
                        var deleteRecipeTagsQuery = "DELETE FROM RecipeTags WHERE RecipeId = @RecipeId";

                        await connection
                            .ExecuteAsync(deleteRecipeTagsQuery, new { RecipeId = id }, transaction);

                        var insertRecipeTagsQuery = "INSERT INTO RecipeTags(TagId, RecipeId) " +
                            " VALUES (@TagId, @RecipeId);";

                        foreach (var tagId in entity.TagIds)
                        {
                            var recipeTagParameters = new
                            {
                                TagId = tagId,
                                RecipeId = id
                            };

                            await connection
                                .ExecuteAsync(insertRecipeTagsQuery, recipeTagParameters, transaction);
                        }

                    }
                    transaction.Commit();
                    response.IsSuccess = true;
                    response.Message = _successResponses.EntityUpdated();
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                    response.Message = _errorMessages.ErrorAccessingDb("Recipe", ex);
                    return response;
                }
                finally { _context.CreateConnection().Close(); }

            }

        }

        private StringBuilder InsertMemberRecipeQuery(RecipeCreateDTO entity)
        {
            return new StringBuilder("INSERT INTO MemberRecipe(RecipeId, MemberId) " +
                "VALUES (@RecipeId, @MemberId); " +
                "SELECT SCOPE_IDENTITY();");
        }

        private StringBuilder UpdateRecipeQuery(RecipeCreateDTO entity, int id)
        {
            return new StringBuilder("UPDATE Recipe " +
                "SET Title = @Title, Subtitle = @Subtitle, Text = @Text, " +
                " DateUpdated = @DateUpdated, IsActive = @IsActive, " +
                " CategoryId = @CategoryId, PictureId = @PictureId" +
                " WHERE Id = @Id");
        }

        private StringBuilder UpdatePictureQuery(RecipeCreateDTO entity)
        {

            return new StringBuilder("UPDATE Picture SET Name = @Name, " +
                                       " Location = @Location, IsActive = @IsActive " +
                                      $" WHERE Id = {entity.Picture.Id};");

        }

        private StringBuilder InsertPictureQuery(RecipeCreateDTO entity)
        {
            return new StringBuilder("INSERT Picture (Name, Location, IsActive) " +
                "VALUES (@Name, @Location, @IsActive);" +
                "SELECT SCOPE_IDENTITY();");
        }
    }
}
