using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.DataModels.OutDataModels.StudentDataModel;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MySchoolApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
   // [Authorize]
    public class StudentController : ControllerBase
    {


        private readonly ILogger<EmployeeController> _logger;
        private readonly IStudentRepository _studentRepository;
     
       
        public MySchoolApiDbContext DbContext { get; }


        public StudentController(ILogger<EmployeeController> logger, MySchoolApiDbContext dbContext
          , IStudentRepository studentRepository)
        {
            _logger = logger;
            DbContext = dbContext;
            _studentRepository = studentRepository;
           


        }
        [HttpGet]
        //[Authorize(Roles = "Director,Admin")]
        public ActionResult<IEnumerable<StudentDataModel1>> getAllStudents([FromQuery]ContextQuery query)
        {
            var result =_studentRepository.getAllStudents(query);
            return Ok(result);
        }  
        
        [HttpGet("{name}/{surename}")]
        [Authorize(Roles = "Director,Admin,Student")]
        public ActionResult<IEnumerable<StudentDataModel1>> getStudentByNameAndSureName([FromRoute]string name,[FromRoute]string surename)
        {
            var a = User.Claims.ToList();
            var result = _studentRepository.GetStudentByNameAndSureName(name, surename);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Roles = "Director,Admin")]
        public  ActionResult DeleteStudent([FromHeader] string pesel)
        {
            _studentRepository.DeleteStudent(pesel);
            return Ok();
        }
        [HttpPost("{className}")]
       // [Authorize(Roles = "Director,Admin")]
        public ActionResult AddStudent([FromBody] CreateStudentDataModel studentDataModel, [FromRoute] string className)
        {
            _studentRepository.AddStudent(className, studentDataModel);
            return Ok();
        } 
        [HttpPost("{className}/{subjectName}/{studentName}/{studentSureName}")]
        //[Authorize(Roles = "Director,Admin,Teacher")]
        public ActionResult AddSchoolGrade([FromHeader]int grade,[FromHeader]string gradeDesciption,
            [FromRoute]string className,[FromRoute]string subjectName,[FromRoute]string studentName,
            [FromRoute]string studentSureName )
        {
            _studentRepository.AddGradeForStudent(grade, gradeDesciption, className, subjectName, studentName, studentSureName);
            return Ok();
        }
    }
}