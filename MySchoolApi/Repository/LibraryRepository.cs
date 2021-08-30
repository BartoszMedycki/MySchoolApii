using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySchoolApiDataBase.Entities
{
    public class LibraryRepository : MainRepository<Book>, ILibraryRepository
    {
        private readonly CreateBookMapper CreateBookMapper;
        private readonly IStudentRepository studentRepository;
        private readonly BookMapper bookMapper;
        private readonly StudentMapper studentMapper;
        private readonly ILogger<LibraryRepository> logger;

        public LibraryRepository(IServiceProvider service, CreateBookMapper CreateBookMapper, IStudentRepository studentRepository
           , BookMapper bookMapper, StudentMapper studentMapper, ILogger<LibraryRepository> logger) : base(service)
        {
            this.CreateBookMapper = CreateBookMapper;
            this.studentRepository = studentRepository;
            this.bookMapper = bookMapper;
            this.studentMapper = studentMapper;
            this.logger = logger;
        }

        protected override DbSet<Book> dbSet => dbContext.Books;

        public void AddBook(CreateBookDataModel bookDataModel)
        {
            logger.LogInformation("AddBook method invoked");
            var mappedBook = CreateBookMapper.Map(bookDataModel);
            dbContext.Books.Add(mappedBook);
            this.SaveChanges();
            logger.LogInformation($"Book : {mappedBook.author} {mappedBook.title} Added");

        }
        public void DeleteBook(int bookId)
        {
            logger.LogWarning("DeleteBook method invoked");
            var bookById = dbContext.Books.FirstOrDefault(prop => prop.Id == bookId);
            dbContext.Books.Remove(bookById);
            this.SaveChanges();
            logger.LogInformation($"Book : {bookById.author} {bookById.title} Deleted");
        }
        public void RentBook(int bookId, int studentId)
        {
            var bookById = dbContext.Books.FirstOrDefault(prop => prop.Id == bookId);
            if (bookById != null)
            {
                var studentById = dbContext.Students.Include(table => table.Books).FirstOrDefault(prop => prop.Id == studentId);
                if (studentById != null)
                {
                    bookById.IsAvailable = false;
                    this.SaveChanges();
                    studentRepository.StudentRentBook(studentById, bookById);
                }
                else throw new NotFoundException("Student not found");

            }
            else throw new NotFoundException("Book not found");

        }
        public void ReturnBook(int bookId, int studentId)
        {
            var bookById = dbContext.Books.Include(prop => prop.rentStudent).FirstOrDefault(prop => prop.Id == bookId);
            if (bookById != null)
            {
                var studentById = dbContext.Students.Include(table => table.Books).FirstOrDefault(prop => prop.Id == studentId);
                if (studentById != null)
                {
                    bookById.IsAvailable = true;
                    this.SaveChanges();
                    studentRepository.StudentRentBook(studentById, bookById);
                }
                else throw new NotFoundException("Student notFound");

            }
            else throw new NotFoundException("Book not found");

        }
        public IEnumerable<BookDataModel> getAvailableBooks()
        {
            var allAvailableBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class)
                .Where(prop => prop.IsAvailable == true).ToList();
            if (allAvailableBooks != null)
            {
                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.Map(allAvailableBooks);
                foreach (var book in mappedBooks)
                {
                    listOfBooksDataModels.Add(book);
                }
                return listOfBooksDataModels;
            }
            else throw new NotFoundException("Books not found");
        }
        public IEnumerable<BookDataModel> getNotAvailableBooks()
        {
            var allNotAvailableBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class)
                .Where(prop => prop.IsAvailable == false).ToList();
            if (allNotAvailableBooks != null)
            {
                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.Map(allNotAvailableBooks);
                foreach (var book in mappedBooks)
                {
                    listOfBooksDataModels.Add(book);
                }
                return listOfBooksDataModels;
            }
            else throw new NotFoundException("Books not found");
        }
        public IEnumerable<BookDataModel> getAllBooks()
        {
            var allNotAvailableBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class).ToList();
            if (allNotAvailableBooks != null)
            {
                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.Map(allNotAvailableBooks);
                foreach (var book in mappedBooks)
                {
                    listOfBooksDataModels.Add(book);
                }
                return listOfBooksDataModels;
            }
            else throw new NotFoundException("Books not found");
        }
        public IEnumerable<StudentDataModel1> GetStudentWhoRentABooks()
        {
            var studentsWhoRentBooks = dbContext.Students.Include(prop => prop.Books).Include(prop => prop.Class).ThenInclude(prop=>prop.SupervisingTeacher)
                .ThenInclude(prop=>prop.Role).OrderBy(prop => prop.Class.ClassName).ToList();
            if (studentsWhoRentBooks != null)
            {
                var mappedStudents = studentMapper.Map(studentsWhoRentBooks);
                return mappedStudents;
            }
            else throw new NotFoundException("Students not found");

          

        } 
        public IEnumerable<StudentDataModel1> GetClassWithStudentsWhoRentBooks(string className)
        {
            var studentsWhoRentBooks = dbContext.Students.Include(prop => prop.Books).Include(prop => prop.Class).ThenInclude(prop => prop.SupervisingTeacher)
                .ThenInclude(prop => prop.Role).Where(prop=>prop.Class.ClassName == className).ToList();
            if (studentsWhoRentBooks != null)
            {
                var mappedStudents = studentMapper.Map(studentsWhoRentBooks);
                return mappedStudents;
            }
            else throw new NotFoundException("Students not found");



        }  
        public StudentDataModel1 GetStudentWhoRentBookById(int studentId)
        {
            var studentsWhoRentBooks = dbContext.Students.Include(prop => prop.Books).Include(prop => prop.Class).ThenInclude(prop => prop.SupervisingTeacher)
                .ThenInclude(prop => prop.Role).FirstOrDefault(prop => prop.Id == studentId);
            if (studentsWhoRentBooks != null)
            {
                var mappedStudent = studentMapper.Map(studentsWhoRentBooks);
                return mappedStudent;
            }
            else throw new NotFoundException("Student not found");



        }
    }
}
