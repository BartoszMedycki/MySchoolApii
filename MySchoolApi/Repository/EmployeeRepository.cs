using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class EmployeeRepository : MainRepository<Employee>, IEmployeeRepository
    {
     
        private readonly EmployeeMapper _employeeMapper;
        private readonly ILogger<EmployeeRepository> logger;
        private readonly IPasswordHasher<Employee> passwordHasher;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public EmployeeRepository(IServiceProvider service, 
            EmployeeMapper employeeMapper, ILogger<EmployeeRepository> logger, IPasswordHasher<Employee> passwordHasher
            ,IAuthorizationService authorizationService, IUserContextService userContextService) : base(service)
        {
           
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
                    var mappedEmployee = _employeeMapper.InDataMap(employeeDataModel);
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
        public IEnumerable<EmployeeDataModel> GetAllEmployees(ContextQuery query)
        {
            List<EmployeeDataModel> listOfMappedEmployees = new List<EmployeeDataModel>();
            var allEmployees = dbContext.Employees.Include(role => role.Role).Where(prop=>prop.Id != 0);
            if (allEmployees != null)
            {
                if (string.IsNullOrEmpty(query.SortBy))
                {
                    allEmployees = allEmployees.OrderBy(prop => prop.Name);
                }
                else if(!string.IsNullOrEmpty(query.SortBy))
                {

                    var columnSelector = new Dictionary<string, Expression<Func<Employee, object>>>
               {
                    {nameof(Employee.Name), prop=>prop.Name },
                    {nameof(Employee.SureName), prop=>prop.SureName },
                    {nameof(Employee.Id), prop=>prop.Id },
                    {nameof(Employee.Role), prop=>prop.Role },

                };
                    var column = columnSelector[query.SortBy];
                    allEmployees = query.SortDirection == SortDirection.ASC ? allEmployees.OrderBy(column) : allEmployees.OrderByDescending(column);
                }
                

                foreach (var employee in allEmployees)
                {
                    listOfMappedEmployees.Add(_employeeMapper.OutDataMap(employee));
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
                var mappedEmployee = _employeeMapper.OutDataMap(employeeByUniqueCode);
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
                
                    employeeByUniqueNumber.Name = employeeDataModel.Name;
                    employeeByUniqueNumber.SureName = employeeDataModel.SureName;
                    employeeByUniqueNumber.ContactTelephoneNumber = employeeDataModel.ContactTelephoneNumber;
                    var role = dbContext.Roles.FirstOrDefault(cfg => cfg.RoleName == employeeDataModel.RoleName);
                    employeeByUniqueNumber.Role = role;

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
                var mappedEmployee = _employeeMapper.OutDataMap(employeeByUniqueCode);
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
                        employee.User.PasswordHash = passwordHasher.HashPassword(employee, employee.User.PasswordHash);
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
                Email = "barmed55322@wp.pl",
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
                Email = "barmed55342@wp.pl",
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
                Email = "barmediko@wp.pl",
                
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
