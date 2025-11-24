using BookManager.Shared.Dtos;

namespace BookManager.Web.Services
{
    public interface IBookApiClient
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<IEnumerable<BookDto>> SearchAsync(string term);
        Task AddAsync(BookCreateDto book);
        Task BorrowAsync(Guid id);
        Task ReturnAsync(Guid id);
    }
}
