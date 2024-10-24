﻿using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class CommentService : AbstractService<Comment, CommentFilter>, ICommentService
    {
        private  readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository) : base(commentRepository) 
        {
            _commentRepository = commentRepository;
        }

        public async Task<MessageResponse> CreateAsync(Comment comment)
        {
            comment.IsActive = true;
            comment.DateCreated = DateTime.Now;
            comment.DateUpdated = DateTime.Now;

            var response = await _commentRepository.CreateAsync(comment);

            return response;
        }

        public new async Task<MessageResponse> UpdateAsync(int id, Comment comment)
        {
            comment.DateUpdated = DateTime.Now;

            var response = await _commentRepository.UpdateAsync(id, comment);

            return response;    

        }

        public async Task<RepositoryResponse<List<Comment>>> GetRecipeCommentsAsync(int recipeId)
        {
            var response = await _commentRepository.GetRecipeCommentsAsync(recipeId);

            return response;
        }


    }
}
