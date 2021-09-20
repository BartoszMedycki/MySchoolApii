using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace MySchoolApiDataBase.Entities
{
    public interface IEmployeeRepository
    {
        void AddEmployee(CreateEmployeeDataModel employeeDataModel);
        IEnumerable<EmployeeDataModel> GetAllEmployees(ContextQuery query);
        EmployeeDataModel GetEmployeeByNameAndSurename(string name, string sureName);
        public EmployeeDataModel GetEmployeeByUniqueCode(int code);
        IEnumerable<Employee> GetStartEmployees();
        public void UpdateEmployee(UpdateEmployeeDataModel1 employeeDataModel, int uniqueCode);
        void Seed();
    }
}