using BookManager.Shared.Dtos;
using BookManager.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookManager.Web.Components.Pages
{
    public partial class BookHistory
    {
        [Parameter]
        public Guid BookId { get; set; }
        [Inject]
        IBookApiClient ApiClient { get; set; } = null!;

        private List<LoanHistoryDto>? history;

        protected override async Task OnInitializedAsync()
        {
            history = await ApiClient.GetHistoryAsync(BookId);
        }
    }
}
