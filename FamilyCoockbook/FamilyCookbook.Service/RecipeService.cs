using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<Recipe>> AddMemberToRecipe(MemberRecipe entity)
        {
            var response = await _repository.AddMemberToRecipeAsync(entity);

            return response;
        }


        public async Task<RepositoryResponse<Recipe>> CreateAsync(Recipe entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;

            var response = await _repository.CreateAsync(entity);

            return response;
        }

        public async Task<RepositoryResponse<Recipe>> DeleteAsync(int id)
        {
            var response = await _repository.DeleteAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<List<Recipe>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Recipe>> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging)
        {
          var response = await _repository.PaginateAsync(paging);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;
        }

        public Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = _repository.RemoveMemberFromRecipeAsync(memberId, recipeId);

            return response;
        }

        public Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id)
        {
            return _repository.SoftDeleteAsync(id);
        }

        public async Task<RepositoryResponse<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            entity.DateUpdated = DateTime.Now;

            var response = await _repository.UpdateAsync(id, entity);

            return response;
        }
    }
}
