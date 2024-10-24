﻿using AngleSharp.Common;
using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using System.Linq;

namespace FamilyCookbook.Repository
{
    public abstract class AbstractRepository<T, Filter> : IRepository<T, Filter> where T : class
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
                response.Message = _errorMessages.ErrorAccessingDb(GetTableName(), ex);
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

                if (entity == null)
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
                response.Message = _errorMessages.ErrorAccessingDb(GetTableName(),ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        #endregion

        public async Task<MessageResponse> CreateAsync(T entity)
        {
            var response = new MessageResponse();

            int rowsAffected = 0;
            string tableName = GetTableName();

            try
            {
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);

                var query = BuildCreateQuery(tableName, columns, properties);
                            
                using var connection = _context.CreateConnection();

                rowsAffected = await BuildCreateQueryCommand(query.ToString(), connection, entity);
                    
                response.IsSuccess = rowsAffected > 0;
                response.Message = _successResponses.EntityCreated();

                return response;

            }
            catch (Exception ex) 
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.ErrorCreatingEntity(tableName, ex);
                return response;
            }
            finally 
            { 
                _context.CreateConnection().Close();
            }

       }

        public async Task<MessageResponse> UpdateAsync(int id, T entity)
        {
            var response = new MessageResponse();

            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                var query = BuildUpdateQuery(tableName, keyColumn, keyProperty, id);

                var parameters = new DynamicParameters(entity);
                parameters.Add(keyProperty, id);

                using var connection = _context.CreateConnection();

                rowsAffected = await BuildUpdateCommand(query.ToString(), connection, parameters);
                  
                response.IsSuccess = rowsAffected > 0;
                response.Message = _successResponses.EntityUpdated();

                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.NotFound(id, ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<MessageResponse> DeleteAsync(int id)
        {
            var response = new MessageResponse(); 

            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                var query = BuildPermaDeleteQuery(tableName, keyColumn, id);
             
                using var connection =  _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), new { id });

                response.IsSuccess = rowsAffected > 0;
                response.Message = _successResponses.EntityDeleted(tableName);
                return response;

            }
            catch (SqlException ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.DeleteConstriantError("Recipe");
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = _errorMessages.NotFound(id);
                return response;
            }
            
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        protected virtual StringBuilder BuildPermaDeleteQuery(string tableName, string keyColumn, int id)
        {
            StringBuilder query = new();

            return query.Append($"DELETE FROM {tableName} WHERE {keyColumn} = @id;");

        }

        public async Task<RepositoryResponse<T>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<T>();

            int rowsAffected = 0;

            string tableName = GetTableName();
            
            try
            {
                var query = BuildSoftDeleteQuery(tableName, id);
                    
                using var connection = _context.CreateConnection();

                rowsAffected = await connection.ExecuteAsync(query.ToString(), new { id });

                if (rowsAffected == 0)
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
                response.Message = _errorMessages.ErrorAccessingDb(tableName, ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        public async Task<RepositoryResponse<Lazy<List<T>>>> PaginateAsync(Paging paging, Filter filter)
        {
            var response = new RepositoryResponse<Lazy<List<T>>>();

            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            try
            {
                StringBuilder query = PaginateQueryBuilder(paging, filter, tableName, keyColumn, keyProperty);

                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("Offset", (paging.PageNumber - 1) * paging.PageSize);
                parameters.Add("PageSize", paging.PageSize);

                var filterProperties = filter.GetType().GetProperties();

                foreach (var property in filterProperties) 
                {
                    var value = property.GetValue(filter);
                    if(value != null)
                    {
                        parameters.Add(property.Name, value);
                    }
                
                }

                using var multipleQuery = await connection.QueryMultipleAsync(query.ToString(), parameters);

                var entities = await BuildPaginationCommand(query, multipleQuery);

                response.Success = true;
                response.Items = new Lazy<List<T>>(() => entities.ToList());
                response.TotalCount = await multipleQuery.ReadSingleAsync<int>();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb(tableName, ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }

        //protected virtual StringBuilder PaginateQueryBuilder<Filter>(Paging paging, Filter? filter, string tableName, string keyColumn, string keyPropery)
        //{
        //    StringBuilder countQuery = new($"SELECT COUNT (DISTINCT {keyColumn}) FROM {tableName} WHERE 1 = 1 ");

        //    StringBuilder query = new($"SELECT * FROM {tableName} WHERE 1 = 1 ");

        //    var filterProperties = GetFilterProperties();
        //    var genericProperties = GetProperties();

        //    foreach(var prop in filterProperties) 
        //    { 
        //        var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();

        //        string propName = prop.Name;
        //        string actualPropName = prop.Name.Substring("SearchBy".Length);

        //        if(prop.Name.StartsWith("SearchBy"))
        //        {
        //            var matchingGenericProp = genericProperties
        //                .FirstOrDefault(p => p.Name.Equals(actualPropName, StringComparison.OrdinalIgnoreCase));

        //            if(prop.Name.Equals("SearchByActivityStatus", StringComparison.OrdinalIgnoreCase)) 
        //            {
        //                countQuery.Append($" AND IsActive = @{prop.Name} ");
        //                query.Append($" AND IsActive = @{prop.Name} ");
        //            }

        //            if (matchingGenericProp != null && prop.GetValue(filter) != null) 
        //            { 
        //                string columnName = columnAttr?.Name ?? matchingGenericProp.Name;

        //                if((matchingGenericProp.PropertyType == typeof(int) && matchingGenericProp.Name
        //                    != "SearchByActivityStatus") && prop.GetValue(filter) != null)
        //                {
        //                    countQuery.Append($" AND {columnName} = @{prop.Name} ");
        //                    query.Append($" AND {columnName} = @{prop.Name} ");
        //                } 

        //                if(matchingGenericProp.PropertyType == typeof(string) && prop.GetValue(filter) != null)
        //                {
        //                    countQuery.Append(@$" AND {columnName} LIKE '%' + @{prop.Name} + '%' ");
        //                    query.Append(@$" AND {columnName} LIKE '%' + @{prop.Name} + '%' ");
        //                }

        //                if(matchingGenericProp.PropertyType == typeof(bool) && prop.GetValue(filter) != null)
        //                {
        //                    countQuery.Append($" AND {columnName} = @{prop.Name} ");
        //                    query.Append($" AND {columnName} = @{prop.Name} ");
        //                }

        //            }
        //        }
        //    }
        //    query.Append($" ORDER BY {keyColumn} DESC");
        //    query.Append(@" OFFSET @Offset ROWS ");
        //    query.Append(@" FETCH NEXT @PageSize ROWS ONLY");
        //    query.Append(countQuery);

        //    return query;

        //}


        #region PRIVATE METHODS

        private IEnumerable<PropertyInfo> GetFilterProperties(bool excludeKey = false)
        {
            var properties = typeof(Filter).GetProperties()
                 .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }



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

        protected virtual StringBuilder PaginateQueryBuilder(Paging paging, Filter? filter, string tableName, string keyColumn, string keyProperty)
        {
            StringBuilder countQuery = new($"SELECT COUNT (DISTINCT {keyColumn}) FROM {tableName} WHERE 1 = 1 ");

            StringBuilder query = new($"SELECT * FROM {tableName} WHERE 1 = 1 ");

            var filterProperties = GetFilterProperties();
            var genericProperties = GetProperties();

            foreach(var prop in filterProperties)
            {
                var columntAttr = prop.GetCustomAttribute<ColumnAttribute>();

                string propName = prop.Name;
                string actualPropName = prop.Name.Substring("SearchBy".Length);


                if (prop.Name.StartsWith("SearchBy"))
                {
                    
                    var matchingGenericProp = 
                        genericProperties.FirstOrDefault(p => 
                        p.Name.Equals(actualPropName, StringComparison.OrdinalIgnoreCase));
                    

                    if (prop.Name.Equals("SearchByActivityStatus", StringComparison.OrdinalIgnoreCase)  
                        && prop.GetValue(filter) != null)
                    {
                        countQuery.Append($" AND IsActive = @{prop.Name} ");
                        query.Append($" AND IsActive = @{prop.Name} ");
                    }

                    if (matchingGenericProp != null && prop.GetValue(filter) != null)
                    {
                        string columnName = columntAttr?.Name ?? matchingGenericProp.Name;

                         if ((matchingGenericProp.PropertyType == typeof(int) && matchingGenericProp.Name
                            != "SearchByActivitiyStatus") && prop.GetValue(filter) != null)
                        {
                            countQuery.Append($" AND {columnName} = {prop.Name} ");
                            query.Append($" AND {columnName} = {prop.Name} ");
                        }

                        if (matchingGenericProp.PropertyType == typeof(string) && prop.GetValue(filter) != null)
                        {
                            countQuery.Append(@$" AND {columnName} LIKE '%' + @{prop.Name} + '%' ");
                            query.Append(@$" AND {columnName} LIKE '%' + @{prop.Name} + '%' ");
                        }

                        if (matchingGenericProp.PropertyType == typeof(bool) && prop.GetValue(filter) != null)
                        {
                            countQuery.Append($" AND {columnName} = @{prop.Name} ");
                            query.Append($" AND {columnName} = @{prop.Name} ");
                        }

                        if (matchingGenericProp.PropertyType == typeof(DateTime) && prop.GetValue(filter) != null)
                        {
                            countQuery.Append($" AND {columnName} = @{prop.Name} ");
                            query.Append($" AND {columnName} = @{prop.Name} ");
                        } 
                    }

                }
             
            }
            query.Append($" ORDER BY {keyColumn} DESC ");
            query.Append(@" OFFSET @Offset ROWS ");
            query.Append(@" FETCH NEXT @PageSize ROWS ONLY; ");

            query.Append(countQuery);


            return query;

        }




        protected virtual StringBuilder BuildSoftDeleteQuery(string tableName, int id)
        {
            StringBuilder query = new();

            query.Append($"UPDATE {tableName} SET IsActive = 0 WHERE Id = @id;");

            return query;
        }

        protected virtual async Task<int> BuildUpdateCommand(string query, System.Data.IDbConnection connection, DynamicParameters parameters)
        {
            return await connection.ExecuteAsync(query, parameters);
        }

        protected virtual StringBuilder BuildUpdateQuery(string tableName, string keyColumn, string keyProperty, int id)
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


        protected virtual async Task<List<T>> BuildPaginationCommand(StringBuilder query, GridReader multipleQuery)
        {
            var entities = await multipleQuery.ReadAsync<T>();

            return entities.ToList();
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
