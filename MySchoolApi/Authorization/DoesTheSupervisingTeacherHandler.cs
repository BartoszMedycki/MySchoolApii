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
    public class DoesTheSupervisingTeacherRequirementHandler<T> : IAuthorizationHandler where T : IAuthorizationRequirement
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            
            if (Role == RolesNames.Teacher.ToString())
            {
                foreach (var req in context.Requirements.OfType<T>())
                {
                    context.Succeed(req);
                }
                
            }
            return Task.CompletedTask;
        }

        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoesTheSupervisingTeacherRequirement requirement, Class employee)
        //{
        //    var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
           
        //    if (Role == "Teacher")
        //    {
        //        var id = int.Parse(context.User.FindFirst(prop => prop.Type == ClaimTypes.NameIdentifier).Value);
        //        if (id == employee.Id)
        //        {
        //            context.Succeed(requirement);
        //        }
             
        //    }
           
        //    return Task.CompletedTask;
        //}

    }


}
