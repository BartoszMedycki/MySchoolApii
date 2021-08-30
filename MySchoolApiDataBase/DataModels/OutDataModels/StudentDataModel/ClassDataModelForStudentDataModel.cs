namespace MySchoolApiDataBase.DataModels.OutDataModels
{
    public class ClassDataModelForStudentDataModel
    {
        public string ClassName { get; set; }
        public EmployeeDataModel SupervisingTeacher { get; set; }
    }
}
