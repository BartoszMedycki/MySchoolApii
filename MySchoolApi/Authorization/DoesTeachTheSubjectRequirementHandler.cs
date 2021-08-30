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
    public class DoesTeachTheSubjectRequirementHandler : AuthorizationHandler<DoesTeachTheSubjectRequirement, Employee>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoesTeachTheSubjectRequirement requirement, Employee employee)
        {
            var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            if (Role == "Director" && Role == "Admin")
            {
                context.Succeed(requirement);
            }
            if (Role == "Teacher")
            {
                var userId = int.Parse(context.User.FindFirst(prop => prop.Type == ClaimTypes.NameIdentifier).Value);
                if (userId == employee.Id)
                {
                    context.Succeed(requirement);
                }
            }
            else context.Fail();

           return Task.CompletedTask;
        }
    }


}
