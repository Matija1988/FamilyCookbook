﻿using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface ISearchRepository
    {
        Task<RepositoryResponse<List<Recipe>>> GetAllBySearchText(string searchText);
    }
}
