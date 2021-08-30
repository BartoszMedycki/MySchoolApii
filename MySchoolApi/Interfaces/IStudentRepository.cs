using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace MySchoolApiDataBase.Entities
{
    public interface IStudentRepository
    {
        IEnumerable<StudentDataModel1> getAllStudents();
        IEnumerable<Student> GetStartStudents();
        public StudentDataModel1 GetStudentByNameAndSureName(string name, string surename);
        public void DeleteStudent(int pesel);
        public void StudentRentBook(Student studentById, Book bookById);
        public void AddStudent(string className, CreateStudentDataModel studentDataModel);
        public IEnumerable<GradesFromTheSchoolObjectDataModel> getStudentsWithSubjectGrades(string className, string subjectName);
        public void AddGradeForStudent(int grade, string gradeDescription, string className, string subjectName,
            string studentMame, string studentSurename);
        public void Seed();
    }
}