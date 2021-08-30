using System.Collections.Generic;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class NoteDataModel
    {
        public string SubjectName { get; set; }
        public virtual List<RateDataModel> Rates { get; set; } = new List<RateDataModel>();
     
    }
}
