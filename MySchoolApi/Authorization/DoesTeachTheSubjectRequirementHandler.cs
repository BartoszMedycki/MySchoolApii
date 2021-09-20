using Microsoft.AspNetCore.Authorization;
using MySchoolApiDataBase.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public class DoesTeachTheSubjectRequirementHandler : AuthorizationHandler<DoesTeachTheSubjectRequirement, Employee>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoesTeachTheSubjectRequirement requirement, Employee employee)
        {
            var Role = context.User.FindFirst(prop => prop.Type == ClaimTypes.Role).Value;
            if (Role == "Teacher")
            {
                var userId = int.Parse(context.User.FindFirst(prop => prop.Type == ClaimTypes.NameIdentifier).Value);

                if (userId == employee.Id)
                    context.Succeed(requirement);
            }
           

           return Task.CompletedTask;
        }
    }


}
