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
    public sealed class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<Recipe>> AddMemberToRecipe(MemberRecipe entity)
        {
            var chk = await _repository.GetByIdAsync(entity.RecipeId);

            if (chk.Success == false)
            {
                    var create = await _repository.AddMemberToRecipeAsync(entity);
                    if (create.Success == false)
                    {
                        create.Success = false;
                        create.Message = "Error adding members to recipe";
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
                if(!oldMembersIds.Contains(entity.MemberId))
                {
                    tempList.Add(entity.MemberId);
                }
            });

            var tempList2 = tempList.Distinct().ToImmutableList();

            if (tempList2.Count == 0 || tempList2 is null) 
            {
                chk.Success = false;
                chk.Message = "Author is already added to the recipe!";
                return chk;
            }
            
            foreach (var item in tempList2) 
            {
                MemberRecipe memberRecipe = new MemberRecipe { MemberId = item, RecipeId = entity.RecipeId};
                var response = await _repository.AddMemberToRecipeAsync(memberRecipe);
            }

            return chk;
        }


        public async Task<RepositoryResponse<Recipe>> CreateAsync(Recipe entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;

            var response = await _repository.CreateAsync(entity);

            return response;
        }

        public async Task<RepositoryResponse<Recipe>> DeleteAsync(int id)
        {
            var response = await _repository.DeleteAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<List<Recipe>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Recipe>> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            return response;
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
            
            return response;
        }

        public Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = _repository.RemoveMemberFromRecipeAsync(memberId, recipeId);

            return response;
        }

        public Task<RepositoryResponse<Recipe>> SoftDeleteAsync(int id)
        {
            return _repository.SoftDeleteAsync(id);
        }

        public async Task<RepositoryResponse<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;

            var response = await _repository.UpdateAsync(id, entity);

            return response;
        }   
    }
}
