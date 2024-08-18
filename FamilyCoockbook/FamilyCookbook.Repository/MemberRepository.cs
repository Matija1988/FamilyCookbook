using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public MemberRepository
            (DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        public async Task<RepositoryResponse<Member>> CreateAsync(Member entity)
        {
            var response = new RepositoryResponse<Member>();

            int rowsAffeted = 0;

            try
            {
                var query = "INSERT INTO Member " +
                    "(UniqueId, " +
                    "FirstName, " +
                    "LastName, " +
                    "DateOfBirth, " +
                    "Bio, " +
                    "IsActive, " +
                    "DateCreated, " +
                    "DateUpdated, " +
                    "Username, " +
                    "Password, " +
                    "RoleId) " +
                    "VALUES " +
                    "(@UniqueId, " +
                    "@FirstName, " +
                    "@LastName, " +
                    "@DateOfBirth, " +
                    "@Bio, " +
                    "@IsActive, " +
                    "@DateCreated, " +
                    "@DateUpdated," +
                    "@Username," +
                    "@Password," +
                    "@RoleId)";

                using var connection = _context.CreateConnection();

                rowsAffeted = await connection.ExecuteAsync(query, entity);

                response.Success = rowsAffeted > 0;
                response.Message = _successResponses.EntityCreated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorCreatingEntity(" Member ").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Member>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                var query = "UPDATE Member " +
                    "SET IsActive = 0 " +
                    "WHERE Id = @Id";

                using var connection = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new { Id = id });

                response.Success = rowAffected > 0;
                response.Message = _successResponses.EntityUpdated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.NotFound(id).ToString() + ex.Message;

                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<List<Member>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<Member>>();

            try
            {

                var query = "SELECT  a.*, " +
                    "b.Id AS RoleRoleId, " +
                    "b.* " +
                    "FROM Member a " +
                    "LEFT JOIN Role b on a.RoleId = b.Id " +
                    "WHERE a.IsActive = 1;" +
                    "" +
                    "SELECT COUNT(*) FROM Member WHERE IsActive = 1;";

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query);

                var members = multipleQuery.Read<Member, Role, Member>(
                    (entity, role ) =>
                {
                    if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                    {
                        existingEntity = entity;
                        existingEntity.Recipes = new List<Recipe>();
                        entityDictionary.Add(existingEntity.Id, existingEntity);
                    }
                    if (role != null)
                    {

                        existingEntity.Role = role;
                    }

                    return existingEntity;
                },
                splitOn: "RoleRoleId");

                response.TotalCount = multipleQuery.ReadSingle<int>();

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId)
        {
            var response = new RepositoryResponse<Member>();

            try
            {

                var query = "SELECT a.Id as MemberId," +
                    "a.*, " +
                    "b.Id AS RoleId, " +
                    "b.* " +
                    "FROM Member a " +
                    "LEFT JOIN Role b on a.RoleId = b.Id " +
                    "WHERE a.UniqueId = @UniqueId;";

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Member, Role, Member>
                    (query,
                    (entity, role) =>
                    {
                        if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                        {
                            existingEntity = entity;
                            existingEntity.Recipes = new List<Recipe>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }
                        if (role != null)
                        {

                            existingEntity.Role = role;
                        }

                        return existingEntity;
                    },
                    new { UniqueId = uniqueId },
                splitOn: "RoleId");

                var result = entityDictionary.Values.FirstOrDefault();

                if (result is null)
                {
                    response.Success = false;
                    response.Message = _errorMessages.NotFoundByGuid().ToString();
                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Member>> GetByIdAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            try
            {

                var query = "SELECT a.Id as MemberId," +
                    "a.*, " +
                    "b.Id AS RoleId, " +
                    "b.* " +
                    "FROM Member a " +
                    "LEFT JOIN Role b on a.RoleId = b.Id " +
                    "WHERE a.Id = @Id;";

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                var entities = await connection.QueryAsync<Member, Role, Member>
                    (query,
                    (entity, role) =>
                    {
                        if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                        {
                            existingEntity = entity;
                            existingEntity.Recipes = new List<Recipe>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }
                        if (role != null)
                        {

                            existingEntity.Role = role;
                        }
 
                        return existingEntity;
                    },
                    new { Id = id },
                splitOn: "RoleId");

                var result = entityDictionary.Values.FirstOrDefault();

                if (result is null)
                {
                    response.Success = false;
                    response.Message = _errorMessages.NotFound(id).ToString();
                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<List<Member>>> PaginateAsync(Paging paging, MemberFilter filter)
        {
            var response = new RepositoryResponse<List<Member>>();

            try
            {

                //var query = @"SELECT  a.*, " +
                //    "b.Id AS RoleRoleId, " +
                //    "b.* " +
                //    "FROM Member a " +
                //    "LEFT JOIN Role b on a.RoleId = b.Id " +
                //    "WHERE a.IsActive = 1 " +
                //    "ORDER BY a.Id " +
                //    "OFFSET @Offset ROWS " +
                //    "FETCH NEXT @PageSize ROWS ONLY;" +
                //    "" +
                //    "SELECT COUNT(*) FROM Member WHERE IsActive = 1;";

                string query = QueryBuilder(paging, filter);

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query, new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize,
                });

                var members = multipleQuery.Read<Member, Role, Member>(
                    (entity, role) =>
                    {
                        if (!entityDictionary.TryGetValue(entity.Id, out var existingEntity))
                        {
                            existingEntity = entity;
                            existingEntity.Recipes = new List<Recipe>();
                            entityDictionary.Add(existingEntity.Id, existingEntity);
                        }
                        if (role != null)
                        {

                            existingEntity.Role = role;
                        }

                        return existingEntity;
                    },
                splitOn: "RoleRoleId");

                response.TotalCount = multipleQuery.ReadSingle<int>();

                response.Success = true;
                response.Items = entityDictionary.Values.ToList();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        private string QueryBuilder(Paging paging, MemberFilter filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT  a.*, " +
                "b.Id AS RoleRoleId, " +
                "b.* " +
                "FROM Member a " +
                "LEFT JOIN Role b on a.RoleId = b.Id " +
                "WHERE a.IsActive = 1 ");

            if (!string.IsNullOrWhiteSpace(filter.SearchByFirstName))
            {
                query.Append($"AND a.FirstName LIKE '%{filter.SearchByFirstName}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByLastName)) 
            { 
                query.Append($"AND a.LastName LIKE '%{filter.SearchByLastName}%' ");
            }

            if(!string.IsNullOrWhiteSpace(filter.SearchByBio))
            {
                query.Append($"AND a.Bio LIKE '%{filter.SearchByBio}%' "); 
            }
            

            if (filter.SearchByDateOfBirth.HasValue) 
            {
                query.Append($"AND a.DateOfBirth = {filter.SearchByDateOfBirth}");
            }

            if(!filter.SearchByRoleId.Equals(null))
            {
                query.Append($"AND a.RoleId = {filter.SearchByRoleId}");
            }

            if (!filter.SearchByActivtyStatus.Equals(null))
            {
                query.Append($"AND a.IsActive = {filter.SearchByActivtyStatus} ");
            }

            query.Append("ORDER BY a.LastName ");
            query.Append($"OFFSET @Offset ROWS ");
            query.Append($"FETCH NEXT @PageSize ROWS ONLY;");

            query.Append($" SELECT COUNT(*) FROM Member WHERE IsActive = {filter.SearchByActivtyStatus};");


            //var query = @" +
            //    "ORDER BY a.Id " +
            //    "OFFSET @Offset ROWS " +
            //    "FETCH NEXT @PageSize ROWS ONLY;" +
            //    "" +
            //    "SELECT COUNT(*) FROM Member WHERE IsActive = 1;";

            return query.ToString();
        }

        public async Task<RepositoryResponse<Member>> DeleteAsync(int id)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                string query = "DELETE FROM Member WHERE Id = @Id;";

                using var connection = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new { Id = id });

                response.Success = rowAffected > 0;
                response.Message = _successResponses.EntityDeleted("Member").ToString();
                return response;
        
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.NotFound(id).ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Member>> UpdateAsync(int id, Member entity)
        {
            var response = new RepositoryResponse<Member>();

            int rowAffected = 0;

            try
            {
                var query = "UPDATE Member " +
                    "SET FirstName = @FirstName, " +
                    "LastName = @LastName, " +
                    "DateOfBirth = @DateOfBirth, " +
                    "Bio = @Bio, " +
                    "DateUpdated = @DateUpdated, " +
                    "Username = @Username, " +
                    "Password = @Password, " +
                    "RoleId = @RoleId " +
                    "WHERE Id = @Id;";

                using var connection  = _context.CreateConnection();

                rowAffected = await connection.ExecuteAsync(query, new
                {
                    entity.FirstName,
                    entity.LastName,
                    entity.DateOfBirth,
                    entity.Bio,
                    entity.DateUpdated,
                    entity.Username,
                    entity.Password,
                    entity.RoleId,
                    Id = id
                });

                response.Success = rowAffected > 0;
                response.Message = _successResponses.EntityUpdated().ToString();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member").ToString() + ex.Message;    
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }
    }
}
