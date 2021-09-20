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
    public class IsDirectorRequirementHandler<T> : IAuthorizationHandler where T : IAuthorizationRequirement
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            if (Role == RolesNames.Director.ToString())
            {
                
                var req = context.Requirements.OfType<T>().FirstOrDefault();
                context.Succeed(req);
                
                   
                
            }
          
            return Task.CompletedTask;
            
        }
        

        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDirectorRequirement requirement)
        //{
        //    var contextRole = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
        //    if (contextRole == "Director")
        //    {
        //        context.Succeed(requirement);
        //    }
        //    return Task.CompletedTask;
        //}
    }


}
