using AutoMapper;
using BookManager.Core.Models;
using BookManager.Shared.Dtos;
using Google.Protobuf;
using Microsoft.VisualBasic;
using System.Reflection;


namespace BookManager.ApiService.Mappings
{
    public class BookProfile : Profile
    {
        /// <summary>
        /// Automapper mappings between domain objects and DTOs
        /// </summary>
        public BookProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}