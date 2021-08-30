using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
   public class Note
    {
        public int Id { get; set; }


        public string SubjectName { get; set; }
        public virtual List<Rate> Rates { get; set; } = new List<Rate>();
    }
}
