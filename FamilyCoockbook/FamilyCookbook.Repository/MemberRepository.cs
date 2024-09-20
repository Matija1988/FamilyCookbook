using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public sealed class MemberRepository : IMemberRepository
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
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorCreatingEntity(" Member ");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        #region DELETE METHODS
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
                response.Message = _successResponses.EntityUpdated();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.NotFound(id);

                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
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
                response.Message = _successResponses.EntityDeleted("Member");
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.NotFound(id);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }
        #endregion

        #region GET METHODS
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

                IEnumerable<Member> members = multipleQuery.Read<Member, Role, Member>(
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
                response.Message = _errorMessages.ErrorAccessingDb("Member");
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
                    response.Message = _errorMessages.NotFoundByGuid();
                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member");
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
                    response.Message = _errorMessages.NotFound(id);
                    return response;
                }

                response.Success = true;
                response.Items = result;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member");
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }
        }

        public async Task<RepositoryResponse<Lazy<List<Member>>>> PaginateAsync(Paging paging, MemberFilter filter)
        {
            var response = new RepositoryResponse<Lazy<List<Member>>>();

            try
            {

                string query = QueryBuilder(paging, filter);

                var entityDictionary = new Dictionary<int, Member>();

                using var connection = _context.CreateConnection();

                using var multipleQuery = await connection.QueryMultipleAsync(query, new
                {
                    Offset = (paging.PageNumber - 1) * paging.PageSize,
                    PageSize = paging.PageSize,
                });

                IEnumerable<Member> members = multipleQuery.Read<Member, Role, Member>(
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
                response.Items = new Lazy<List<Member>>(() => entityDictionary.Values.ToList());
                
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member");
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
            StringBuilder countQuery = 
                new StringBuilder($" SELECT COUNT(*) FROM Member a " +
                $"LEFT JOIN Role b on a.RoleId = b.Id " +
                $"WHERE " +
                $"a.IsActive = {filter.SearchByActivityStatus} ");


            query.Append("SELECT  a.*, " +
                "b.Id AS RoleRoleId, " +
                "b.* " +
                "FROM Member a " +
                "LEFT JOIN Role b on a.RoleId = b.Id " +
                "WHERE a.IsActive = 1 ");

            if (!string.IsNullOrWhiteSpace(filter.SearchByFirstName))
            {
                query.Append($"AND a.FirstName LIKE '%{filter.SearchByFirstName}%' ");
                countQuery.Append($" AND a.FirstName LIKE '%{filter.SearchByFirstName}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByLastName))
            {
                query.Append($"AND a.LastName LIKE '%{filter.SearchByLastName}%' ");
                countQuery.Append($" AND a.LastName LIKE '%{filter.SearchByLastName}%' ");
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchByBio))
            {
                query.Append($"AND a.Bio LIKE '%{filter.SearchByBio}%' ");
                countQuery.Append($" AND a.Bio LIKE '%{filter.SearchByBio}%' ");
            }

            if (filter.SearchByDateOfBirth.HasValue)
            {
                query.Append($"AND a.DateOfBirth = {filter.SearchByDateOfBirth} ");
                countQuery.Append($" AND a.DateOfBirth = {filter.SearchByDateOfBirth} ");
            }

            if (!filter.SearchByRoleId.Equals(null))
            {
                query.Append($"AND a.RoleId = {filter.SearchByRoleId} ");
                countQuery.Append($" AND a.RoleId = {filter.SearchByRoleId} ");
            }

            if (!filter.SearchByActivityStatus.Equals(null))
            {
                query.Append($"AND a.IsActive = {filter.SearchByActivityStatus} ");
                countQuery.Append($" AND a.IsActive = {filter.SearchByActivityStatus} ");
            }

            query.Append("ORDER BY a.LastName ");
            query.Append($"OFFSET @Offset ROWS ");
            query.Append($"FETCH NEXT @PageSize ROWS ONLY;");

            query.Append(countQuery);
            
            return query.ToString();
        }

        #endregion


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
                response.Message = _successResponses.EntityUpdated();

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Member");    
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }
    }
}
