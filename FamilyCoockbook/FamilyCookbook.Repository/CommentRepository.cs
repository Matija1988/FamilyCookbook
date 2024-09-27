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
    public sealed class CommentRepository(DapperDBContext context, 
        IErrorMessages errorMessages, 
        ISuccessResponses successResponses) 
        : AbstractRepository<Comment>(context, errorMessages, successResponses), ICommentRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new();

            return query.Append("SELECT a.*, b.Id, b.FirstName, b.LastName FROM Comment a JOIN " +
                "Member b on b.Id = a.MemberId ORDER BY a.DateCreated DESC;");
        }

        protected override StringBuilder BuildQueryReadSingle(int id)
        {
            StringBuilder query = new();

            return query.Append("SELECT a.*, b.Id, b.FirstName, b.LastName FROM Comment a JOIN " +
                $"Member b on b.Id = a.MemberId WHERE a.Id = {id}");
        }

        protected override async Task<List<Comment>> BuildQueryCommand(string query, IDbConnection connection)
        {
            var entityDictionary = new Dictionary<int, Comment>();

            using var multipleQuery = await connection.QueryMultipleAsync(query);

            IEnumerable<Comment> member = multipleQuery.Read<Comment, Member,Comment>((comment, member) =>
            {
                if (!entityDictionary.TryGetValue(comment.Id, out var existingEntity))
                {
                    existingEntity = comment;
                    existingEntity.Member = member;
                    entityDictionary.Add(existingEntity.Id, existingEntity);
                }

                return existingEntity;
            }, splitOn: "Id");

            return member.ToList();
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
    }
}












