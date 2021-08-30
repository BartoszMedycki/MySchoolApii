using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers
{
   public class ClassMapper
    {
        IMapper InMapper;
        IMapper OutMapper;
        public ClassMapper()
        {
            InMapper = new MapperConfiguration(config =>
            {
               config.CreateMap<CreateClassDataModel, Class>();
                

            }).CreateMapper();

            OutMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Class, ClassDataModel1>().ReverseMap(); ;
                config.CreateMap<Student, StudentDataModelForClass>().ReverseMap();
                config.CreateMap<SchoolSubject, SchoolSubjectDataModel>().ReverseMap();
                config.CreateMap<Note, NoteDataModel>().ReverseMap();
                config.CreateMap<Rate, RateDataModel>().ReverseMap();
                config.CreateMap<Employee, EmployeeDataModel>().ForMember(Role => Role.RoleName, dto => dto.MapFrom(Role => Role.Role.RoleName));
            }).CreateMapper();
        }
        public Class Map(CreateClassDataModel classDataModel)
        {
            return InMapper.Map<Class>(classDataModel);
        } 
        public List<ClassDataModel1> Map2(List<Class> classes)
        {
            List<ClassDataModel1> listOfClassDataModel = new List<ClassDataModel1>();
            foreach (var oneClass in classes)
            {
                listOfClassDataModel.Add( OutMapper.Map<ClassDataModel1>(oneClass));
            }
            return listOfClassDataModel;
        } 
        public ClassDataModel1 Map2(Class classToMap)
        {

            return OutMapper.Map<ClassDataModel1>(classToMap);
          
        }
    }
}
