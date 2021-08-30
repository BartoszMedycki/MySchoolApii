using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class BookDataModel
    {
        public string author { get; set; }
        public string title { get; set; }
        public bool IsAvailable { get; set; }
        public virtual StudentDataModelForBookDataModel rentStudent { get; set; }
    }
}
