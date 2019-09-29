using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface IReviewerRepositoryGUI
    {
        IEnumerable<ReviewerDto> GetReviewers();
        ReviewerDto GetReviewerById(int reviewerId);
        IEnumerable<ReviewDto> GetReviewsByReviewer(int reviewerId);
        ReviewerDto GetReviewerOfAReview(int reviewId);
    }
}