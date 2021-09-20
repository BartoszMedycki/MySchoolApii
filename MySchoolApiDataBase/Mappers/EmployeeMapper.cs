using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
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
        IMapper OutMapper;
        IMapper InMapper;
        IMapper UpdateMapper;
        public EmployeeMapper()
        {
            OutMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Employee, EmployeeDataModel>().ForMember(Role=>Role.RoleName,dto =>dto.MapFrom(Role=>Role.Role.RoleName));
                

            }).CreateMapper();

            InMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateEmployeeDataModel, Employee>();


            }).CreateMapper();

            

        }
        
        public EmployeeDataModel OutDataMap(Employee employeeDataModel)
        {
            return OutMapper.Map<EmployeeDataModel>(employeeDataModel);
        } 
        public IEnumerable<EmployeeDataModel> OutDataMap(IEnumerable<Employee> employeeDataModel)
        {
            List<EmployeeDataModel> listofEmployeeDto = new List<EmployeeDataModel>();
            foreach (var employee in employeeDataModel)
            {
                listofEmployeeDto.Add(OutMapper.Map<EmployeeDataModel>(employee));
            }
            return listofEmployeeDto;
        }

        public Employee InDataMap(CreateEmployeeDataModel employeeDataModel)
        {
            return InMapper.Map<Employee>(employeeDataModel);
        }
        
    }
}
