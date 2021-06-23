using Microsoft.Extensions.Configuration;
using ReservationProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor
{
    public class BooksLookupService
    {
        private HttpClient _client;
        private IConfiguration _config;

        public BooksLookupService(HttpClient client, IConfiguration config)
        {
          
            _config = config;
            client.BaseAddress = new Uri(_config["booksUrl"]);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "ReservationProcessor");

            _client = client;
        }

        public async Task<(bool exists,Book book)> CheckIfBookExists(string bookId)
        {
            var response = await _client.GetAsync(bookId);
            if(response.IsSuccessStatusCode)
            {
                var bookJson = await response.Content.ReadAsStringAsync();
                var book = JsonSerializer.Deserialize<Book>(bookJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return (true, book);

            } else
            {
                return (false, null);
            }
        }
    }
}
