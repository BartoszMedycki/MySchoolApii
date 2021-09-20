using MySchoolApiDataBase.DataModels.OutDataModels.StudentDataModel;
using System.Collections.Generic;

namespace MySchoolApiDataBase.DataModels.OutDataModels

{
    public class StudentDataModel1
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string Pesel { get; set; }
        public string KeeperName { get; set; }
        public string KeeperTelephoneNumber { get; set; }
        public virtual List<BookDataModelForStudentDataModel> Books { get; set; } 

        public virtual ClassDataModelForStudentDataModel Class { get; set; }

     
        public List<NoteDataModel> Notes { get; set; } = new List<NoteDataModel>();
    }
}
