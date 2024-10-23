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
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        private readonly ICommentRepository _commentRepository;
        public SearchService(ISearchRepository searchRepository, ICommentRepository commentRepository)
        {
            _searchRepository = searchRepository;
            _commentRepository = commentRepository;
        }
        public async Task<RepositoryResponse<List<Recipe>>> GetAllBySearchText(string searchText)
        {
            var response = await _searchRepository.GetAllBySearchText(searchText);

            double averageRating = 0.0;

            if (response.Success)
            {

                foreach (var item in response.Items)
                {
                    averageRating = await CalculateAverageRating(item.Id);
                    item.AverageRating = averageRating;
                }
            }

            return response;
        }

        private async Task<double> CalculateAverageRating(int recipeId)
        {
            var commentResponse = await _commentRepository.GetAllAsync();

            double averageRating = 0.0;

            var recipeComments = commentResponse.Items.Where(c => c.RecipeId == recipeId && c.IsActive).ToList();

            if (recipeComments.Count != 0) { averageRating = recipeComments.Average(c => c.Rating) ?? 0; }

            return averageRating;
        }
    }
}
