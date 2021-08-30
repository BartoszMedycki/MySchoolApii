using System.Collections.Generic;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class StudentDataModelForClass
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        
        public virtual List<NoteDataModel> Notes { get; set; }
    }
}
