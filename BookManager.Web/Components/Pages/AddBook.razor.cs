using BookManager.Shared.Dtos;
using BookManager.Web.Services;
using Microsoft.AspNetCore.Components;

namespace BookManager.Web.Components.Pages
{

    public partial class AddBook
    {
        [Inject] 
        IBookApiClient BookApiClient { get; set; } = null!;
        [Inject] 
        NavigationManager Navigation { get; set; } = null!;
        private BookCreateDto bookCreateModel = new BookCreateDto();

        private async Task HandleValidSubmit()
        {
            await BookApiClient.AddAsync(bookCreateModel);
            // after successful add, navigate to list page
            Navigation.NavigateTo("/books");
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/books");
        }
    }
}
