using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class EmployeeRepository : MainRepository<Employee>, IEmployeeRepository
    {
        private readonly CreateEmployeeMapper _createEmployeeMapper;
        private readonly EmployeeMapper _employeeMapper;
        private readonly ILogger<EmployeeRepository> logger;
        private readonly IPasswordHasher<Employee> passwordHasher;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public EmployeeRepository(IServiceProvider service, CreateEmployeeMapper createEmployeeMapper,
            EmployeeMapper employeeMapper, ILogger<EmployeeRepository> logger, IPasswordHasher<Employee> passwordHasher
            ,IAuthorizationService authorizationService, IUserContextService userContextService) : base(service)
        {
            _createEmployeeMapper = createEmployeeMapper;
            _employeeMapper = employeeMapper;
            this.logger = logger;
            this.passwordHasher = passwordHasher;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        protected override DbSet<Employee> dbSet => dbContext.Employees;

        public void AddEmployee(CreateEmployeeDataModel employeeDataModel)

        {
            logger.LogInformation($"AddEmployee method invoked || Employee: {employeeDataModel.Name}  {employeeDataModel.SureName}");
            if (dbContext.Database.CanConnect())
            {
                if (!dbContext.Employees.Any(uniqueId => uniqueId.UniqueNumber == employeeDataModel.UniqueNumber))
                {
                    var employeeRole = dbContext.Roles.FirstOrDefault(role => role.RoleName == employeeDataModel.RoleName);
                    var mappedEmployee = _createEmployeeMapper.Map(employeeDataModel);
                    mappedEmployee.Role = employeeRole;

                    

                    var hashedPassword = passwordHasher.HashPassword(mappedEmployee, employeeDataModel.Password);
                    mappedEmployee.User.PasswordHash = hashedPassword;
                    mappedEmployee.User.Email = employeeDataModel.Email;

                    dbContext.Employees.Add(mappedEmployee);
                    this.SaveChanges();
                    logger.LogInformation($"Employee {employeeDataModel.Name}  {employeeDataModel.SureName} Added");

                }
            }
        }
        public IEnumerable<EmployeeDataModel> GetAllEmployees()
        {
            List<EmployeeDataModel> listOfMappedEmployees = new List<EmployeeDataModel>();
            var allEmployees = dbContext.Employees.Include(role => role.Role);
            if (allEmployees != null)
            {
                foreach (var employee in allEmployees)
                {
                    listOfMappedEmployees.Add(_employeeMapper.Map(employee));
                }
                return listOfMappedEmployees;
            }
            else throw new NotFoundException("Employees not found");
        }
        public EmployeeDataModel GetEmployeeByUniqueCode(int code)
        {
            var employeeByUniqueCode = dbContext.Employees.Include(role => role.Role).FirstOrDefault(param => param.UniqueNumber == code);
            if (employeeByUniqueCode != null)
            {
               var authResult = authorizationService.AuthorizeAsync(userContextService.Claims, employeeByUniqueCode, new EmployeeIsOwnerRequirement()).Result;
                if (!authResult.Succeeded)
                {
                    throw new NotAuthorizeException("Unauthorize request");
                }
                var mappedEmployee = _employeeMapper.Map(employeeByUniqueCode);
                return mappedEmployee;
            }
            else throw new NotFoundException("Employee not found");
        }
        public void UpdateEmployee(UpdateEmployeeDataModel1 employeeDataModel,int uniqueCode)
        {
            logger.LogInformation($"UpdateEmployee method invoked");
            var employeeByUniqueNumber = dbContext.Employees.Include(role => role.Role)
                                        .FirstOrDefault(find=>find.UniqueNumber == uniqueCode);
            if (employeeByUniqueNumber != null)
            {
                if (employeeDataModel.Name != null)
                {
                    employeeByUniqueNumber.Name = employeeDataModel.Name;
                }
                if (employeeDataModel.SureName != null)
                {
                    employeeByUniqueNumber.SureName = employeeDataModel.SureName;
                }
                if (employeeDataModel.ContactTelephoneNumber != 0)
                {
                    employeeByUniqueNumber.ContactTelephoneNumber = employeeDataModel.ContactTelephoneNumber;
                }
                if (employeeDataModel.RoleName != null)
                {
                    var role = dbContext.Roles.FirstOrDefault(cfg => cfg.RoleName == employeeDataModel.RoleName);
                    employeeByUniqueNumber.Role = role;
                }

                this.SaveChanges();
                logger.LogInformation($"Employee with uniqueCode {uniqueCode} updated");
            }
            else throw new NotFoundException("Employee not found");
        }
        public EmployeeDataModel GetEmployeeByNameAndSurename(string name, string sureName)
        {
            var employeeByUniqueCode = dbContext.Employees.Include(role => role.Role)
                .FirstOrDefault(param => param.Name == name && param.SureName == sureName);
            if (employeeByUniqueCode != null)
            {
                var authResult = authorizationService.AuthorizeAsync(userContextService.Claims, employeeByUniqueCode, new EmployeeIsOwnerRequirement()).Result;
                if (!authResult.Succeeded)
                {
                    throw new NotAuthorizeException("Unauthorize request");
                }
                var mappedEmployee = _employeeMapper.Map(employeeByUniqueCode);
                return mappedEmployee;
            }
            else throw new NotFoundException("Employee not found");
        }
        public void Seed()
        {
            var startEmployees = GetStartEmployees();
            if (!dbContext.Employees.Any())
            {
                if (startEmployees != null)
                {
                    foreach (var employee in startEmployees)
                    {
                        dbContext.Employees.Add(employee);
                        this.SaveChanges();
                    }
                }
            }
        }

        public IEnumerable<Employee> GetStartEmployees()
        {
            List<Employee> listOfStartEmployees = new List<Employee>()
        {
                new Employee()
                {
                Name = "Jolanta",
                SureName = "Medycka",
                User = new User()
                {
                Email = "barmed553@wp.pl",
                PasswordHash = "admin123"
                }
                ,
                ContactTelephoneNumber = 791329986,
                UniqueNumber = 9997,
                Role = dbContext.Roles.Where(role=>role.RoleName=="Teacher").FirstOrDefault()

                },
                new Employee()
                {
                Name = "Ewa",
                SureName = "Pietrzyk",
                 User = new User()
                {
                Email = "barmed553@wp.pl",
                PasswordHash = "admin123"
                },
                ContactTelephoneNumber = 791320123,
                UniqueNumber = 9998,
                Role = dbContext.Roles.Where(role=>role.RoleName=="Teacher").FirstOrDefault()

                },
                new Employee()
                {
                Name = "Kornelia",
                SureName = "Wieszak",
               User = new User()
                {
                Email = "barmed553@wp.pl",
                PasswordHash = "admin123"
                },
                ContactTelephoneNumber = 791111333,
                UniqueNumber = 9999,
                Role = dbContext.Roles.Where(role=>role.RoleName=="Director").FirstOrDefault()

                }
        };
            return listOfStartEmployees;
        }
    }
}
