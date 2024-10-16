﻿using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface ICommentService : IService<Comment>
    {
        Task<MessageResponse> CreateAsync(Comment comment);

        Task<RepositoryResponse<List<Comment>>> GetRecipeCommentsAsync(int recipeId);
    }
}
