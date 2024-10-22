using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public sealed class CommentRepository : AbstractRepository<Comment>, ICommentRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        public CommentRepository(DapperDBContext context, 
            IErrorMessages errorMessages, 
            ISuccessResponses successResponses) : base(context, errorMessages, successResponses)
        {
            _context = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }




        #region OVEERIDE UPDATE

        protected override StringBuilder BuildUpdateQuery(string tableName, string keyColumn, string keyProperty, int id)
        {
            StringBuilder query = new();

            query.Append("UPDATE Comment SET MemberId = @MemberId, RecipeId = @RecipeId, Text = @Text," +
                " Rating = @Rating, DateUpdated = @DateUpdated WHERE Id = @id");
            return query;
        }

        #endregion

        #region OVERRIDE READ ALL

        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new();

            return query.Append("SELECT a.*, b.Id, b.FirstName, b.LastName FROM Comment a JOIN " +
                "Member b on b.Id = a.MemberId WHERE a.IsActive = 1 ORDER BY a.DateCreated DESC;");
        }

        protected override async Task<List<Comment>> BuildQueryCommand(string query, IDbConnection connection)
        {
            var entityDictionary = new Dictionary<int, Comment>();

            using var multipleQuery = await connection.QueryMultipleAsync(query);

            IEnumerable<Comment> comments = multipleQuery.Read<Comment, Member, Comment>((comment, member) =>
            {
                if (!entityDictionary.TryGetValue(comment.Id, out var existingEntity))
                {
                    existingEntity = comment;
                    existingEntity.Member = member;
                    entityDictionary.Add(existingEntity.Id, existingEntity);
                }

                return existingEntity;
            }, splitOn: "Id");

            return comments.ToList();
        }


        #endregion

        #region OVERRIDE GETbyID

        protected override StringBuilder BuildQueryReadSingle(int id)
        {
            StringBuilder query = new("SELECT a.*, b.Id, b.FirstName, b.LastName FROM Comment a JOIN ");

            return query.Append($"Member b on b.Id = a.MemberId WHERE a.Id = {id}");
        }


        protected override async Task<Comment> BuildQueryCommand(string query, IDbConnection connection, int id)
        {
            var entityDictionary = new Dictionary<int, Comment>();

            var entity = await connection.QueryAsync<Comment, Member, Comment>(query,
                (comment, member) =>
                {
                    if(!entityDictionary.TryGetValue(comment.Id, out var exitingEntity))
                    {
                        exitingEntity = comment;
                        exitingEntity.Member = member;
                        entityDictionary.Add(comment.Id, exitingEntity);
                    }

                    return exitingEntity;
                }, new {id});

            return entityDictionary.Values.FirstOrDefault();

        }
#endregion

        #region CREATE OVERRIDE

        protected override StringBuilder BuildCreateQuery(string tableName, string columns, string properties)
        {
            StringBuilder query = new();

            query.Append("INSERT INTO Comment (MemberId, RecipeId, Text, DateCreated, DateUpdated, Rating, IsActive)");
            query.Append(" VALUES (@MemberId, @RecipeId, @Text, @DateCreated, @DateUpdated, @Rating, @IsActive)");

            return query;
        }

        protected override async Task<int> BuildCreateQueryCommand(string query, IDbConnection connection, Comment entity)
        {
            var parameters = new
            {
                MemberId = entity.MemberId,
                RecipeId = entity.RecipeId,
                Text = entity.Text,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated,
                Rating = entity.Rating,
                IsActive = entity.IsActive,
            };

            int rowsAffected = await connection.ExecuteAsync(query, parameters);

            return rowsAffected;
        }
        #endregion

        #region UniqueMethods

        public async Task<RepositoryResponse<List<Comment>>> GetRecipeCommentsAsync(int id)
        {
            var response = new RepositoryResponse<List<Comment>>();

            try
            {
                StringBuilder query = new("SELECT a.*, b.Id, b.FirstName, b.LastName");
                query.Append(" FROM Comment a ");
                query.Append(" JOIN Member b on b.Id = a.MemberId ");
                query.Append($" WHERE a.RecipeId = {id} ");
                query.Append($" AND a.IsActive = 1 "); 
                query.Append($" ORDER BY a.DateCreated DESC");

                var entityDictionary = new Dictionary<int, Comment>();

                using var connection = _context.CreateConnection();

                IEnumerable<Comment> comments = await connection
                    .QueryAsync<Comment, Member, Comment>(query.ToString(), (comment, member) =>
                    {
                        if(!entityDictionary.TryGetValue(comment.Id, out var exitingEntity))
                        {
                            exitingEntity = comment;
                            exitingEntity.Member = member;
                            entityDictionary.Add(comment.Id, exitingEntity);
                        }

                        return exitingEntity;
                    }, new {id}, splitOn: "Id");
                   
                response.Success = true;
                response.Items = comments.ToList();

                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = _errorMessages.ErrorAccessingDb("Comment", ex);
                return response;
            }
            finally
            {
                _context.CreateConnection().Close();
            }

        }


        #endregion
    }
}












