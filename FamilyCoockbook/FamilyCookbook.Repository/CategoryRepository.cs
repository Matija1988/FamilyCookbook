using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System.Runtime.CompilerServices;

namespace FamilyCookbook.Repository
{
    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public CategoryRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses) 
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }
        
        //public async Task<RepositoryResponse<Category>> SoftDelete(int id)
        //{
        //    var response = new RepositoryResponse<Category>();

        //    int rowAffected = 0;

        //    try
        //    {
        //        string query = "UPDATE Category " +
        //            "SET IsActive = 0 " +
        //            "WHERE Id = @Id";

        //        using var connection = _context.CreateConnection();

        //        rowAffected = await connection.ExecuteAsync(query, new { Id = id });

        //        response.Success = rowAffected > 0;
        //        response.Message = _successResponses.EntityUpdated().ToString();

        //        return response;
        //    }
        //    catch  
        //    { 
        //        response.Success = false;
        //        response.Message = _errorMessages.NotFound(id).ToString();
        //        return response;
        //    }
            

        //}

    }
}
