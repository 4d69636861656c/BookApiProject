using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        // Injection of the IReviewRepository into the controller. 
        private IReviewRepository _reviewRepository;
        private IReviewerRepository _reviewerRepository;
        private IBookRepository _bookRepository;

        public ReviewsController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IBookRepository bookRepository)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _bookRepository = bookRepository;
        }

        //api/reviews
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewsDto);
        }

        //api/reviews/reviewId
        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };

            return Ok(reviewDto);
        }

        //api/reviews/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetAllReviewsForABook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var reviews = _reviewRepository.GetAllReviewsForABook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewsDto);
        }

        //api/reviews/reviewId/book
        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var book = _reviewRepository.GetBookOfAReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Title = book.Title,
                DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }

        //api/reviews
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))] // Resource created. 
        [ProducesResponseType(400)] // Bad Request. 
        [ProducesResponseType(404)] // Resource not found. 
        [ProducesResponseType(500)] // Internal server error. 
        public IActionResult CreateCategory([FromBody]Review reviewToCreate)
        {
            if (reviewToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewToCreate.Reviewer.Id))
            {
                ModelState.AddModelError("", "Reviewer doesn't exist!");
            }

            if (!_bookRepository.BookExists(reviewToCreate.Book.Id))
            {
                ModelState.AddModelError("", "Book doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }

            reviewToCreate.Book = _bookRepository.GetBook(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { reviewId = reviewToCreate.Id }, reviewToCreate);
        }

        //api/reviews/reviewId
        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)] // Bad Request. 
        [ProducesResponseType(404)] // Resource not found. 
        [ProducesResponseType(500)] // Internal server error. 
        public IActionResult UpdateReview(int reviewId, [FromBody]Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewId != reviewToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewId))
            {
                ModelState.AddModelError("", "Review doesn't exist!");
            }

            if (!_reviewerRepository.ReviewerExists(reviewToUpdate.Reviewer.Id))
            {
                ModelState.AddModelError("", "Reviewer doesn't exist!");
            }

            if (!_bookRepository.BookExists(reviewToUpdate.Book.Id))
            {
                ModelState.AddModelError("", "Book doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }

            reviewToUpdate.Book = _bookRepository.GetBook(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating the review!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/reviews/reviewId
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the review!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}