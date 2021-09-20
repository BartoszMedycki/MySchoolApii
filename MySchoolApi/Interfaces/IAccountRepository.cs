using Microsoft.AspNetCore.Identity;
using MySchoolApiDataBase.DataModels.InDataModels;

namespace MySchoolApiDataBase.Entities
{
    public interface IAccountRepository
    {

        string GenerateJwtForStudent(LoginDto loginDto);
        string GenerateJwtForEmployee(LoginDto loginDto);
    }
}