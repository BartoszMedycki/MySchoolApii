using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers.CreateDataMappers
{
    public class CreateEmployeeMapper
    {
        IMapper mapper;
        IMapper IgnoreUniqueMapper;
        public CreateEmployeeMapper()
        {
            mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateEmployeeDataModel, Employee>();
               

            }).CreateMapper();

            IgnoreUniqueMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateEmployeeDataModel, Employee>().ForMember(empl => empl.UniqueNumber, config => config.Ignore());

            }).CreateMapper();
        }
        public Employee Map(CreateEmployeeDataModel employeeDataModel)
        {
            return mapper.Map<Employee>(employeeDataModel);
        } 
        public Employee MapIgnoreUniqueNumber(CreateEmployeeDataModel employeeDataModel)
        {
            return IgnoreUniqueMapper.Map<Employee>(employeeDataModel);
        }
    }
}
