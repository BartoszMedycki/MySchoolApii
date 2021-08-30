using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;

namespace MySchoolApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LibraryController : ControllerBase
    {


        private readonly ILogger<EmployeeController> _logger;
        private readonly ILibraryRepository libraryRepository;
       

        public MySchoolApiDbContext DbContext { get; }


        public LibraryController(ILogger<EmployeeController> logger,ILibraryRepository libraryRepository)
        {
            _logger = logger;
            this.libraryRepository = libraryRepository;
            ;
        }
        [HttpPost]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult AddBok([FromBody] CreateBookDataModel bookDataModel)
        {
            libraryRepository.AddBook(bookDataModel);
            return Ok();
        
        }
        [HttpGet]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult<IEnumerable<BookDataModel>> GetAllBooks()
        {
            var result = libraryRepository.getAllBooks();
            return Ok(result);
        }
        [HttpGet("Available")]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult<IEnumerable<BookDataModel>> GetAllAvailableBooks()
        {
            var result = libraryRepository.getAvailableBooks();
            return Ok(result);
        } 
        [HttpGet("NotAvailable")]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult<IEnumerable<BookDataModel>> NotAvailable()
        {
            var result = libraryRepository.getNotAvailableBooks();
            return Ok(result);
        }  
        [HttpGet("Students")]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult<IEnumerable<BookDataModel>> GetStudendsWhoRentBooks()
        {
            var result = libraryRepository.GetStudentWhoRentABooks();
            return Ok(result);
        }
        [HttpPatch("rent/{bookId}")]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult RentBook([FromRoute] int bookId, [FromHeader] int studentId)
        {
            libraryRepository.RentBook(bookId, studentId);
            return Ok();
        }
        [HttpPatch("return/{bookId}")]
        [Authorize(Roles = "Librarian,Admin")]
        public ActionResult ReturnBook([FromRoute] int bookId, [FromHeader] int studentId)
        {
            libraryRepository.ReturnBook(bookId, studentId);
            return Ok();
        }
    }
}