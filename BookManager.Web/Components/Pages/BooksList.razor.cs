using BookManager.Shared.Dtos;
using BookManager.Web.Services;
using Microsoft.AspNetCore.Components;

namespace BookManager.Web.Components.Pages
{
    public partial class BooksList
    {
        private IEnumerable<BookDto>? books;
        
        [Inject]
        IBookApiClient BookApiClient { get; set; } = null!;
        private string searchTerm = "";

        protected override async Task OnInitializedAsync()
        {
            books = await BookApiClient.GetAllAsync();
        }
        private IEnumerable<BookDto> FilteredBooks =>
            (books ?? Enumerable.Empty<BookDto>())
            .Where(b =>
                b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || b.ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        async Task BorrowBook(Guid id)
        {
            await BookApiClient.BorrowAsync(id);
            books = await BookApiClient.GetAllAsync();
        }

        async Task ReturnBook(Guid id)
        {
            await BookApiClient.ReturnAsync(id);
            books = await BookApiClient.GetAllAsync();
        }
    }
}
