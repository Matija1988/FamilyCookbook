using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System.Collections.Immutable;
using System.Text;

namespace FamilyCookbook.Service
{
    public sealed class RecipeService : AbstractService<Recipe>, IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly ICommentRepository _commentRepository;

        public RecipeService(IRecipeRepository repository,
            ICommentRepository commentRepository) : base(repository)
        {
            _repository = repository;
            _commentRepository = commentRepository;
        }

        public async Task<RepositoryResponse<Recipe>> AddMemberToRecipe(MemberRecipe entity)
        {
            var chk = await _repository.GetByIdAsync(entity.RecipeId);

            StringBuilder errorBuilder = new StringBuilder();

            if (chk.Success == false)
            {
                var create = await _repository.AddMemberToRecipeAsync(entity);
                if (create.Success == false)
                {

                    create.Success = false;
                    create.Message = errorBuilder.Append("Error adding members to recipe");
                    return create;
                }
                return create;

            }

            var oldMembers = chk.Items.Members;

            var oldMembersIds = new List<int>();

            foreach (var member in oldMembers)
            {
                oldMembersIds.Add(member.Id);
            }

            var tempList = new List<int>();

            oldMembersIds.ForEach(x => {
                if (!oldMembersIds.Contains(entity.MemberId))
                {
                    tempList.Add(entity.MemberId);
                }
            });

            var tempList2 = tempList.Distinct().ToImmutableList();

            if (tempList2.Count == 0 || tempList2 is null)
            {
                chk.Success = false;
                chk.Message = errorBuilder.Append("Author is already added to the recipe!");
                return chk;
            }

            foreach (var item in tempList2)
            {
                MemberRecipe memberRecipe = new MemberRecipe { MemberId = item, RecipeId = entity.RecipeId };
                var response = await _repository.AddMemberToRecipeAsync(memberRecipe);
            }

            return chk;
        }

        public async Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors()
        {
            var resposne = await _repository.GetRecipesWithoutAuthors();


            return resposne;
        }

        public async Task<RepositoryResponse<List<Recipe>>> PaginateAsync(Paging paging, RecipeFilter filter)
        {
            var response = await _repository.PaginateAsync(paging, filter);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            if (response.Items.Count > 0)
            {
                foreach (var item in response.Items)
                {
                    item.AverageRating = await CalculateAverageRating(item.Id);
                }
            }
            
            return response;
        }

        public Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = _repository.RemoveMemberFromRecipeAsync(memberId, recipeId);

            return response;
        }

        public async Task<MessageResponse> UpdateAsync(int id, RecipeCreateDTO entity)
        {
            var response = new MessageResponse();
            var memberChk = await _repository.GetByIdAsync(id);

            if (!memberChk.Success)
            {
                response.IsSuccess = false;
                response.Message = memberChk.Message;
                return response;
            }


            int[] tagDistArray = Array.Empty<int>();

            if (entity.TagIds != null || entity.TagIds.Length > 0) {
                tagDistArray = entity.TagIds.Distinct().ToArray();
            }

            List<int> oldMemberIds = new();

            foreach(var member in memberChk.Items.Members)
            {
                oldMemberIds.Add(member.Id);
            }

            var tempList = ReturnDistinctList(entity.MemberIds, oldMemberIds);

            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;
            entity.Picture.IsActive = true;
            entity.MemberIds = tempList;
            
            entity.TagIds = tagDistArray;

            response = await _repository.UpdateAsync(id, entity);

            return response;
        }

        private List<int> ReturnDistinctList(List<int> intList, List<int> oldMemberIds)
        {
            var tempList = intList.Distinct().ToList();
            
            if (tempList.Count > 0)
            {
               return intList = tempList;
            }
            else
            {
                return intList = oldMemberIds.Distinct().ToList();
            }
        }

        public async Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId)
        {
            var response = await _repository.AddPictureToRecipeAsync(pictureId, recipeId);

            return response;
        }

        public async Task<MessageResponse> CreateAsync(RecipeCreateDTO entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;
            entity.Picture.IsActive = true;

            entity.MemberIds = entity.MemberIds.Distinct().ToList();

            if (entity.TagIds != null)
            {
              entity.TagIds = entity.TagIds.Distinct().ToArray();
            }

            var response = await _repository.CreateAsyncTransaction(entity);

            return response;
        }

        protected override async Task<RepositoryResponse<Recipe>> ReturnEntity(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            if(!response.Success)
            {
                return response;
            }

            response.Items.AverageRating = await CalculateAverageRating(id);
            var distinctMembers = response.Items.Members.GroupBy(m => m.Id).Select(g => g.First()).ToList();
            response.Items.Members = distinctMembers;

            var distinctTags = response.Items.Tags.GroupBy(t => t.Id).Select(g => g.First()).ToList();
            response.Items.Tags = distinctTags;

            return response; 
        }

        protected override async Task<RepositoryResponse<List<Recipe>>> ReturnEntities()
        {
            var recipeResponse = await _repository.GetAllAsync();

            if (!recipeResponse.Success || recipeResponse.Items.Count == 0)
            {
                return recipeResponse;
            }

            foreach (var recipe in recipeResponse.Items) 
            {
                recipe.AverageRating = await CalculateAverageRating(recipe.Id); ;
            }

            return recipeResponse;
        }

        private async Task<double> CalculateAverageRating(int recipeId)
        {
            var commentResponse = await _commentRepository.GetAllAsync();

            double averageRating = 0.0;

            var recipeComments = commentResponse.Items
                .Where(c => c.RecipeId == recipeId && c.IsActive).ToList();

            if(recipeComments.Count != 0)
            {
                averageRating = recipeComments.Average(c => c.Rating) ?? 0;

                return averageRating;
                
            }
            return averageRating;

        }

    }
}
