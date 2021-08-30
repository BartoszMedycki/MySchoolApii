using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels.InDataModels
{
   public class CreateStudentDataModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }

        public double Pesel { get; set; }
        public string KeeperName { get; set; }
        public string KeeperSureName { get; set; }
        public int KeeperTelephoneNumber { get; set; }
    }
}
