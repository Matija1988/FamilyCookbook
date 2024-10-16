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
    public class BannerPositionRepository : IBannerPositionRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public BannerPositionRepository(DapperDBContext dapperDBContext,
            IErrorMessages errorMessages,
            ISuccessResponses successResponses)
        {
            _dbContext = dapperDBContext;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        public async Task<MessageResponse> AssignBannerToPosition(BannerPosition bannerPosition)
        {
            var response = new MessageResponse();
            int rowsAffected = 0;

            try
            {
                StringBuilder query = new("UPDATE BannerPosition SET BannerId = @BannerId " +
                    "WHERE Position = @Position");
                using var connection = _dbContext.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), bannerPosition);

                response.IsSuccess = rowsAffected > 0;

                if (!response.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = new StringBuilder($"Invalid inputs!");
                    return response;
                }

                response.Message = _successResponses.EntityUpdated();
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.ErrorAccessingDb("BannerPosition");
                return response;
            }
            finally
            {
                _dbContext.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Banner>> GetBannerForPosition(int position)
        {
            var response = new RepositoryResponse<Banner>();

            try
            {
                StringBuilder query = new("SELECT TOP 1 a.* FROM Banner a JOIN BannerPosition b " +
                    $"ON a.Id = b.BannerId WHERE b.Position = {position} ORDER BY a.DateCreated DESC;");

                var connection = _dbContext.CreateConnection();

                var entity = await connection.QueryFirstOrDefaultAsync<Banner>(query.ToString(), position);

                if (entity == null)
                {
                    response.Success = false;
                    response.Message = new StringBuilder($"No banner for possition {position} found!!!");
                    return response;
                }

                response.Success = true;
                response.Items = entity;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Banner", ex);
                return response;
            }
            finally
            {
                _dbContext.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<List<BannerPosition>>> GetAllBannerPositions()
        {
            var response = new RepositoryResponse<List<BannerPosition>>();
            try
            {
                StringBuilder query = new ("SELECT * FROM BannerPosition;");

                using var connection = _dbContext.CreateConnection();   

                var entities = await connection.QueryAsync<BannerPosition>(query.ToString());

                response.Success = true;
                response.Items = entities.ToList();
                return response;

            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = _errorMessages.ErrorAccessingDb("BannerPosition");
                return response;
            }
            finally
            {
                _dbContext.CreateConnection().Close();
            }
        }
    }
}
