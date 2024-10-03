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
            } finally
            {
                _context.CreateConnection().Close();
            }
        }

        #endregion

        public async Task<RepositoryResponse<Tag>> CreateAsync(Tag entity)
        {
            var response = new RepositoryResponse<Tag>();

            int rowsAffected = 0;

            try
            {
                StringBuilder query = new();
                query.Append("INSERT INTO Tags (Text) VALUES (@Text);");

                using var connection = _context.CreateConnection();

                var entites = await connection.ExecuteAsync(query.ToString(), entity);

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorCreatingEntity("Tag");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }
    }
}
