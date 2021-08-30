using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.InDataModels
{
   public class CreateEmployeeDataModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UniqueNumber { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public int ContactTelephoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
