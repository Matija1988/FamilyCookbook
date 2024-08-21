using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Respository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class MemberService : IMemberService
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

        public async Task<RepositoryResponse<Member>> SoftDeleteAsync(int id)
        {
            var response = await _repository.SoftDeleteAsync(id);
            
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

        public async Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            var response = await _repository.DeleteAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId)
        {
            var response = await _repository.GetByGuidAsync(uniqueId);  

            return response;
        }

        public async Task<RepositoryResponse<List<Member>>> PaginateAsync(Paging paging, MemberFilter filter)
        {
            var response = await _repository.PaginateAsync(paging, filter);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);
           
            return response;
        }

        public async Task<RepositoryResponse<List<Member>>> SearchMemberByCondition(string condition)
        {
            var response = await _repository.GetAllAsync();

            var members = response.Items.Where(member => (member.FirstName.ToLower().Contains(condition.ToLower()) ||
                member.LastName.ToLower().Contains(condition.ToLower()))).ToList();

            response.Items = members;

            if(response.Items.Count < 1)
            {
                response.Success = false;
                response.Message = "No member with condition " + condition + " found";
                return response;
            }

            return response;
        }
    }
}
