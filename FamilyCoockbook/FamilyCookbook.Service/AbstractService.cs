using FamilyCookbook.Common;
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
    public abstract class AbstractService<T, Filter> : IService<T, Filter> where T : class
    {
        protected readonly IRepository<T, Filter> _repository;

        protected AbstractService(IRepository<T, Filter> repository)
        {
            _repository = repository;
        }
        public async Task<MessageResponse> DeleteAsync(int id)
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

        public async Task<MessageResponse> UpdateAsync(int id, T entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }

        public Task<RepositoryResponse<Lazy<List<T>>>> PaginateAsync(Paging paging, Filter filter)
        {
            throw new NotImplementedException();
        }
    }
}
