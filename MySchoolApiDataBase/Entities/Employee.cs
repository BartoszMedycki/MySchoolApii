using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
   public class Employee
    {
        public int Id { get; set; }
      
        public int UniqueNumber { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
     
        public int ContactTelephoneNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public int RoleId { get; set; }
      
        public Role Role { get; set; }

    }
}
