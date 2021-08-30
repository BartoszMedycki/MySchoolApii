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
    public class EmployeeIsOwnerRequirementHandler : AuthorizationHandler<EmployeeIsOwnerRequirement, Employee>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeIsOwnerRequirement requirement, Employee student)
        {
            var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            if (Role == "Director" && Role == "Admin")
            {
                context.Succeed(requirement);
            }
            else if (Role == "Teacher")
            {
                var id = int.Parse(context.User.FindFirst(prop => prop.Type == ClaimTypes.NameIdentifier).Value);
                if (id == student.Id)
                {
                    context.Succeed(requirement);
                }
                else context.Fail();
            }
            else context.Fail();
            return Task.CompletedTask;
        }

    }


}
