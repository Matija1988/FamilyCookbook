using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FamilyCookbook.Repository
{
    
    public class DapperDBContext
    {
        private readonly string _connectionString;

        public DapperDBContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() 
        {
            return new SqlConnection(_connectionString);
        }
    }
}
