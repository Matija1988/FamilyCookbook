using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    internal class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task<RepositoryResponse<List<Tag>>> GetAllAsync()
        {
            var response = await _tagRepository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Tag>> CreateAsync(List<Tag> entity)
        {
            var response = new RepositoryResponse<Tag>();

            foreach (var item in entity) 
            { 
                response = await _tagRepository.CreateAsync(item);
            }

            return response;
        }
    }
}
