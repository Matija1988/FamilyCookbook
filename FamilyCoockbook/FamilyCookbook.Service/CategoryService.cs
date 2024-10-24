﻿using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
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
    public sealed class CategoryService : AbstractService<Category, CategoryFilter>, ICategoryService
    {
        private new readonly ICategoryRepository _repository;
        private readonly IRecipeRepository _recipeRepository;

        public CategoryService(ICategoryRepository repository,
            IRecipeRepository recipeRepository) : base(repository)
        {
            _repository = repository;
            _recipeRepository = recipeRepository;
        }

        public async Task<MessageResponse> CreateAsync(Category entity)
        {
            entity.IsActive = true;
            return await _repository.CreateAsync(entity);
        }

        public async Task<MessageResponse> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<MessageResponse> UpdateAsync(int id, Category entity)
        {
            entity.IsActive = true;
            var response = await _repository.UpdateAsync(id, entity);

            return response;
        }

        public async Task<RepositoryResponse<Category>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Category>();

            var recipeResponse = await _recipeRepository.GetAllAsync();

            StringBuilder errorBuilder = new StringBuilder();

            if (recipeResponse.Success == false)
            {
                response.Success = false;
                response.Message = recipeResponse.Message;
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

            return await _repository.SoftDeleteAsync(id);
        }

        public async Task<RepositoryResponse<Lazy<List<Category>>>> PaginateAsync(Paging paging, CategoryFilter filter)
        {
            var response = await _repository.PaginateAsync(paging, filter);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;
        }
    }
}
