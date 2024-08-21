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
    public sealed class CategoryService : IService<Category>
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
            
        }

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
            return await _repository.SoftDeleteAsync(id);   
        }
    }
}
