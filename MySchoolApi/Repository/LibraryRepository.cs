using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApi;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MySchoolApiDataBase.Entities
{
    public class LibraryRepository : MainRepository<Book>, ILibraryRepository
    {
        
        private readonly IStudentRepository studentRepository;
        private readonly BookMapper bookMapper;
        private readonly StudentMapper studentMapper;
        private readonly ILogger<LibraryRepository> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public LibraryRepository(IServiceProvider service,  IStudentRepository studentRepository
           , BookMapper bookMapper, StudentMapper studentMapper, ILogger<LibraryRepository> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService) : base(service)
        {
         
            this.studentRepository = studentRepository;
            this.bookMapper = bookMapper;
            this.studentMapper = studentMapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        protected override DbSet<Book> dbSet => dbContext.Books;

        public void AddBook(CreateBookDataModel bookDataModel)
        {
            logger.LogInformation("AddBook method invoked");
            var result = authorizationService.AuthorizeAsync(userContextService.Claims,bookDataModel,new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
            {
                var mappedBook = bookMapper.InMap(bookDataModel);
                dbContext.Books.Add(mappedBook);
                this.SaveChanges();
                logger.LogInformation($"Book : {mappedBook.author} {mappedBook.title} Added");
            }
            else throw new NotAuthorizeException("Not authorize request");
            

        }
        public void DeleteBook(int bookId)
        {
            logger.LogWarning("DeleteBook method invoked");
            var result = authorizationService.AuthorizeAsync(userContextService.Claims, bookId, new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
            {
                var bookById = dbContext.Books.FirstOrDefault(prop => prop.Id == bookId);
                dbContext.Books.Remove(bookById);
                this.SaveChanges();
                logger.LogInformation($"Book : {bookById.author} {bookById.title} Deleted");
            }
            
        }
        public void RentBook(int bookId, int studentId)
        {
            logger.LogInformation($"RentBook MethodInvoked");
            var result = authorizationService.AuthorizeAsync(userContextService.Claims, bookId, new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
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
                        logger.LogInformation($"Student {studentById.Name}  {studentById.Surename} borrowed book with Id : {bookById.Id}");
                    }
                    else throw new NotFoundException("Student not found");

                }
                else throw new NotFoundException("Book not found");
            }
            else throw new NotAuthorizeException("Not authorize request");

        }
        public void ReturnBook(int bookId, int studentId)
        {
            logger.LogInformation($"ReturnBook MethodInvoked");
            var bookById = dbContext.Books.Include(prop => prop.rentStudent).FirstOrDefault(prop => prop.Id == bookId);
            if (bookById != null)
            {
                var studentById = dbContext.Students.Include(table => table.Books).FirstOrDefault(prop => prop.Id == studentId);
                if (studentById != null)
                {
                    bookById.IsAvailable = true;
                    this.SaveChanges();
                    studentRepository.StudentReturnBook(studentById, bookById);
                    logger.LogInformation($"Student {studentById.Name}  {studentById.Surename} returned book with Id : {bookById.Id}");
                }
                else throw new NotFoundException("Student notFound");

            }
            else throw new NotFoundException("Book not found");

        }
        public IEnumerable<BookDataModel> getAvailableBooks(ContextQuery query)
        {
            var allAvailableBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class)
                .Where(prop => prop.IsAvailable == true).Where(prop=>prop.Id!=null);
            if (allAvailableBooks != null)
            {
                if (string.IsNullOrEmpty(query.SortBy))
                {
                    allAvailableBooks.OrderBy(prop => prop.author);
                }
                else if (!string.IsNullOrEmpty(query.SortBy))
                {
                    var columnSelector = new Dictionary<string, Expression<Func<Book, object>>>
                    {
                         { nameof(Book.author),  prop=>prop.author },
                         { nameof(Book.title),  prop=>prop.title },
                    };
                    var selectedColumn = columnSelector[query.SortBy];

                    allAvailableBooks = query.SortDirection == SortDirection.ASC ? allAvailableBooks.OrderBy(selectedColumn)
                                                                                 : allAvailableBooks.OrderByDescending(selectedColumn);

                }
                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.OutMap(allAvailableBooks);
                foreach (var book in mappedBooks)
                {
                    listOfBooksDataModels.Add(book);
                }
                return listOfBooksDataModels;
            }
            else throw new NotFoundException("Books not found");
        }
        public IEnumerable<BookDataModel> getNotAvailableBooks(ContextQuery query)
        {
            var allNotAvailableBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class)
                .Where(prop => prop.IsAvailable == false).Where(prop=>prop.Id!=null);
            if (allNotAvailableBooks != null)
            {
                if (string.IsNullOrEmpty(query.SortBy))
                {
                    allNotAvailableBooks.OrderBy(prop => prop.author);
                }
                else if (!string.IsNullOrEmpty(query.SortBy))
                {
                    var columnSelector = new Dictionary<string, Expression<Func<Book, object>>>
                    {
                         { nameof(Book.author),  prop=>prop.author },
                         { nameof(Book.title),  prop=>prop.title },
                    };
                    var selectedColumn = columnSelector[query.SortBy];

                    allNotAvailableBooks = query.SortDirection == SortDirection.ASC ? allNotAvailableBooks.OrderBy(selectedColumn)
                                                                                 : allNotAvailableBooks.OrderByDescending(selectedColumn);

                }
                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.OutMap(allNotAvailableBooks);
                foreach (var book in mappedBooks)
                {
                    listOfBooksDataModels.Add(book);
                }
                return listOfBooksDataModels;
            }
            else throw new NotFoundException("Books not found");
        }
        public IEnumerable<BookDataModel> getAllBooks(ContextQuery query)
        {
            var AllBooks = dbContext.Books.Include(prop=>prop.rentStudent).ThenInclude(prop=>prop.Class).Where(prop=>prop.Id!=null);
            if (AllBooks != null)
            {
                if (string.IsNullOrEmpty(query.SortBy))
                {
                    AllBooks.OrderBy(prop => prop.author);
                }
                else if (!string.IsNullOrEmpty(query.SortBy))
                {
                    var columnSelector = new Dictionary<string, Expression<Func<Book, object>>>
                    {
                         { nameof(Book.author),  prop=>prop.author },
                         { nameof(Book.title),  prop=>prop.title },
                    };
                        var selectedColumn = columnSelector[query.SortBy];

                        AllBooks = query.SortDirection == SortDirection.ASC ? AllBooks.OrderBy(selectedColumn)
                                                                                     : AllBooks.OrderByDescending(selectedColumn);

                }

                List<BookDataModel> listOfBooksDataModels = new List<BookDataModel>();
                var mappedBooks = bookMapper.OutMap(AllBooks);
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
            var result = authorizationService.AuthorizeAsync(userContextService.Claims, studentsWhoRentBooks, new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
            {
                if (studentsWhoRentBooks != null)
                {
                    var mappedStudents = studentMapper.Map(studentsWhoRentBooks);
                    return mappedStudents;
                }
                else throw new NotFoundException("Students not found");
            }
            else throw new NotAuthorizeException("Not Authorize request");

          

        } 
        public IEnumerable<StudentDataModel1> GetClassWithStudentsWhoRentBooks(string className)
        {
            var studentsWhoRentBooks = dbContext.Students.Include(prop => prop.Books).Include(prop => prop.Class).ThenInclude(prop => prop.SupervisingTeacher)
                .ThenInclude(prop => prop.Role).Where(prop=>prop.Class.ClassName == className).ToList();
            var result = authorizationService.AuthorizeAsync(userContextService.Claims, studentsWhoRentBooks, new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
            {
                if (studentsWhoRentBooks != null)
                {
                    var mappedStudents = studentMapper.Map(studentsWhoRentBooks);
                    return mappedStudents;
                }
                else throw new NotFoundException("Students not found");
            }
            else throw new NotAuthorizeException("Not Authorize request");



        }  
        public StudentDataModel1 GetStudentWhoRentBookById(int studentId)
        {
            var studentsWhoRentBooks = dbContext.Students.Include(prop => prop.Books).Include(prop => prop.Class).ThenInclude(prop => prop.SupervisingTeacher)
                .ThenInclude(prop => prop.Role).FirstOrDefault(prop => prop.Id == studentId);
            var result = authorizationService.AuthorizeAsync(userContextService.Claims, studentsWhoRentBooks, new IsLibrarianRequirement()).Result;
            if (result.Succeeded)
            {
                if (studentsWhoRentBooks != null)
                {
                    var mappedStudent = studentMapper.Map(studentsWhoRentBooks);
                    return mappedStudent;
                }
                else throw new NotFoundException("Student not found");
            }
            else throw new NotAuthorizeException("Not Authorize request");



        }
    }
}
