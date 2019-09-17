using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : Controller
    {
        // Injection of the IReviewerRepository into the constructor of the class. 
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;

        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/reviewers
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            // ModelState validation. 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewersDto = new List<ReviewerDto>();

            foreach (var reviewer in reviewers)
            {
                reviewersDto.Add(new ReviewerDto
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                });
            }

            return Ok(reviewersDto);
        }

        //api/reviewers/reviewerId
        [HttpGet("{reviewerId}", Name = "GetReviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }

        //api/reviewers/reviewerId/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetAllReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviews = _reviewerRepository.GetAllReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewsDto);
        }

        //api/reviewers/reviewId/reviewer
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            // Validation of the review object. 
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }

        //api/reviewers/
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))] // Resource created. 
        [ProducesResponseType(400)] // Bad request. 
        [ProducesResponseType(500)] // Internal Server Error.
        public IActionResult CreateReviewer([FromBody]Reviewer reviewerToCreate)
        {
            if (reviewerToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.CreateReviewer(reviewerToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating the reviewer {reviewerToCreate.FirstName} {reviewerToCreate.LastName}!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewerToCreate.Id }, reviewerToCreate);
        }

        //api/reviewers/reviewerId
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)] // Bad Request. 
        [ProducesResponseType(404)] // No resource found. 
        [ProducesResponseType(500)] // Internal Server Error. 
        public IActionResult UpdateReviewer(int reviewerId, [FromBody]Reviewer updatedReviewerInfo)
        {
            if (updatedReviewerInfo == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewerId != updatedReviewerInfo.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.UpdateReviewer(updatedReviewerInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating the reviewer {updatedReviewerInfo.FirstName} {updatedReviewerInfo.LastName}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/reviewers/reviewerId
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)] // Bad request. 
        [ProducesResponseType(404)] // Not found. 
        [ProducesResponseType(500)] // Internal Server Error. 
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            var reviewsToDelete = _reviewerRepository.GetAllReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the reviewer {reviewerToDelete.FirstName} {reviewerToDelete.LastName}!");
                return StatusCode(500, ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the reviews by reviewer {reviewerToDelete.FirstName} {reviewerToDelete.LastName}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}