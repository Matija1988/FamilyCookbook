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

        #region Get ALL
        public async Task<RepositoryResponse<List<T>>> GetAllAsync()
        {
            return await ReturnEntities();
        }

        protected virtual async Task<RepositoryResponse<List<T>>> ReturnEntities()
        {
            return await _repository.GetAllAsync();
        }

        #endregion

        #region Get By Id

        public async Task<RepositoryResponse<T>> GetByIdAsync(int id)
        {
            return await ReturnEntity(id);
        }

        protected virtual async Task<RepositoryResponse<T>> ReturnEntity(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        #endregion

        public async Task<RepositoryResponse<T>> SoftDeleteAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }

        public async Task<CreateResponse> UpdateAsync(int id, T entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }


    }
}
