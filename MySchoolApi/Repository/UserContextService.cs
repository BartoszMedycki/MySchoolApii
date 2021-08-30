
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySchoolApi;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class UserContextService : IUserContextService

    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal Claims => httpContextAccessor.HttpContext.User;
        public int GetUserId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst(prop => prop.Type == ClaimTypes.NameIdentifier).Value);


    }
}