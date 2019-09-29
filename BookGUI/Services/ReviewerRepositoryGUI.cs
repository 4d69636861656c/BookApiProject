using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class ReviewerRepositoryGUI : IReviewerRepositoryGUI
    {
        public ReviewerDto GetReviewerById(int reviewerId)
        {
            ReviewerDto reviewer = new ReviewerDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var response = client.GetAsync($"reviewers/{reviewerId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ReviewerDto>();
                    readTask.Wait();

                    reviewer = readTask.Result;
                }
            }

            return reviewer;
        }

        public ReviewerDto GetReviewerOfAReview(int reviewId)
        {
            ReviewerDto reviewer = new ReviewerDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"reviewers/{reviewId}/reviewer");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<ReviewerDto> readTask = result.Content.ReadAsAsync<ReviewerDto>();
                    readTask.Wait();

                    reviewer = readTask.Result;
                }
            }

            return reviewer;
        }

        public IEnumerable<ReviewerDto> GetReviewers()
        {
            IEnumerable<ReviewerDto> reviewers = new List<ReviewerDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("reviewers");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<ReviewerDto>> readTask = result.Content.ReadAsAsync<IList<ReviewerDto>>();
                    readTask.Wait();

                    reviewers = readTask.Result;
                }
            }

            return reviewers;
        }

        public IEnumerable<ReviewDto> GetReviewsByReviewer(int reviewerId)
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"reviewers/{reviewerId}/reviews");
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