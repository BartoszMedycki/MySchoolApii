using AutoMapper;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers
{
   public class EmployeeMapper
    {
        IMapper mapper;
        public EmployeeMapper()
        {
            mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Employee, EmployeeDataModel>().ForMember(Role=>Role.RoleName,dto =>dto.MapFrom(Role=>Role.Role.RoleName));
                

            }).CreateMapper();
        }
        
        public EmployeeDataModel Map(Employee employeeDataModel)
        {
            return mapper.Map<EmployeeDataModel>(employeeDataModel);
        } 
        public IEnumerable<EmployeeDataModel> Map(IEnumerable<Employee> employeeDataModel)
        {
            List<EmployeeDataModel> listofEmployeeDto = new List<EmployeeDataModel>();
            foreach (var employee in employeeDataModel)
            {
                listofEmployeeDto.Add(mapper.Map<EmployeeDataModel>(employee));
            }
            return listofEmployeeDto;
        }
    }
}
