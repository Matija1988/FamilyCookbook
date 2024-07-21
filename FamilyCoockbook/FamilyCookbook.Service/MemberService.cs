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
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<Member>> CreateAsync(Member entity)
        {
            entity.UniqueId = Guid.NewGuid();
            entity.IsActive = true;
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;


            var response = await _repository.CreateAsync(entity);

            return response;
        }

        public async Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            var response = await _repository.DeleteAsync(id);
            
            return response;
        }

        public async Task<RepositoryResponse<List<Member>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Member>> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<Member>> UpdateAsync(int id, Member entity)
        {
            entity.DateUpdated = DateTime.Now;

            var response = await _repository.UpdateAsync(id, entity);

            return response;

        }

        public async Task<RepositoryResponse<Member>> PermaDeleteAsync(int id)
        {
            var response = await _repository.PermaDeleteAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId)
        {
            var response = await _repository.GetByGuidAsync(uniqueId);  

            return response;
        }

        public async Task<RepositoryResponse<List<Member>>> PaginateAsync(Paging paging)
        {
            var response = await _repository.PaginateAsync(paging);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;
        }
    }
}
