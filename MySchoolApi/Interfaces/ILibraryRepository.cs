using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using System.Collections.Generic;

namespace MySchoolApiDataBase.Entities
{
    public interface ILibraryRepository
    {
        void AddBook(CreateBookDataModel bookDataModel);
        IEnumerable<BookDataModel> getAllBooks();
        IEnumerable<BookDataModel> getAvailableBooks();
        IEnumerable<BookDataModel> getNotAvailableBooks();
        public IEnumerable<StudentDataModel1> GetStudentWhoRentABooks();
        void RentBook(int bookId, int studentId);
        void ReturnBook(int bookId, int studentId);
    }
}