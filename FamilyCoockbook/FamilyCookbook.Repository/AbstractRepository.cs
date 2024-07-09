using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        private readonly DapperDBContext _context;

        
        
        public AbstractRepository(DapperDBContext context)
        {
            _context = context;

        }

        public async Task<RepositoryResponse<List<T>>> GetAllAsync()
        {
            var response = new RepositoryResponse<List<T>>();

            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName};";

                using var connection = _context.CreateConnection();

                var list = await connection.QueryAsync<T>(query);

                response.Items = (List<T>)list;
                response.Success = true;
                response.TotalCount = list.Count();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorAccessingDb(GetTableName()).ToString();
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
                string tableName = GetTableName();

                string query = $"SELECT * FROM {tableName} WHERE Id = {id};";

                using var connection = _context.CreateConnection();

                var entity = await connection.QueryFirstOrDefaultAsync<T>(query, new { id });

                response.Items = entity;
                response.Success = true;

                return response;

            } catch (Exception ex) 
            {
                response.Success = false;
                response.Message = ErrorMessages.NotFound(id).ToString() + " " + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<T>> CreateAsync(T entity)
        {
            var response = new RepositoryResponse<T>();

            int rowsAffected = 0;
            string tableName = GetTableName();

            try
            {
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);

                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties});";
                
                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query, entity);

                response.Success = rowsAffected > 0;
                response.Message = SuccessResponses.EntityCreated().ToString();

                return response;

            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = ErrorMessages.ErrorCreatingEntity(tableName).ToString() + " " + ex.Message;
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

                StringBuilder query = new StringBuilder();

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

                var parameters = new DynamicParameters(entity);
                parameters.Add(keyProperty, id);

                using var connection = _context.CreateConnection();

                rowsAffected = connection.Execute(query.ToString(), parameters);

                response.Success = rowsAffected > 0;
                response.Message = SuccessResponses.EntityUpdated().ToString();

                return response;


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.NotFound(id).ToString() + ex.Message;
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

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
                response.Message = SuccessResponses.EntityDeleted(tableName).ToString();
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

    }
}
