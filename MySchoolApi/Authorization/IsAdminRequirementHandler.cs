using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using MySchoolApiDataBase.Entities;

namespace MySchoolApi
{
    public class IsAdminRequirementHandler<T> : IAuthorizationHandler where T: IAuthorizationRequirement
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            if (role == RolesNames.Admin.ToString())
            {
                foreach (var req in context.Requirements.OfType<T>())
                {
                    context.Succeed(req);
                }
            }
            return Task.CompletedTask;
        }

        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        //{
        //    var contextRole = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
        //    if (contextRole == "Admin")
        //    {
        //        context.Succeed(requirement);
        //    }
        //    return Task.CompletedTask;
        //}
    }


}
