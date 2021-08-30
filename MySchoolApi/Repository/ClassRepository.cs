using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class ClassRepository : MainRepository<Class>, IClassRepository
    {
        private readonly ClassMapper _classMapper;
        private readonly ILogger<ClassRepository> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public ClassRepository(IServiceProvider service, ClassMapper classMapper, ILogger<ClassRepository> logger, 
            IAuthorizationService authorizationService,IUserContextService userContextService) : base(service)
        {
            _classMapper = classMapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        protected override DbSet<Class> dbSet => dbContext.Classes;
        public void CreateClass(CreateClassDataModel classDataModel)
        {
            logger.LogInformation("CreateClass method invoked");
            var mappedClass = _classMapper.Map(classDataModel);
            var employeeByUniqueNumber = dbContext.Employees
                           .FirstOrDefault(employee => employee.UniqueNumber == classDataModel.SupervisingTeacherUniqueNumber);
            if (employeeByUniqueNumber != null)
            {
                mappedClass.SupervisingTeacher = employeeByUniqueNumber;

                dbSet.Add(mappedClass);
                this.SaveChanges();
                logger.LogInformation($"Class {classDataModel.ClassName} created");
            }
            else throw new NotFoundException("Employee not found");
            
            
            
            
        }
        public IEnumerable<ClassDataModel1> GetAllClasses()
        {
            
            var allClasses = dbContext.Classes.Include(table => table.SchoolSubjects).ThenInclude(table=>table.Leader)
                .ThenInclude(table=>table.Role)
                .Include(table => table.Students)
                .Include(table=>table.Students)
                .ThenInclude(table=>table.Notes).ThenInclude(table=>table.Rates)
                .Include(table=>table.SupervisingTeacher).ThenInclude(table=>table.Role).ToList();
            
            if (allClasses != null)
            {
                var mappedClasses = _classMapper.Map2(allClasses);
                return mappedClasses;
            }
            else throw new NotFoundException("Classes not found");
            
        }
        public ClassDataModel1 GetClassByName(string className)
        {
            var ClassByName = dbContext.Classes.Include(table => table.SchoolSubjects).ThenInclude(table => table.Leader)
                    .ThenInclude(table => table.Role)
                    .Include(table => table.Students)
                    .Include(table => table.Students)
                    .ThenInclude(table => table.Notes).ThenInclude(table => table.Rates)
                    .Include(table => table.SupervisingTeacher).ThenInclude(table => table.Role)
                    .FirstOrDefault(prop => prop.ClassName == className);
            
            

            if (ClassByName != null)
            {
                var authResult = authorizationService.AuthorizeAsync(userContextService.Claims, ClassByName, new DoesTheSupervisingTeacherRequirement()).Result;
                if (authResult.Succeeded)
                {
                    var mappedClass = _classMapper.Map2(ClassByName);
                    return mappedClass;
                }
                else throw new NotAuthorizeException("Unathorize request");
                  
            }
            else throw new NotFoundException("Class not found");

            
        }
       
        public void DelateClass(string name)
        {
           logger.LogWarning($"DeleteClass method on {name} class invoked");
            var classByName = dbContext.Classes.FirstOrDefault(prop => prop.ClassName == name);
            if (classByName != null)
            {
                
                dbContext.Classes.Remove(classByName);
                this.SaveChanges();
                logger.LogInformation($"Class {name} deleted");
            }
            else throw new NotFoundException("Class not found");
        }
        public void AddStudentToClass(Student student,string className)
        {

            logger.LogInformation($"AddStudentToClass method invoked || student: {student.Name} {student.Surename}, class : {className}");
            if (student != null)
            {
                var classByName = dbContext.Classes.Include(table => table.SchoolSubjects).ThenInclude(table => table.Students)
                      .FirstOrDefault(prop => prop.ClassName == className);
                if (classByName != null)
                {
                    classByName.Students.Add(student);

                    var listOfSchoolSubject = classByName.SchoolSubjects;
                    foreach (var subject in listOfSchoolSubject)
                    {
                        subject.Students.Add(student);
                    }

                    this.SaveChanges();
                    logger.LogInformation($"Student {student.Name} {student.Surename} added to class {className}");
                }
                else throw new NotFoundException("Class not found");

            }
            else throw new NotFoundException("Student not found");
        
        }
        public void UpdateSupervisingTeacher(int teacherUniqueNumber,string className)
        {
            logger.LogInformation($"UpdateSupervisingTeacher method invoked || class : {className}");
            var clasByName = dbContext.Classes.Include(table => table.SupervisingTeacher)
                             .FirstOrDefault(prop => prop.ClassName == className);
            if (clasByName != null)
            {
                clasByName.SupervisingTeacher = dbContext.Employees
                          .FirstOrDefault(prop => prop.UniqueNumber == teacherUniqueNumber);
                this.SaveChanges();
                logger.LogInformation($"SupervisingTeacher in class {className} updated");
            }
            else throw new NotFoundException("Class not found");
        }
        public void AddSubjectToClass(string className,CreateSchoolSubjectDataModel subject)
        {
            logger.LogInformation($"AddSubjectToClass method invoked || subjectName : {subject.SubcjectName}, class : {className}");
            var classByName = dbContext.Classes.Include(table => table.SchoolSubjects)
                     .FirstOrDefault(prop => prop.ClassName == className);
            if (classByName != null)
            {
                var employeeByUniqueCode = dbContext.Employees.Include(table => table.Role)
                        .FirstOrDefault(prop => prop.UniqueNumber == subject.UniqueId);
                if (employeeByUniqueCode != null)
                {
                    var a = employeeByUniqueCode;
                    var b = subject.SubcjectName;
                    classByName.SchoolSubjects.Add(new SchoolSubject { Leader = employeeByUniqueCode, SubcjectName = subject.SubcjectName });
                    this.SaveChanges();
                    logger.LogInformation($"Subject {subject.SubcjectName} added to class {className}");
                }
                else throw new NotFoundException("Employee not found");
            }
            else throw new NotFoundException("Class not found");
        }
        public void Seed()
        {

            if (!dbContext.Classes.Any())
            {
                var startClasses = GetStartClasses();
                if (startClasses != null)
                {
                    foreach (var startClass in startClasses)
                    {
                        dbContext.Classes.Add(startClass);
                        this.SaveChanges();
                    }
                }

            }
        }
        
        public IEnumerable<Class> GetStartClasses()
        {
            List<SchoolSubject> listOfSchoolSubjects = new List<SchoolSubject>();
            listOfSchoolSubjects = dbContext.SchoolSubjects.ToList();
            List<Class> listOfStartClasses = new List<Class>()
             {
                new Class()
                {
                ClassName = "1A",
                SupervisingTeacher = dbContext.Employees.Where(employee=>employee.UniqueNumber == 9997).FirstOrDefault(),
                SchoolSubjects = listOfSchoolSubjects
                },
                new Class()
                {
                ClassName = "1B",
                SupervisingTeacher = dbContext.Employees.Where(employee=>employee.UniqueNumber == 9998).FirstOrDefault()
                }

            };
            return listOfStartClasses;
        }


    }
}
