
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
    public class AccountRepository : IAccountRepository

    {
        private readonly MySchoolApiDbContext dbContext;
        private readonly AuthenticationSettings authenticationSettings;

        public IPasswordHasher<Student> PasswordHasherForStudent { get; }
        public IPasswordHasher<Employee> PasswordHasherForEmployee { get; }

      

        public AccountRepository(MySchoolApiDbContext dbContext, IPasswordHasher<Student> studentPasswordHasher,
            IPasswordHasher<Employee> employeePasswordHasher,
            AuthenticationSettings authenticationSettings)
        {
            this.dbContext = dbContext;
         
            this.authenticationSettings = authenticationSettings;
            this.PasswordHasherForStudent = studentPasswordHasher;
            this.PasswordHasherForEmployee = employeePasswordHasher;
        }



        public string GenerateJwtForStudent(LoginDto loginDto)
        {
            var user = dbContext.Users.FirstOrDefault(prop => prop.Email == loginDto.Email);
            var studentByEmail = dbContext.Students.Include(prop=>prop.User).Include(prop => prop.Role).Include(prop => prop.Class)
                .ThenInclude(prop => prop.SupervisingTeacher).Include(prop=>prop.SchoolSubjects).FirstOrDefault(prop => prop.User.Email == user.Email);
            if (studentByEmail is null)
            {
                throw new BadEmailOrPasswordException("Incorect Email or Password");
            }

            var verifyPassword = PasswordHasherForStudent.VerifyHashedPassword(studentByEmail, studentByEmail.User.PasswordHash, loginDto.Password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                throw new BadEmailOrPasswordException("Incorect Email or Password");
            }


            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.NameIdentifier,studentByEmail.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{studentByEmail.Name} {studentByEmail.Surename}"),
            new Claim(ClaimTypes.Role, $"{studentByEmail.Role.RoleName}"),
            new Claim("ClassName", studentByEmail.Class.ClassName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHendler = new JwtSecurityTokenHandler();
            return tokenHendler.WriteToken(token);
        }  

        public string GenerateJwtForEmployee(LoginDto loginDto)
        {
            var employeeByEmail = dbContext.Employees.Include(prop => prop.Role).FirstOrDefault(prop => prop.User.Email == loginDto.Email);
                
            if (employeeByEmail is null)
            {
                throw new BadEmailOrPasswordException("Incorect Email or Password");
            }

            var verifyPassword = PasswordHasherForEmployee.VerifyHashedPassword(employeeByEmail, employeeByEmail.User.PasswordHash, loginDto.Password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                throw new BadEmailOrPasswordException("Incorect Email or Password");
            }


            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.NameIdentifier,employeeByEmail.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{employeeByEmail.Name} {employeeByEmail.SureName}"),
            new Claim(ClaimTypes.Role, $"{employeeByEmail.Role.RoleName}"),
      
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHendler = new JwtSecurityTokenHandler();
            return tokenHendler.WriteToken(token);
        }
    }
}