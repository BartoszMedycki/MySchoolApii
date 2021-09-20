using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MySchoolApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class ClassController : ControllerBase
    {


        private readonly ILogger<EmployeeController> _logger;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILogger<ClassController> logger1;

        public MySchoolApiDbContext DbContext { get; }


        public ClassController(ILogger<EmployeeController> logger, MySchoolApiDbContext dbContext
          , IClassRepository classRepository, IStudentRepository studentRepository, ILogger<ClassController> logger1)
        {
            _logger = logger;
            DbContext = dbContext;
            this._classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.logger1 = logger1;
        }
        [HttpPost]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult AddClass([FromBody] CreateClassDataModel classDataModel)
        {
            _classRepository.CreateClass(classDataModel);
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult<IEnumerable<ClassDataModel1>> GetAllClass()
        {
          
            var result = _classRepository.GetAllClasses();
            return Ok(result);
        }
        [HttpGet("{className}")]
        [Authorize(Roles = "Director,Admin,Teacher")]
        public ActionResult<ClassDataModel1> GetClassByName([FromRoute] string className)
        {
            var result = _classRepository.GetClassByName(className);
            return Ok(result);
        }
        [HttpGet("Subjects/{subjectName}")]
      //  [Authorize(Roles = "Director,Admin,Teacher")]
        public ActionResult<IEnumerable<GradesFromTheSchoolObjectDataModel>> getStudentsWithGradesInTheSubjcets([FromHeader] string className,
            [FromRoute] string subjectName)
        {
            var result = studentRepository.getStudentsWithSubjectGrades(className, subjectName);
            return Ok(result);
        }
        [HttpDelete("{className}")]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult DeleteClass([FromRoute] string className)
        {

            _classRepository.DelateClass(className);
            return Ok();

        }
        [HttpPatch("{className}")]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult UpdateSupervisingTeacher([FromRoute] string className, [FromHeader] int uniqueNumber)
        {
            _classRepository.UpdateSupervisingTeacher(uniqueNumber, className);
            return Ok();

        }
        [HttpPost("Subject/{className}")]
        [Authorize(Roles = "Director,Admin")]
        public ActionResult AddSchoolSubjectToClass([FromRoute]string className,
            [FromBody] CreateSchoolSubjectDataModel subjectDataModel)
        {
            _classRepository.AddSubjectToClass(className, subjectDataModel);
            return Ok();
        }

        


    }
}