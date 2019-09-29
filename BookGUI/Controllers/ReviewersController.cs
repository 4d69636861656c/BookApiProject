using BookApiProject.Dtos;
using BookApiProject.Models;
using BookGUI.Services;
using BookGUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BookGUI.Controllers
{
    public class ReviewersController : Controller
    {
        // Injection of IReviewerRepositoryGUI. 
        private IReviewerRepositoryGUI _reviewerRepository;
        private IReviewRepositoryGUI _reviewRepository;

        public ReviewersController(IReviewerRepositoryGUI reviewerRepository, IReviewRepositoryGUI reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            if (reviewers.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving reviewers or no reviewers exist!";
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(reviewers);
        }

        public IActionResult GetReviewerById(int reviewerId)
        {
            var reviewer = _reviewerRepository.GetReviewerById(reviewerId);

            if (reviewer == null)
            {
                ModelState.AddModelError("", "Error getting the reviewer!");
                ViewBag.ReviewerMessage = $"There was a problem retrieving the reviewer with ID {reviewerId} from the database or no reviewer with that ID exists.";
                reviewer = new ReviewerDto();
            }

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (reviews.Count() <= 0)
            {
                ViewBag.ReviewMessage = $"The reviewer {reviewer.FirstName} {reviewer.LastName} has no reviews.";
            }

            IDictionary<ReviewDto, BookDto> reviewAndBook = new Dictionary<ReviewDto, BookDto>();
            foreach (var review in reviews)
            {
                var book = _reviewRepository.GetBookOfAReview(review.Id);
                reviewAndBook.Add(review, book);
            }

            var reviewerReviewsBooksViewModel = new ReviewerReviewsBooksViewModel()
            {
                Reviewer = reviewer,
                ReviewBook = reviewAndBook
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(reviewerReviewsBooksViewModel);
        }

        [HttpGet]
        public IActionResult CreateReviewer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateReviewer(Reviewer reviewer)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PostAsJsonAsync("reviewers", reviewer);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var newReviewerTask = result.Content.ReadAsAsync<Reviewer>();
                    newReviewerTask.Wait();

                    var newReviewer = newReviewerTask.Result;
                    TempData["SuccessMessage"] = $"Reviewer {newReviewer.FirstName} {newReviewer.LastName} was successfully created!";

                    return RedirectToAction("GetReviewerById", new { reviewerId = newReviewer.Id });
                }

                ModelState.AddModelError("", "Some kind of error occurred! The reviewer was not created!");

            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateReviewer(int reviewerId)
        {
            var reviewerToUpdate = _reviewerRepository.GetReviewerById(reviewerId);

            if (reviewerToUpdate == null)
            {
                ModelState.AddModelError("", "Something went wrong while retrieving the reviewer!");
                reviewerToUpdate = new ReviewerDto();
            }

            return View(reviewerToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateReviewer(Reviewer reviewerToUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PutAsJsonAsync($"reviewers/{reviewerToUpdate.Id}", reviewerToUpdate);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Reviewer {reviewerToUpdate.FirstName} {reviewerToUpdate.LastName} was updated successfully!";

                    return RedirectToAction("GetReviewerById", new { reviewerId = reviewerToUpdate.Id });
                }

                ModelState.AddModelError("", "Some kind of error occured! The reviewer was not updated!");
            }

            var reviewerDto = _reviewerRepository.GetReviewerById(reviewerToUpdate.Id);

            return View(reviewerDto);
        }

        [HttpGet]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            var reviewer = _reviewerRepository.GetReviewerById(reviewerId);
            if (reviewer == null)
            {
                ModelState.AddModelError("", $"Something went wrong! The reviewer does not exist!");
                reviewer = new ReviewerDto();
            }

            return View(reviewer);
        }

        [HttpPost]
        public IActionResult DeleteReviewer(int reviewerId, string reviewerFirstName, string reviewerLastName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.DeleteAsync($"reviewers/{reviewerId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Reviewer {reviewerFirstName} {reviewerLastName} was successfully deleted.";

                    return RedirectToAction("Index");
                }

                // No 409 needed, the reviewers can be deleted without any restriction. 
                ModelState.AddModelError("", $"Something went wrong! The reviewer was not deleted!");
            }

            // If the deletetion was not successful, the same view for the reviewer that the user is trying to delete is returned. 
            var reviewerDto = _reviewerRepository.GetReviewerById(reviewerId);

            return View(reviewerDto);
        }
    }
}