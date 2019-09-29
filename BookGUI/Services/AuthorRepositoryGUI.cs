using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class AuthorRepositoryGUI : IAuthorRepositoryGUI
    {
        public AuthorDto GetAuthorById(int authorId)
        {
            AuthorDto author = new AuthorDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"authors/{authorId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<AuthorDto> readTask = result.Content.ReadAsAsync<AuthorDto>();
                    readTask.Wait();

                    author = readTask.Result;
                }
            }

            return author;
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            IEnumerable<AuthorDto> authors = new List<AuthorDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("authors");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<AuthorDto>> readTask = result.Content.ReadAsAsync<IList<AuthorDto>>();
                    readTask.Wait();

                    authors = readTask.Result;
                }
            }

            return authors;
        }

        public IEnumerable<AuthorDto> GetAuthorsOfABook(int bookId)
        {
            IEnumerable<AuthorDto> authors = new List<AuthorDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"authors/books/{bookId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<AuthorDto>> readTask = result.Content.ReadAsAsync<IList<AuthorDto>>();
                    readTask.Wait();

                    authors = readTask.Result;
                }
            }

            return authors;
        }

        public IEnumerable<BookDto> GetBooksByAuthor(int authorId)
        {
            IEnumerable<BookDto> books = new List<BookDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"authors/{authorId}/books");
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