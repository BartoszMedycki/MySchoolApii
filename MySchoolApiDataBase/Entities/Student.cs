using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
   public class Student
    {
        public int Id { get; set; }
    
        public string Name { get; set; }
        public string Surename { get; set; }
        
        public string Pesel { get; set; }
        public string KeeperName { get; set; }
        public string KeeperSureName { get; set; }
        public string KeeperTelephoneNumber { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        public virtual List<Book> Books { get; set; } = new List<Book>();

        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
      
        public virtual List<SchoolSubject> SchoolSubjects { get; set; } = new List<SchoolSubject>();
        public virtual List<Note> Notes { get; set; } = new List<Note>();
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        
    }
}
