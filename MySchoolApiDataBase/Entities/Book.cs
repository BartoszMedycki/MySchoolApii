using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public bool IsAvailable { get; set; }
      
        public virtual Student rentStudent { get; set; }
    }
}
