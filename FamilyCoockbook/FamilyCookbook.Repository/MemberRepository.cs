using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DapperDBContext _context;
        public MemberRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<Member>> CreateAsync(Member entity)
        {
            var response = new RepositoryResponse<Member>();

            int rowsAffeted = 0;

            try
            {
                var query = "INSERT INTO Member " +
                    "(UniqueId, " +
                    "FirstName, " +
                    "LastName, " +
                    "DateOfBirth, " +
                    "Bio, " +
                    "IsActive, " +
                    "DateCreated, " +
                    "DateUpdated, " +
                    "Username, " +
                    "Password, " +
                    "RoleId) " +
                    "VALUES " +
                    "(@UniqueId, " +
                    "@FirstName, " +
                    "@LastName, " +
                    "@DateOfBirth, " +
                    "@Bio, " +
                    "@IsActive, " +
                    "@DateCreated, " +
                    "@DateUpdated," +
                    "@Username," +
                    "@Password," +
                    "@RoleId)";

                using var connection = _context.CreateConnection();

                rowsAffeted = await connection.ExecuteAsync(query, entity);

                response.Success = rowsAffeted > 0;
                response.Message = SuccessResponses.EntityCreated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorCreatingEntity(" Member ").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                var query = "UPDATE Member " +
                    "SET IsActive = 0 " +
                    "WHERE Id = @Id";

                using var connection = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new { Id = id });

                response.Success = rowAffected > 0;
                response.Message = SuccessResponses.EntityUpdated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.NotFound(id).ToString() + ex.Message;

                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<List<Member>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Member>>();

            try
            {

                var query = "SELECT DISTINCT a.*, " +
                    "b.Id AS RoleId, " +
                    "b.*, " +
                    "c.Id AS PictureId, " +
                    "c.*, " +
                    "e.Id AS RecipeId, " +
                    "e.Title " +
                    "FROM Member a " +
                    "LEFT JOIN Role b on a.RoleId = b.Id " +
                    "LEFT JOIN Picture c on a.PictureId = c.Id " +
                    "LEFT JOIN MemberRecipe d on a.Id = d.MemberId " +
                    "LEFT JOIN Recipe e on d.RecipeId = e.Id " +
                    "WHERE a.IsActive = 1;";

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Member, Role, Picture, Recipe, Member>
                    (query,
                    (entity, role, picture, recipe) =>
                {
                    if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                    {
                        existingEntity = entity;
                        existingEntity.Recipes = new List<Recipe>();
                        entityDictionary.Add(existingEntity.Id, existingEntity);
                    }
                    if (role != null)
                    {

                        existingEntity.Role = role;
                    }

                    if (picture != null)
                    {
                        existingEntity.Picture = picture;
                    }
                    if (recipe != null)
                    {
                        existingEntity.Recipes.Add(recipe);
                    }
                    return existingEntity;
                },
                splitOn: "RoleId, PictureId, RecipeId");

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Member>> GetByIdAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            try
            {

                var query = "SELECT a.*, " +
                    "b.Id AS RoleId, " +
                    "b.*, " +
                    "c.Id AS PictureId, " +
                    "c.*, " +
                    "e.Id AS RecipeId, " +
                    "e.Title " +
                    "FROM Member a " +
                    "LEFT JOIN Role b on a.RoleId = b.Id " +
                    "LEFT JOIN Picture c on a.PictureId = c.Id " +
                    "LEFT JOIN MemberRecipe d on a.Id = d.MemberId " +
                    "LEFT JOIN Recipe e on d.RecipeId = e.Id " +
                    "WHERE a.Id = @Id;";

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Member, Role, Picture, Recipe, Member>
                    (query,
                    (entity, role, picture, recipe) =>
                    {
                        if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                        {
                            existingEntity = entity;
                            existingEntity.Recipes = new List<Recipe>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }
                        if (role != null)
                        {

                            existingEntity.Role = role;
                        }

                        if (picture != null)
                        {
                            existingEntity.Picture = picture;
                        }
                        if (recipe != null)
                        {
                            existingEntity.Recipes.Add(recipe);
                        }
                        return existingEntity;
                    },
                    new { Id = id },
                splitOn: "RoleId, PictureId, RecipeId");

                var result = entityDictionary.Values.FirstOrDefault();

                if (result is null)
                {
                    response.Success = false;
                    response.Message = ErrorMessages.NotFound(id).ToString();
                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Member>> PermaDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                string query = "DELETE FROM Member WHERE Id = @Id;";

                using var connection = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new { Id = id });

                response.Success = rowAffected > 0;
                response.Message = SuccessResponses.EntityDeleted("Member").ToString();
                return response;
        
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.NotFound(id).ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Member>> UpdateAsync(int id, Member entity)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                var query = "UPDATE Member " +
                    "SET FirstName = @FirstName, " +
                    "LastName = @LastName, " +
                    "DateOfBirth = @DateOfBirth, " +
                    "Bio = @Bio, " +
                    "DateUpdated = @DateUpdated, " +
                    "Username = @Username, " +
                    "Password = @Password, " +
                    "RoleId = @RoleId " +
                    "WHERE Id = @Id;";

                using var connection  = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new
                {
                    entity.FirstName,
                    entity.LastName,
                    entity.DateOfBirth,
                    entity.Bio,
                    entity.DateUpdated,
                    entity.Username,
                    entity.Password,
                    entity.RoleId,
                    Id = id
                });

                response.Success = rowAffected > 0;
                response.Message = SuccessResponses.EntityUpdated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;    
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }
    }
}
