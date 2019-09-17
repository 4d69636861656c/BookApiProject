using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetAllReviewsForABook(int bookId);
        Book GetBookOfAReview(int reviewId);
        bool ReviewExists(int reviewId);

        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}