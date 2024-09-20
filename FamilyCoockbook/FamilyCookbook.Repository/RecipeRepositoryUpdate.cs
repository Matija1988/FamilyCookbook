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
        public async Task<RepositoryResponse<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            var response = new RepositoryResponse<Recipe>();

            int rowAffected = 0;

            using var connection = _context.CreateConnection();

            connection.Open();

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

      
                rowAffected = await connection.ExecuteAsync(query, new
                {
                    entity.Title,
                    entity.Subtitle,
                    entity.Text,
                    entity.DateUpdated,
                    entity.IsActive,
                    entity.CategoryId,
                    Id = id
                });

                response.Success = rowAffected > 0;
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
    }
}
