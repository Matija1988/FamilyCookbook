﻿using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace FamilyCookbook.Service.Common
{
    public interface IMemberService : IService<Member, MemberFilter>
    {
        Task<MessageResponse> CreateAsync(Member entity);
        Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId);
        Task<IActionResult> LogIn(string username, string password);

        Task<MessageResponse> UpdateAsync(int id, Member entity);
        Task<RepositoryResponse<Lazy<List<Member>>>> PaginateAsync(Paging paging, MemberFilter filter);

        Task<RepositoryResponse<List<Member>>> SearchMemberByCondition(string condition);

        Task<RepositoryResponse<Lazy<Member>>> GeByUsernameAsync(string username);

        Task<MessageResponse> UserRegister(UserRegistry user);
    }
}
