using BookManager.Shared.Dtos;

namespace BookManager.Web.Services
{
    public class BookApiClient : IBookApiClient
    {
        private readonly HttpClient _http;

        public BookApiClient(HttpClient http)
        {
            _http = http;
            Console.WriteLine($"HttpClient BaseAddress = {_http.BaseAddress}");
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            //return await _http.GetFromJsonAsync<IEnumerable<BookDto>>(new Uri("https://localhost:7318/api/books"))!;
            return await _http.GetFromJsonAsync<IEnumerable<BookDto>>("api/books")!;
        }

        public async Task<IEnumerable<BookDto>> SearchAsync(string term)
        {
            return await _http.GetFromJsonAsync<IEnumerable<BookDto>>($"api/books/search?term={Uri.EscapeDataString(term)}")!;
        }

        public async Task AddAsync(BookCreateDto book)
        {
            await _http.PostAsJsonAsync("api/books", book);
        }

        public async Task BorrowAsync(Guid id)
        {
            await _http.PostAsync($"api/books/{id}/borrow", null);
        }

        public async Task ReturnAsync(Guid id)
        {
            await _http.PostAsync($"api/books/{id}/return", null);
        }
    }
}
