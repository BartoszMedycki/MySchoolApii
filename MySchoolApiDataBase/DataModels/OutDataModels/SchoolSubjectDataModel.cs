using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel
{
    public class SchoolSubjectDataModel
    {
        public string SubcjectName { get; set; }
        public EmployeeDataModel Leader { get; set; }
    }
}
