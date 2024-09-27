using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public abstract class AbstractService<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        protected AbstractService(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<RepositoryResponse<T>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<RepositoryResponse<List<T>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RepositoryResponse<T>> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<RepositoryResponse<T>> SoftDeleteAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }

        public async Task<RepositoryResponse<T>> UpdateAsync(int id, T entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }


    }
}
