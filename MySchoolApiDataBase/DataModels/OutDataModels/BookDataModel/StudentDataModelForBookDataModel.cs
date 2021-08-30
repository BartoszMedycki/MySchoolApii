using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class StudentDataModelForBookDataModel
    {
        public string Name { get; set; }
        public string Surename { get; set; }

        public int Pesel { get; set; }
        public string KeeperName { get; set; }
        public int KeeperTelephoneNumber { get; set; }
        public virtual List<BookDataModel> Books { get; set; } = new List<BookDataModel>();

        public string ClassName { get; set; }


       
    }
}
