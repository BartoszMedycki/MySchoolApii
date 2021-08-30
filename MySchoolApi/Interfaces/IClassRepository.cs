using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using System.Collections.Generic;

namespace MySchoolApiDataBase.Entities
{
    public interface IClassRepository
    {
        void CreateClass(CreateClassDataModel classDataModel);
        IEnumerable<Class> GetStartClasses();
        public IEnumerable<ClassDataModel1> GetAllClasses();
        public void DelateClass(string name);
        public void UpdateSupervisingTeacher(int teacherUniqueNumber, string className);
        public void AddSubjectToClass(string className, CreateSchoolSubjectDataModel subject);
        public void AddStudentToClass(Student student, string className);
        public ClassDataModel1 GetClassByName(string className);
        void Seed();
    }
}