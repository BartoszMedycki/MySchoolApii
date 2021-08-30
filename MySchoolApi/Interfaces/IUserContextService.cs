using System.Security.Claims;

namespace MySchoolApiDataBase.Entities
{
    public interface IUserContextService
    {
        ClaimsPrincipal Claims { get; }
        int GetUserId { get; }
    }
}