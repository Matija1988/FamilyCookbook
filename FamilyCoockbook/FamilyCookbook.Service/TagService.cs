using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class TagService : ITagService
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

        public async Task<RepositoryResponse<List<Tag>>> GetByTextAsync(string text)
        {
            var response = await _tagRepository.GetByTextAsync(text);

            return response;
        }

        public async Task<MessageResponse> CreateAsync(List<Tag> entity)
        {
            var response = new MessageResponse();

            foreach (var item in entity) 
            { 
                response = await _tagRepository.CreateAsync(item);
            }

            return response;
        }

        public async Task<MessageResponse> ConnectRecipeAndTag(RecipeTagArray dto)
        {
            var response = new MessageResponse();

            for (int j = 0; j < dto.tagId.Length; j++)
            {
                var recipeTag = new RecipeTag(recipeId: dto.recipeId, tagId: dto.tagId[j]);
                response = await _tagRepository.ConnectRecipeAndTag(recipeTag);

            }

            return response;
        }

        public async Task<RepositoryResponse<ImmutableList<Tag>>> PaginateAsync(Paging paging, string text)
        {
            var response = await _tagRepository.PaginateAsync(paging, text);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;
        }

        public async Task<MessageResponse> DeleteAsync(int id)
        {
            var response = await _tagRepository.DeleteAsync(id);

            return response;
        }

        public async Task<MessageResponse> UpdateAsync(int id, Tag tag)
        {
            return await _tagRepository.UpdateAsync(id, tag);
        }

        public Task<RepositoryResponse<Tag>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Tag>> SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
