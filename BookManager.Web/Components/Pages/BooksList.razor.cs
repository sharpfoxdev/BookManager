using BookManager.Shared.Dtos;
using BookManager.Web.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;

namespace BookManager.Web.Components.Pages
{
    public partial class BooksList
    {
        private IEnumerable<BookDto>? books;
        
        [Inject]
        IBookApiClient BookApiClient { get; set; } = null!;
        private string searchTerm = "";
        private string? errorMessage;
        protected override async Task OnInitializedAsync()
        {
            await LoadBooksAsync();
        }
        private async Task LoadBooksAsync()
        {
            try
            {
                books = await BookApiClient.GetAllAsync();
            }
            catch (HttpRequestException)
            {
                errorMessage = "Cannot reach server. Please try again later.";
                books = Array.Empty<BookDto>();
            }
            catch (Exception)
            {
                errorMessage = "An unexpected error occurred while loading books.";
                books = Array.Empty<BookDto>();
            }
        }
        private IEnumerable<BookDto> FilteredBooks =>
            (books ?? Enumerable.Empty<BookDto>())
            .Where(b =>
                b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || b.ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        private async Task BorrowBook(Guid id)
        {

            try
            {
                await BookApiClient.BorrowAsync(id);
                await LoadBooksAsync();
            }
            catch (ApiException apiEx)
            {
                if (apiEx.StatusCode == 409)
                {
                    errorMessage = "Another user modified this book. Please reload and try again.";
                }
                else if (apiEx.StatusCode == 404)
                {
                    errorMessage = "The book was not found.";
                }
                else if (apiEx.StatusCode == 400)
                {
                    errorMessage = "Invalid request: " + apiEx.ResponseMessage;
                }
                else if (apiEx.StatusCode >= 500)
                {
                    errorMessage = "Server error. Please contact support or try again later.";
                }
                else
                {
                    errorMessage = "Unexpected error occurred while borrowing the book.";
                }

                await LoadBooksAsync();
            }
            catch (HttpRequestException)
            {
                errorMessage = "Cannot reach server. Please try again later.";
                await LoadBooksAsync();
            }
            catch (Exception)
            {
                errorMessage = "An unexpected error occurred. Please try again later.";
                await LoadBooksAsync();
            }
        }

        private async Task ReturnBook(Guid id)
        {

            try
            {
                await BookApiClient.ReturnAsync(id);
                await LoadBooksAsync();
            }
            catch (ApiException apiEx)
            {
                if (apiEx.StatusCode == 409)
                {
                    errorMessage = "Another user modified this book. Please reload and try again.";
                }
                else if (apiEx.StatusCode == 404)
                {
                    errorMessage = "The book was not found.";
                }
                else if (apiEx.StatusCode == 400)
                {
                    errorMessage = "Invalid request: " + apiEx.ResponseMessage;
                }
                else if (apiEx.StatusCode >= 500)
                {
                    errorMessage = "Server error. Please contact support or try again later.";
                }
                else
                {
                    errorMessage = "Unexpected error occurred while returning the book.";
                }

                await LoadBooksAsync();
            }
            catch (HttpRequestException)
            {
                errorMessage = "Cannot reach server. Please try again later.";
                await LoadBooksAsync();
            }
            catch (Exception)
            {
                errorMessage = "An unexpected error occurred. Please try again later.";
                await LoadBooksAsync();
            }
        }
        private void ClearError()
        {
            errorMessage = null;
        }
    }
}
