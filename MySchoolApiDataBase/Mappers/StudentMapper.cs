using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.DataModels.OutDataModels.StudentDataModel;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers
{
    public class StudentMapper
    {
        IMapper OutMapper;
        IMapper InMapper;
        public StudentMapper()
        {
            OutMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Student, StudentDataModel1>().ReverseMap();
               // config.CreateMap<Student, StudentDataModelForBookDataModel>().ReverseMap();
                config.CreateMap<Book, BookDataModelForStudentDataModel>().ReverseMap();
                config.CreateMap<SchoolSubject, SchoolSubjectDataModel>().ReverseMap();
                config.CreateMap<Class,ClassDataModelForStudentDataModel>().ReverseMap();
            
                config.CreateMap<Role, RoleDataModel>().ReverseMap();
                config.CreateMap<Employee, EmployeeDataModel>().ForMember(Role => Role.RoleName, dto => dto.MapFrom(Role => Role.Role.RoleName));
                config.CreateMap<Employee, EmployeeDataModel>().ForMember(Email=>Email.Email, dto => dto.MapFrom(Role => Role.User.Email)); 
             
               config.CreateMap<Student, StudentDataModel1>().ForMember(Email=>Email.Email, dto => dto.MapFrom(Role => Role.User.Email));
 
                config.CreateMap<Note, NoteDataModel>().ReverseMap();
                config.CreateMap<Rate, RateDataModel>().ReverseMap();

            }).CreateMapper();

            InMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateStudentDataModel, Student>().ReverseMap();
            }).CreateMapper();
        }
        public StudentDataModel1 Map(Student student)
        {
            return OutMapper.Map<StudentDataModel1>(student);
        } 
        public IEnumerable<StudentDataModel1> Map(IEnumerable<Student> students)
        {
            List<StudentDataModel1> listOfMappedStudents = new List<StudentDataModel1>();
            foreach (var student in students)
            {
                listOfMappedStudents.Add(OutMapper.Map<StudentDataModel1>(student));
            }
            return listOfMappedStudents;
        }
        public Student CreateMapperMap(CreateStudentDataModel studentDataModel)
        {
            return InMapper.Map<Student>(studentDataModel);
        }
    }
}
