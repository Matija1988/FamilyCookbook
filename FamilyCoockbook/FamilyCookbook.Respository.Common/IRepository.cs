﻿using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using System.Runtime.InteropServices;

namespace FamilyCookbook.Respository.Common
{
    public interface IRepository<T, Filter> where T : class
    {
        Task<RepositoryResponse<List<T>>> GetAllAsync();

        Task<RepositoryResponse<T>> GetByIdAsync(int id);

        Task<MessageResponse> UpdateAsync(int id, T entity);

        Task<MessageResponse> DeleteAsync(int id);

        Task<RepositoryResponse<T>> SoftDeleteAsync(int id);


        Task<RepositoryResponse<Lazy<List<T>>>> PaginateAsync(Paging paging, Filter filter);
    }
}
