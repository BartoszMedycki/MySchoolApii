using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MySchoolApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class EmployeeController : ControllerBase
    {
        

        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        

        public EmployeeController(ILogger<EmployeeController> logger, MySchoolApiDbContext dbContext
          ,IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            DbContext = dbContext;
            this._employeeRepository = employeeRepository;
            
        }

        public MySchoolApiDbContext DbContext { get; }


        [HttpPost]
       //[Authorize(Roles ="Director,Admin")]
        public void AddEmployee([FromBody] CreateEmployeeDataModel employeeDataModel)
        {
            _employeeRepository.AddEmployee(employeeDataModel);
        }

        [HttpGet]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult<IEnumerable<Employee>> GetEmployees([FromQuery]ContextQuery query)
        {
            var AllEmployees = _employeeRepository.GetAllEmployees(query);
            return Ok(AllEmployees);
        }

        [HttpGet("{name}/{surename}")]
      
        public ActionResult<Employee> GetEmployeeByNameAndSurename([FromRoute] string name, [FromRoute] string surename)
        {
            var employee = _employeeRepository.GetEmployeeByNameAndSurename(name, surename);
            return Ok(employee);
        }

        [HttpGet("{uniqueId}")]
       
        public ActionResult<Employee> GetEmployeeById([FromRoute] int uniqueId)
        {
            var employee = _employeeRepository.GetEmployeeByUniqueCode(uniqueId);
            return Ok(employee);
        }
        [HttpPatch("{uniqueId}")]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult UpdateEmployee([FromBody] UpdateEmployeeDataModel1 employeeDataModel, [FromRoute] int uniqueId)
        {
            _employeeRepository.UpdateEmployee(employeeDataModel, uniqueId);
            return Ok();
        }
    }
}
