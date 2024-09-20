using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Respository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class CategoryService(ICategoryRepository repository, 
        IRecipeRepository recipeRepository) : ICategoryService
    {
        private readonly ICategoryRepository _repository = repository;
        private readonly IRecipeRepository _recipeRepository = recipeRepository;

        public async Task<RepositoryResponse<Category>> CreateAsync(Category entity)
        {
            entity.IsActive = true;
            return await _repository.CreateAsync(entity);
        }

        public async Task<RepositoryResponse<Category>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<RepositoryResponse<List<Category>>> GetAllAsync()
        {
           return await _repository.GetAllAsync();
        }

        public async Task<RepositoryResponse<Category>> GetByIdAsync(int id)
        {           
            return await _repository.GetByIdAsync(id);
        }

        public async Task<RepositoryResponse<Category>> UpdateAsync(int id, Category entity)
        {
            entity.IsActive = true;
            var response = await _repository.UpdateAsync(id, entity);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<RepositoryResponse<Category>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Category>();

            var recipeResponse = await _recipeRepository.GetAllAsync();

            StringBuilder errorBuilder = new StringBuilder();

            if (recipeResponse == null || recipeResponse.Items.Count == 0)
            {
                response.Success = false;
                response.Message = errorBuilder.Append("Error parsing recipes!");
                return response;
            }

            bool chk = recipeResponse.Items.Any(r => r.CategoryId == id);

            if (chk) 
            {
                response.Success = false;
                response.Message = errorBuilder.Append("The category you are trying to delete has active recipes." +
                    "Deleting the category before deleting the recipes that belong to this category will " +
                    "cause issues in the program. Please delete all active recipes in this category first!");
                return response;
            }

            return response;   
        }
    }
}
