using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using System.Collections.Generic;

namespace MySchoolApiDataBase.Entities
{
    public interface ILibraryRepository
    {
        void AddBook(CreateBookDataModel bookDataModel);
        IEnumerable<BookDataModel> getAllBooks(ContextQuery query);
        IEnumerable<BookDataModel> getAvailableBooks(ContextQuery query);
        IEnumerable<BookDataModel> getNotAvailableBooks(ContextQuery query);
        public IEnumerable<StudentDataModel1> GetStudentWhoRentABooks();
        void RentBook(int bookId, int studentId);
        void ReturnBook(int bookId, int studentId);
    }
}