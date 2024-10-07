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

        public async Task<CreateResponse> CreateAsync(List<Tag> entity)
        {
            var response = new CreateResponse();

            foreach (var item in entity) 
            { 
                response = await _tagRepository.CreateAsync(item);
            }

            return response;
        }

        public async Task<CreateResponse> ConnectRecipeAndTag(RecipeTagArray dto)
        {
            var response = new CreateResponse();

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

        public async Task<CreateResponse> DeleteAsync(int id)
        {
            var response = await _tagRepository.DeleteAsync(id);

            return response;
        }
    }
}
