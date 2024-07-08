using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;

namespace FamilyCookbook.Repository
{
    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        private readonly DapperDBContext _context;

        public CategoryRepository(DapperDBContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<RepositoryResponse<Category>> DeleteAsync(int id)
        {
            var response = new RepositoryResponse<Category>();

            try
            {
                var query = "DELETE FROM Category " +
                    "WHERE Id = @Id;";
                
                using var connection = _context.CreateConnection();

                await connection.ExecuteAsync(query, new { Id = id});

                response.Success = true;
                response.Message = SuccessResponses.EntityDeleted("Category").ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.NotFound(id).ToString();
                return response;
            }
            finally 
            { 
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Category>> UpdateAsync(int id, Category entity)
        {
            var response = new RepositoryResponse<Category>();

            try
            {
                var query = "UPDATE Category " +
                    "SET Name = @Name, " + 
                    "Description = @Description " +
                    "WHERE Id = @Id";

                using var connection = _context.CreateConnection();

                await connection.ExecuteAsync(query, entity);

                response.Success = true;

                return response;
            }
            catch (Exception ex) 
            {
                response.Success= false;
                response.Message = ErrorMessages.NotFound(id).ToString();
                return response;    
            }
            finally 
            { 
                _context.CreateConnection().Close();    
            }

        }


        //public async Task<RepositoryResponse<Category>> CreateAsync(Category entity)
        //{
        //    var response = new RepositoryResponse<Category>();

        //    try
        //    {

        //        var query = "INSERT INTO Category " +
        //            "(Name, Description) " +
        //            "VALUES " +
        //            "(@Name, @Description);";

        //        using var connection = _context.CreateConnection();

        //        await connection.ExecuteAsync(query, entity);

        //        response.Items = entity;
        //        response.Success = true;
                
        //        return response;
        //    }
        //    catch (Exception ex) 
        //    {
        //        response.Success = false;
        //        response.Message = ErrorMessages.ErrorCreatingEntity("Category").ToString();
                    
        //        return response;
            
        //    }
        //    finally
        //    {
        //        _context.CreateConnection().Close();
        //    }

        //}

        //#region GET METHODS

        //public async Task<RepositoryResponse<List<Category>>> GetAllAsync()
        //{
        //    var response = new RepositoryResponse<List<Category>>();
        //    try
        //    {
        //        var query = "SELECT * FROM Category;";

        //        using var connection = _context.CreateConnection();

        //        var list = await connection.QueryAsync<Category>(query);

        //        response.Success = true;
        //        response.Items = list.ToList();
        //        response.TotalCount = list.Count();

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ErrorMessages.ErrorAccessingDb("Categories").ToString();

        //        return response;
        //    }
        //    finally 
        //    { 
        //        _context?.CreateConnection().Close();
        //    }
        //}

        //public async Task<RepositoryResponse<Category>> GetByIdAsync(int id)
        //{
        //    var response = new RepositoryResponse<Category>();

        //    try
        //    {
        //        var query = "SELECT * FROM Category WHERE Id = @Id";

        //        using var connection = _context.CreateConnection();

        //        var entity = await connection.QueryFirstOrDefaultAsync<Category>(query, new { id });

        //        response.Success = true;
        //        response.Items = entity;
        //        response.TotalCount = 1;

        //        if(response.Items is null)
        //        {
        //            response.Success = false;
        //            response.Message = ErrorMessages.NotFound(id).ToString();
        //            response.TotalCount = 0;
        //            return response;
        //        }

        //        return response;

        //    }
        //    catch (Exception ex) 
        //    { 
        //        response.Success = false;
        //        response.Message = ErrorMessages.ErrorAccessingDb("Categories").ToString();

        //        return response;
            
        //    } finally 
        //    { 
        //        _context?.CreateConnection().Close(); 
        //    }

        //}

        
    }
}
