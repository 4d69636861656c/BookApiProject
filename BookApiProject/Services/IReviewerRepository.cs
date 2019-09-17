using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetAllReviewsByReviewer(int reviewerId);
        Reviewer GetReviewerOfAReview(int reviewId);
        bool ReviewerExists(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();
    }
}