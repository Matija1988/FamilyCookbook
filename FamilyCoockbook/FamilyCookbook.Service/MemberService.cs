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
    public class MemberService : IService<Member>
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public Task<RepositoryResponse<Member>> CreateAsync(Member entity)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<List<Member>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            return response;
        }

        public Task<RepositoryResponse<Member>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Member>> UpdateAsync(int id, Member entity)
        {
            throw new NotImplementedException();
        }
    }
}
