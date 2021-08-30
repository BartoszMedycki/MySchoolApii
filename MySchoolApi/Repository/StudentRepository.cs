using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MySchoolApiDataBase.Entities
{
    public class StudentRepository : MainRepository<Student>, IStudentRepository
    {
        private readonly IServiceProvider service;
        private readonly StudentMapper mapper;
        private readonly IClassRepository classRepository;
      
        private readonly RateMapper rateMapper;
        private readonly ILogger<StudentRepository> logger;
        private readonly IPasswordHasher<Student> passwordHasher;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public StudentRepository(IServiceProvider service, StudentMapper mapper
            ,IClassRepository classRepository,
            RateMapper rateMapper, ILogger<StudentRepository> logger, IPasswordHasher<Student> passwordHasher
            ,IAuthorizationService authorizationService, IUserContextService userContextService) : base(service)
        {
            this.service = service;
            this.mapper = mapper;
            this.classRepository = classRepository;
            
            this.rateMapper = rateMapper;
            this.logger = logger;
            this.passwordHasher = passwordHasher;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        protected override DbSet<Student> dbSet => dbContext.Students;

        public void Seed()
        {
            if (!dbContext.Students.Any())
            {
                var startStudents = GetStartStudents();
                if (startStudents != null)
                {
                    foreach (var student in startStudents)
                    {
                        dbContext.Students.Add(student);
                    }
                }
            }
        }
        public IEnumerable<StudentDataModel1> getAllStudents()
        {
            
            var allStudents = dbContext.Students.Include(table => table.Books).Include(table => table.Class)
                    .ThenInclude(table=>table.SupervisingTeacher).ThenInclude(table => table.Role)
                    .Include(table => table.SchoolSubjects).ThenInclude(table=>table.Leader)
                    .Include(table => table.Notes)
                    .ThenInclude(table => table.Rates).Include(table => table.SchoolSubjects).ToList();
            if (allStudents != null)
            {
                var mappedStudents = mapper.Map(allStudents);
                return mappedStudents;
            }
            else  throw new NotFoundException("Students not found");

        }
        public StudentDataModel1 GetStudentByNameAndSureName(string name, string surename)
        {
            var student = dbContext.Students.Include(table => table.Books).Include(table => table.Class)
                        .ThenInclude(table => table.SupervisingTeacher).ThenInclude(table => table.Role)
                        .Include(table => table.SchoolSubjects).ThenInclude(table => table.Leader).ThenInclude(table=>table.Role)
                        .Include(table => table.Notes)
                        .ThenInclude(table => table.Rates).Include(table => table.SchoolSubjects)
                        .FirstOrDefault(prop => prop.Name == name && prop.Surename == surename);
             var auth =  authorizationService.AuthorizeAsync(userContextService.Claims, student, new StudentIsOwnerRequirement()).Result;
            if (!auth.Succeeded)
            {
                throw new NotAuthorizeException("unauthorized request");
            }
            if (student != null)
            {
                var mappedStudent = mapper.Map(student);
                return mappedStudent;
            }
            else throw new NotFoundException("Student not found");
                    
        }
        public void StudentRentBook(Student studentById, Book bookById)
        {
            studentById.Books.Add(bookById);
            this.SaveChanges();
        }
        public void StudentReturnBook(Student studentById, Book bookById)
        {
            studentById.Books.Remove(bookById);
            this.SaveChanges();
        }
        public void DeleteStudent(int pesel)
        {
            logger.LogWarning("DeleteStudent method invoked");
            var studentByPesel = dbContext.Students.Include(table=>table.Notes)
                .ThenInclude(table=>table.Rates).FirstOrDefault(prop => prop.Pesel == pesel);
            if (studentByPesel != null)
            {
                dbContext.Students.Remove(studentByPesel);
                this.SaveChanges();
                logger.LogInformation($"Student {studentByPesel.Name}  {studentByPesel.Surename} deleted");
            }
            else throw new NotFoundException("Student not found");
        }
        public void AddStudent(string className,CreateStudentDataModel studentDataModel)
        {
            logger.LogInformation("AddStudent method invoked");
            var mappedStudent = mapper.CreateMapperMap(studentDataModel);
            var classByName = dbContext.Classes.Include(table => table.SchoolSubjects).ThenInclude(table => table.Leader)
                .FirstOrDefault(prop => prop.ClassName == className);
            if (classByName != null)
            {
                var listOfSubjects = classByName.SchoolSubjects.Select(prop => prop.SubcjectName);
                List<Note> listOfNotes = new List<Note>();

                foreach (var name in listOfSubjects)
                {
                    listOfNotes.Add(new Note() { SubjectName = name });
                }

                mappedStudent.Notes = listOfNotes;
                mappedStudent.Class = classByName;
                mappedStudent.Role = dbContext.Roles.FirstOrDefault(prop=>prop.RoleName == "Student");
                var hashedPassword = passwordHasher.HashPassword(mappedStudent, studentDataModel.Password);
                mappedStudent.User = new User()
                {
                    Email = studentDataModel.Email,
                    PasswordHash = hashedPassword
                };
                //mappedStudent.User.Email = studentDataModel.Email;
                //mappedStudent.User.PasswordHash = studentDataModel.ConfirmPassword;

                this.SaveChanges();
                classRepository.AddStudentToClass(mappedStudent, className);
                logger.LogInformation($"Student {mappedStudent.Name} {mappedStudent.Surename} added to class {classByName.ClassName}");



            }
            else throw new NotFoundException("Class not found");
        }
        public IEnumerable<GradesFromTheSchoolObjectDataModel> getStudentsWithSubjectGrades(string className, string subjectName)
        {
            var classByName = dbContext.Classes.Include(table => table.Students).ThenInclude(table => table.Notes)
                    .ThenInclude(table => table.Rates).Include(table => table.SchoolSubjects).FirstOrDefault(prop => prop.ClassName == className);
            if (classByName != null)
            {
                var students = classByName.Students;
                if (students != null)
                {
                    List<GradesFromTheSchoolObjectDataModel> listOfGradesFromSchoolObjDto = new List<GradesFromTheSchoolObjectDataModel>();
                    foreach (var student in students)
                    {
                        var subjectByName = student.Notes.FirstOrDefault(prop => prop.SubjectName == subjectName);
                        if (subjectByName != null)
                        {

                            listOfGradesFromSchoolObjDto.Add(new GradesFromTheSchoolObjectDataModel()
                            {
                                Name = student.Name,
                                Surename = student.Surename,
                                Rates = rateMapper.Map(subjectByName.Rates)


                            });
                           
                        }
                        else throw new NotFoundException("Subjects  not found");

                    }

                    return listOfGradesFromSchoolObjDto;
                }
                else throw new NotFoundException("Student not found");
               
                


            }
            else throw new NotFoundException("Class not found");

            


        }
        public void AddGradeForStudent(int grade,string gradeDescription,string className,string subjectName,
            string studentMame, string studentSurename )
        {
            var classByName = dbContext.Classes.Include(table => table.Students).ThenInclude(table => table.Notes)
                   .ThenInclude(table => table.Rates).Include(table => table.SchoolSubjects).Include(prop=>prop.Students)
                   .ThenInclude(prop=>prop.SchoolSubjects)
                   .ThenInclude(prop=>prop.Leader)
                   .FirstOrDefault(prop => prop.ClassName == className);
            if (classByName != null)
            {
                var student = classByName.Students.FirstOrDefault(prop => prop.Name == studentMame && prop.Surename == studentSurename);
                if (student != null)
                {
                  
                    var subject = student.SchoolSubjects.FirstOrDefault(prop => prop.SubcjectName == subjectName);
                    if (subject != null)
                    {
                        var subjectLeader = subject.Leader;
                        var authResult=authorizationService
                            .AuthorizeAsync(userContextService.Claims, subjectLeader, new DoesTeachTheSubjectRequirement()).Result;
                        if (authResult.Succeeded)
                        {
                            var subjectForNote = student.Notes.FirstOrDefault(prop => prop.SubjectName == subjectName);
                            subjectForNote.Rates.Add(new Rate { Notee = grade, Description = gradeDescription });
                            this.SaveChanges();
                        }
                        else throw new NotAuthorizeException("Unathorize request");
                      
                    }
                    else throw new NotFoundException("Subject not found");
                    
                }
                else throw new NotFoundException("Student not found");
               
            }
            else throw new NotFoundException("Class not found");

      
        }

        public IEnumerable<Student> GetStartStudents()
        {
            var Class1A = dbContext.Classes.Include(table => table.SchoolSubjects).Include(table => table.Students).
                FirstOrDefault(data => data.ClassName == "1A");
            var listOfClassSubject = Class1A.SchoolSubjects;
            var namesOfSubject = listOfClassSubject.Select(s => s.SubcjectName);
            List<Note> listOfNotes = new List<Note>();
            foreach (var name in namesOfSubject)
            {
                listOfNotes.Add(new Note() { SubjectName = name });

            }



            List<Student> listOfStartStudents = new List<Student>()

        {
            new Student()
            {
            Name = "Kamil",
            Surename = "Grosicki",
            User = new User()
            {
              Email = "barmed553@wp.pl",
              PasswordHash = "admin123",
            },
          
            Pesel = 666444333,
            KeeperName = "Wojtek",
            KeeperSureName = " Gola",
            KeeperTelephoneNumber = 568489976,
            Class = Class1A,
            SchoolSubjects = listOfClassSubject,
            Notes =listOfNotes,
            
            Role = new Role()
            {
            RoleName = "Student"
            },
            
            


            }
        };
            return listOfStartStudents;

        }
    }
}
