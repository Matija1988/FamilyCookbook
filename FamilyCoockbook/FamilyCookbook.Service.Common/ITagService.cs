﻿using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface ITagService
    {
        Task<RepositoryResponse<List<Tag>>> GetAllAsync();

        Task<RepositoryResponse<Tag>> CreateAsync(List<Tag> entities);
    }
}