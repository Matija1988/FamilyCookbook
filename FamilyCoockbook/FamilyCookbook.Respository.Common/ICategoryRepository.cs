﻿using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<MessageResponse> CreateAsync(Category entity);


        Task<MessageResponse> UpdateAsync(int id, Category entity);

    }
}
