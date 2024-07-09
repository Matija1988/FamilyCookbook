using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DapperDBContext _context;
        public MemberRepository(DapperDBContext context)
        {
            _context = context;
        }

        public Task<RepositoryResponse<Member>> CreateAsync(Member entity)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<List<Member>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Member>>();


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
                    "LEFT JOIN Recipe e on d.RecipeId = e.Id;";

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

                    if(picture != null)
                    {
                        existingEntity.Picture = picture;
                    }
                    if(recipe != null)
                    {
                        existingEntity.Recipes.Add(recipe);
                    }
                    return existingEntity;
                },
                splitOn: "RoleId, PictureId, RecipeId");

                response.Success = true;
                response.Items = entities.ToList();

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

        public Task<RepositoryResponse<Member>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Member>> UpdateAsync(int id, Member entity)
        {
            throw new NotImplementedException();
        }
    }
}
