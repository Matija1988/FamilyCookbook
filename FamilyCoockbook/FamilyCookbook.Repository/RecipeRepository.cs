using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DapperDBContext _context;

        public RecipeRepository(DapperDBContext context)
        {
            _context = context;
        }

        public Task<RepositoryResponse<Recipe>> CreateAsync(Recipe entity)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Recipe>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<List<Recipe>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Recipe>>();

            try
            {

            }
            catch (Exception ex)
            {

            }
            finally 
            { 
            
            }

            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Recipe>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            throw new NotImplementedException();
        }
    }
}
