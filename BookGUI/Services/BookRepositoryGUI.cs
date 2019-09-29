using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class BookRepositoryGUI : IBookRepositoryGUI
    {
        public BookDto GetBookById(int bookId)
        {
            BookDto book = new BookDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"books/{bookId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<BookDto> readTask = result.Content.ReadAsAsync<BookDto>();
                    readTask.Wait();

                    book = readTask.Result;
                }
            }

            return book;
        }

        public BookDto GetBookByIsbn(string bookIsbn)
        {
            BookDto book = new BookDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"books/ISBN/{bookIsbn}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<BookDto> readTask = result.Content.ReadAsAsync<BookDto>();
                    readTask.Wait();

                    book = readTask.Result;
                }
            }

            return book;
        }

        public decimal GetBookRating(int bookId)
        {
            decimal rating = default;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"books/{bookId}/rating");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<decimal> readTask = result.Content.ReadAsAsync<decimal>();
                    readTask.Wait();

                    rating = readTask.Result;
                }
            }

            return rating;
        }

        public IEnumerable<BookDto> GetBooks()
        {
            IEnumerable<BookDto> books = new List<BookDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("books");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<BookDto>> readTask = result.Content.ReadAsAsync<IList<BookDto>>();
                    readTask.Wait();

                    books = readTask.Result;
                }
            }

            return books;
        }
    }
}