using AutoMapper;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers.CreateDataMappers
{
   public class CreateBookMapper
    {
        IMapper mapper;
        public CreateBookMapper()
        {
            mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateBookDataModel, Book>().ReverseMap();
            }).CreateMapper();
        }

        public IEnumerable<Book> Map(IEnumerable<CreateBookDataModel> booksDataModel)
        {
            List<Book> listOfMappedBooks = new List<Book>();
            foreach (var book in booksDataModel)
            {
                listOfMappedBooks.Add(mapper.Map<Book>(book));
            }
            return listOfMappedBooks;
        
        } 
        public Book Map(CreateBookDataModel booksDataModel)
        {
            return mapper.Map<Book>(booksDataModel);
        
        }
    }
}
