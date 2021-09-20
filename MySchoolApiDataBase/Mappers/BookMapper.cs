using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;

namespace MySchoolApiDataBase.Mappers.CreateDataMappers
{
    public class BookMapper
    {
        IMapper OutDataMapper;
        IMapper InDataMapper;
        public BookMapper()
        {
            OutDataMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Book, BookDataModel>().ReverseMap();
                config.CreateMap<Student, StudentDataModelForBookDataModel>().ForMember(prop => prop.ClassName, prop => prop.MapFrom(prop => prop.Class.ClassName))
                .ReverseMap();
            }).CreateMapper();

            InDataMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateBookDataModel, Book>().ReverseMap();
            }).CreateMapper();
        }

        public IEnumerable<BookDataModel> OutMap(IEnumerable<Book> booksDataModel)
        {
            List<BookDataModel> listOfMappedBooks = new List<BookDataModel>();
            foreach (var book in booksDataModel)
            {
                listOfMappedBooks.Add(OutDataMapper.Map<BookDataModel>(book));
            }
            return listOfMappedBooks;

        }
        public BookDataModel OutMap(Book booksDataModel)
        {
            return OutDataMapper.Map<BookDataModel>(booksDataModel);

        }

        public IEnumerable<Book> InMap(IEnumerable<CreateBookDataModel> booksDataModel)
        {
            List<Book> listOfMappedBooks = new List<Book>();
            foreach (var book in booksDataModel)
            {
                listOfMappedBooks.Add(InDataMapper.Map<Book>(book));
            }
            return listOfMappedBooks;

        }
        public Book InMap(CreateBookDataModel booksDataModel)
        {
            return InDataMapper.Map<Book>(booksDataModel);

        }
    }
}
