using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using System.Collections.Generic;

namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class ClassDataModel1
    {
        public string ClassName { get; set; }
        public EmployeeDataModel SupervisingTeacher { get; set; }
        public virtual List<SchoolSubjectDataModel> SchoolSubjects { get; set; } 
        public virtual List<StudentDataModelForClass> Students { get; set; } 
    }
}
