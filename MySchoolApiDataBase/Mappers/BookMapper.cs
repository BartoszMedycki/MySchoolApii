using AutoMapper;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;

namespace MySchoolApiDataBase.Mappers.CreateDataMappers
{
    public class BookMapper
    {
        IMapper mapper;
        public BookMapper()
        {
            mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Book, BookDataModel>().ReverseMap();
                config.CreateMap<Student, StudentDataModelForBookDataModel>().ForMember(prop => prop.ClassName, prop => prop.MapFrom(prop => prop.Class.ClassName))
                .ReverseMap();
            }).CreateMapper();
        }

        public IEnumerable<BookDataModel> Map(IEnumerable<Book> booksDataModel)
        {
            List<BookDataModel> listOfMappedBooks = new List<BookDataModel>();
            foreach (var book in booksDataModel)
            {
                listOfMappedBooks.Add(mapper.Map<BookDataModel>(book));
            }
            return listOfMappedBooks;

        }
        public BookDataModel Map(Book booksDataModel)
        {
            return mapper.Map<BookDataModel>(booksDataModel);

        }
    }
}
