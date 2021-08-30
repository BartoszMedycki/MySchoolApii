using MySchoolApiDataBase.DataModels.OutDataModels.StudentDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class EmployeeDataModel
    {
     
        public int UniqueNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public int ContactTelephoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
