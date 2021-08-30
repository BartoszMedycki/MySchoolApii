using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class SchoolSubject
    {
        public int Id { get; set; }
        public string SubcjectName { get; set; }

        public virtual List<Student> Students { get; set; } = new List<Student>();
        public int LeaderId { get; set; }
        public Employee Leader { get; set; }
       

    }
}
