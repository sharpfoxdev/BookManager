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
        
        protected override async Task OnInitializedAsync()
        {
            books = await BookApiClient.GetAllAsync();
        }

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
