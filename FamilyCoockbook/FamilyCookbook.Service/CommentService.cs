﻿using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class CommentService(ICommentRepository commentRepository) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;

        public async Task<RepositoryResponse<Comment>> CreateAsync(Comment comment)
        {
            comment.IsActive = true;
            comment.DateCreated = DateTime.Now;
            comment.DateUpdated = DateTime.Now;

            var response = await _commentRepository.CreateAsync(comment);

            return response;
        }

        public Task<RepositoryResponse<Comment>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<List<Comment>>> GetAllAsync()
        {
            var response = await _commentRepository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Comment>> GetByIdAsync(int id)
        {
            var response = await _commentRepository.GetByIdAsync(id);

            return response;
        }

        public Task<RepositoryResponse<Comment>> SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<Comment>> UpdateAsync(int id, Comment entity)
        {
            entity.DateUpdated = DateTime.Now;

            var resposne = await _commentRepository.UpdateAsync(id, entity);

            return resposne;
        }
    }
}
