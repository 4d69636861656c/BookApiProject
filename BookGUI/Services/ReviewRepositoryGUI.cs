using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class ReviewRepositoryGUI : IReviewRepositoryGUI
    {
        public BookDto GetBookOfAReview(int reviewId)
        {
            BookDto book = new BookDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"reviews/{reviewId}/book");
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

        public ReviewDto GetReviewById(int reviewId)
        {
            ReviewDto review = new ReviewDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"reviews/{reviewId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<ReviewDto> readTask = result.Content.ReadAsAsync<ReviewDto>();
                    readTask.Wait();

                    review = readTask.Result;
                }
            }

            return review;
        }

        public IEnumerable<ReviewDto> GetReviews()
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("reviews");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<ReviewDto>> readTask = result.Content.ReadAsAsync<IList<ReviewDto>>();
                    readTask.Wait();

                    reviews = readTask.Result;
                }
            }

            return reviews;
        }

        public IEnumerable<ReviewDto> GetReviewsOfABook(int bookId)
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"reviews/books/{bookId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<ReviewDto>> readTask = result.Content.ReadAsAsync<IList<ReviewDto>>();
                    readTask.Wait();

                    reviews = readTask.Result;
                }
            }

            return reviews;
        }
    }
}
