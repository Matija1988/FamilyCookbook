
namespace FamilyCookbook.Repository
{
    public sealed class RoleRepository : IRoleRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public RoleRepository
            (DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
        {
            _dbContext = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        public async Task<RepositoryResponse<Lazy<List<Role>>>> GetAllAsync()
        {
            var response = new RepositoryResponse<Lazy<List<Role>>>();
            try
            {
                StringBuilder query = new StringBuilder("SELECT * FROM Role");

                using var connection = _dbContext.CreateConnection();
                    
                var entities = await connection.QueryAsync<Role>(query.ToString());

                response.Success = true;
                response.Items = new Lazy<List<Role>>(() => entities.ToList());
                return response;

            }
            catch (Exception ex) 
            {
                response.Message = _errorMessages.ErrorAccessingDb("Role");
                response.Success = false;
                return response;
            }
            finally 
            { 
                _dbContext.CreateConnection().Close();
            }


        }

        public async Task<RepositoryResponse<Role>> GetByIdAsync(int id)
        {

            var response = new RepositoryResponse<Role>();
            try
            {
                StringBuilder query = new StringBuilder($"SELECT * FROM Role WHERE Id = {id}");

                using var connection = _dbContext.CreateConnection();

                var entities = await connection.QuerySingleAsync<Role>(query.ToString());

                response.Success = true;
                response.Items = entities;
                return response;

            }
            catch (Exception ex) 
            {
                response.Message = _errorMessages.ErrorAccessingDb("Role", ex);
                response.Success = false;
                return response;
            }
            finally 
            {
                _dbContext.CreateConnection().Close();
            }
        }
    }
}
