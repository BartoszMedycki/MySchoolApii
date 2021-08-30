using MySchoolApiDataBase.DataModels.OutDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels
{
    public class GradesFromTheSchoolObjectDataModel
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public List<RateDataModel> Rates { get; set; } 
    }
}
