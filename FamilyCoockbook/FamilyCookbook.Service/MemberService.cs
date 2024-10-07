using AngleSharp.Css.Dom;
using BCrypt.Net;
using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Respository.Common;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public sealed class MemberService : AbstractService<Member>, IMemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IConfiguration _configuration;

        public MemberService(IMemberRepository repository, 
            IRecipeRepository recipeRepository, IConfiguration configuration) : base(repository) 
        {
            _repository = repository;
            _recipeRepository = recipeRepository;
            _configuration = configuration;
        }

        public async Task<MessageResponse> CreateAsync(Member entity)
        {
            entity.UniqueId = Guid.NewGuid();
            entity.IsActive = true;
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password, 12);

            entity.Password = passwordHash;

            var response = await _repository.CreateAsync(entity);

            return response;
        }

        public async Task<RepositoryResponse<Member>> SoftDeleteAsync(int id)
        {
            var response = new RepositoryResponse<Member>();
            StringBuilder errorBuilder = new StringBuilder();

            var recipeResponse = await _recipeRepository.GetAllAsync();

            if (recipeResponse.Success == false || recipeResponse == null)
            {          
                response.Success = false;
                response.Message = errorBuilder.Append("Error parsing recipes! " + recipeResponse.Message); ;
                return response;
            }

            bool chk = recipeResponse.Items.Any(r => r.Members.Any(m => m.Id == id));
    
            if (chk)
            {
                response.Success = false;
                response.Message = errorBuilder.Append("The member you are trying to delete is an author of active recipes." +
                    "Deleting him before the recipes can cause issues in the program. Please delete his " +
                    "active recipes first!");
                return response;
            }

            response = await _repository.SoftDeleteAsync(id);

            return response;

        }

        public async Task<MessageResponse> UpdateAsync(int id, Member entity)
        {
            var chkMember = await _repository.GetByIdAsync(id);
            var response = new MessageResponse();

            entity.DateUpdated = DateTime.Now;

            if (chkMember.Success == false) 
            {
                response.IsSuccess = false;
                response.Message = chkMember.Message; 
                return response;
            }
            if (chkMember.Items.Password != entity.Password)
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password, 12);
                entity.Password = passwordHash;

            }

            response = await _repository.UpdateAsync(id, entity);

            return response;

        }
        public async Task<RepositoryResponse<Member>> GetByGuidAsync(Guid uniqueId)
        {
            var response = await _repository.GetByGuidAsync(uniqueId);

            return response;
        }

        public async Task<RepositoryResponse<Lazy<List<Member>>>> PaginateAsync(Paging paging, MemberFilter filter)
        {
            var response = await _repository.PaginateAsync(paging, filter);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;
        }

        public async Task<RepositoryResponse<List<Member>>> SearchMemberByCondition(string condition)
        {
            StringBuilder errorBuilder = new StringBuilder();


            var response = await _repository.GetAllAsync();

            var members = response.Items.Where(member => (member.FirstName.ToLower().Contains(condition.ToLower()) ||
                member.LastName.ToLower().Contains(condition.ToLower()))).ToList();

            response.Items = members;

            if (response.Items.Count < 1)
            {
                response.Success = false;
                response.Message = errorBuilder.Append("No member with condition " + condition + " found");
                return response;
            }

            return response;
        }

        public async Task<IActionResult> LogIn(string username, string password)
        {
            StringBuilder errorBuilder = new StringBuilder();
            var response = await _repository.GetAllAsync();

            var validateUser = response.Items
                .Where(member => member.Username!.EndsWith(username))
                .FirstOrDefault();

            if (validateUser == null) 
            {
                return new BadRequestObjectResult(errorBuilder.Append("Invalid username!"));
            }

            if (!BCrypt.Net.BCrypt.Verify(password, validateUser.Password)) 
            {
                return new BadRequestObjectResult(errorBuilder.Append("Invalid password!"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {new Claim("Id", validateUser.Username),
                new Claim(ClaimTypes.Role, validateUser.Role.Name)}),

                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new OkObjectResult(jwt);
        }

        public async Task<RepositoryResponse<Lazy<Member>>> GeByUsernameAsync(string username)
        {
            var response = await _repository.FindByUsernameAsync(username);

            return response;
        }
    }
}
