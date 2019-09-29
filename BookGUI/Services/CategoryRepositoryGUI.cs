using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class CategoryRepositoryGUI : ICategoryRepositoryGUI
    {
        public IEnumerable<BookDto> GetAllBooksForCategory(int categoryId)
        {
            IEnumerable<BookDto> books = new List<BookDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"categories/{categoryId}/books");
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

        public IEnumerable<CategoryDto> GetAllCategoriesOfABook(int bookId)
        {
            IEnumerable<CategoryDto> categories = new List<CategoryDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"categories/books/{bookId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<CategoryDto>> readTask = result.Content.ReadAsAsync<IList<CategoryDto>>();
                    readTask.Wait();

                    categories = readTask.Result;
                }
            }

            return categories;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            IEnumerable<CategoryDto> categories = new List<CategoryDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("categories");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<CategoryDto>> readTask = result.Content.ReadAsAsync<IList<CategoryDto>>();
                    readTask.Wait();

                    categories = readTask.Result;
                }
            }

            return categories;
        }

        public CategoryDto GetCategoryById(int categoryId)
        {
            CategoryDto category = new CategoryDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"categories/{categoryId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<CategoryDto> readTask = result.Content.ReadAsAsync<CategoryDto>();
                    readTask.Wait();

                    category = readTask.Result;
                }
            }

            return category;
        }
    }
}