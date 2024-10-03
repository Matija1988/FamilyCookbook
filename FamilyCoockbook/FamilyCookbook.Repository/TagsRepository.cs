using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class TagsRepository : ITagRepository
    {
        IErrorMessages _errorMessages;
        ISuccessResponses _successResponses;
        DapperDBContext _context;

        public TagsRepository(IErrorMessages errorMessages, ISuccessResponses success, DapperDBContext context)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = success;
        }

        #region GET METHODS
        public async Task<RepositoryResponse<List<Tag>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Tag>>(); 
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM Tags");

                using var connection = _context.CreateConnection();

                var entity = await connection.QueryAsync<Tag>(query.ToString());

                response.Items = entity.ToList();
                response.Success = true;
                
                return response;

            }
            catch (Exception ex)
            {
                response.Message = _errorMessages.ErrorAccessingDb("Tags", ex);
                response.Success = false;
                return response;
            }
        }

        #endregion
    }
}
