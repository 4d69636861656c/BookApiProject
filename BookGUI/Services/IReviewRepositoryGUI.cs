using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface IReviewRepositoryGUI
    {
        IEnumerable<ReviewDto> GetReviews();
        ReviewDto GetReviewById(int reviewId);
        IEnumerable<ReviewDto> GetReviewsOfABook(int bookId);
        BookDto GetBookOfAReview(int reviewId);
    }
}