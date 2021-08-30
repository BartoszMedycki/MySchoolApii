using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
   public class Rate
    {
        public int Id { get; set; }
        public int Notee { get; set; }
        public string Description { get; set; }
        public int NoteId { get; set; }

        public virtual Note Note { get; set; }
    }
}
