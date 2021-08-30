using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
       
        public int SupervisingTeacherId { get; set; }

        public Employee SupervisingTeacher { get; set; }
        public virtual List<Student> Students { get; set; } = new List<Student>();
        public virtual List<SchoolSubject> SchoolSubjects { get; set; } = new List<SchoolSubject>();

    }

  
}
