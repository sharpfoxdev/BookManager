using BookManager.Shared.Dtos;

namespace BookManager.Web.Services
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? ResponseMessage { get; }

        public ApiException(int statusCode, string? responseMessage = null)
            : base($"API call failed with status {statusCode}")
        {
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
        }
    }
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
            //return await _http.GetFromJsonAsync<IEnumerable<BookDto>>("api/books")!;
            var response = await _http.GetAsync("api/books");
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>()!
                   ?? Enumerable.Empty<BookDto>();
        }

        public async Task<IEnumerable<BookDto>> SearchAsync(string term)
        {
            var response = await _http.GetAsync($"api/books/search?term={Uri.EscapeDataString(term)}");
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>()!
                   ?? Enumerable.Empty<BookDto>();
        }

        public async Task AddAsync(BookCreateDto book)
        {
            var response = await _http.PostAsJsonAsync("api/books", book);
            await EnsureSuccess(response);
        }

        public async Task BorrowAsync(Guid id)
        {
            var response = await _http.PostAsync($"api/books/{id}/borrow", null);
            await EnsureSuccess(response);
        }

        public async Task ReturnAsync(Guid id)
        {
            var response = await _http.PostAsync($"api/books/{id}/return", null);
            await EnsureSuccess(response);
        }

        public async Task<List<LoanHistoryDto>> GetHistoryAsync(Guid bookId)
        {
            var response = await _http.GetAsync($"api/books/{bookId}/history");
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<List<LoanHistoryDto>>()!
                   ?? new List<LoanHistoryDto>();
        }
        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string? message = null;
            try
            {
                message = await response.Content.ReadAsStringAsync();
            }
            catch { /* ignore content read failure */ }

            throw new ApiException((int)response.StatusCode, message);
        }
    }
}
