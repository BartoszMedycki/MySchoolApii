using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.DataModels
{
    public class ContextQuery
    {
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
