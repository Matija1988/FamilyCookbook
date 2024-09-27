using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FamilyCookbook.Repository
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        
        
        public AbstractRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

        #region GET 

        public async Task<RepositoryResponse<List<T>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<T>>();

            try
            {
                string query = BuildQueryReadAll().ToString();

                using var connection = _context.CreateConnection();

                var list = BuildQueryCommand(query, connection);
                    
                response.Items = await list;
                response.Success = true;
                response.TotalCount = response.Items.Count();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb(GetTableName());
                return response;
            }
            finally 
            { 
                _context.CreateConnection().Close();
            }


        }

        
        public async Task<RepositoryResponse<T>> GetByIdAsync(int id)
        {
            var response = new RepositoryResponse<T>();

            try
            {
                
                string query = BuildQueryReadSingle(id).ToString();

                using var connection = _context.CreateConnection();

                var entity = await BuildQueryCommand(query, connection, id);                    
                
                response.Items = entity;

                if(entity == null)
                {
                    response.Success = false;
                    response.Message = _errorMessages.NotFound(id);
                    return response;
                }

                response.Success = true;

                return response;

            } catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb(GetTableName());
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        #endregion

        public async Task<RepositoryResponse<T>> CreateAsync(T entity)
        {
            var response = new RepositoryResponse<T>();

            int rowsAffected = 0;
            string tableName = GetTableName();

            try
            {
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);

                var query = BuildCreateQuery(tableName, columns, properties);
                            
                using var connection = _context.CreateConnection();

                rowsAffected = await BuildCreateQueryCommand(query.ToString(), connection, entity);
                    
                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorCreatingEntity(tableName, ex);
                return response;
            }
            finally 
            { 
                _context.CreateConnection().Close();
            }

       }

        public async Task<RepositoryResponse<T>> UpdateAsync(int id, T entity)
        {
            var response = new RepositoryResponse<T>();

            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                var query = BuildUpdateQuery(tableName, keyColumn, keyProperty);

                var parameters = new DynamicParameters(entity);
                parameters.Add(keyProperty, id);

                using var connection = _context.CreateConnection();

                rowsAffected = connection.Execute(query.ToString(), parameters);

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityUpdated();

                return response;


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.NotFound(id, ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        protected virtual StringBuilder BuildUpdateQuery(string tableName, string keyColumn, string keyProperty)
        {
            StringBuilder query = new();

            query.Append($"UPDATE {tableName} SET ");

            foreach (var property in GetProperties(true))
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                string columnName = columnAttr?.Name ?? property.Name;
                string propertyName = property.Name;

                query.Append($"{columnName} = @{propertyName},");
            }
            query.Remove(query.Length - 1, 1);

            query.Append($" WHERE {keyColumn} = @{keyProperty};");

            return query;
        }

        public async Task<RepositoryResponse<T>> DeleteAsync(int id)
        {
            var response = new RepositoryResponse<T>(); 

            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty};";

                using var connection =  _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, new { id });

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityDeleted(tableName);
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

        public async Task<RepositoryResponse<T>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<T>();

            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string query = $"UPDATE {tableName} SET IsActive = 0 WHERE Id = {id};";

                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, new { id });

                if(rowsAffected == 0)
                {
                    response.Success = false;
                    response.Message = _errorMessages.NotFound(id);
                    return response;
                }

                response.Success = rowsAffected > 0;
                response.Message = _successResponses.EntityDeleted(tableName);
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

        #region PRIVATE METHODS

        private IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        private string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }
            return null;

        }

        private static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);
                if (keyAttributes.Length > 0)
                {
                    object[] columnAttr = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                    if (columnAttr != null && columnAttr.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttr[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }

            }
            return null;
        }





        private string GetPropertyNames(bool excludeKey)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        private string GetTableName()
        {
            string tableName = "";

            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();

            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name;
        }

        #endregion


        #region PROTECTED VIRTUAL METHODS

        protected virtual StringBuilder BuildQueryReadAll() 
        { 
            StringBuilder query = new StringBuilder();
            var tableName = GetTableName();

            return query.Append($"SELECT * FROM {tableName} WHERE IsActive = 1");

        }

        protected virtual StringBuilder BuildQueryReadSingle(int id) 
        {
            StringBuilder query = new StringBuilder();

            var tableName = GetTableName();

            return query.Append($"SELECT * FROM {tableName} WHERE Id = @id");  

        }

        protected virtual StringBuilder BuildCreateQuery(string tableName, string columns, string properties)
        {
            StringBuilder queryBuilder = new();

            return queryBuilder.Append($"INSERT INTO {tableName} ({columns}) VALUES ({properties});");
        }

        protected virtual async Task<List<T>> BuildQueryCommand(string query, System.Data.IDbConnection connection)
        {
            var entity = await connection.QueryAsync<T>(query);

            return entity.ToList();
        }

        protected virtual async Task<T> BuildQueryCommand(string query, System.Data.IDbConnection connection, int id)
        {
            var entity = await connection.QueryFirstOrDefaultAsync<T>(query, new { id });

            return entity;
        }

        protected virtual async Task<int> BuildCreateQueryCommand(string query, System.Data.IDbConnection connection, T entity)
        {
            return await connection.ExecuteAsync(query, entity);
        }

        #endregion
    }
}
