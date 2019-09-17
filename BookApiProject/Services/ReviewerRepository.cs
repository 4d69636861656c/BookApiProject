using BookApiProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookApiProject.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        //Injection of the BookDbContext into the constructor. 
        private BookDbContext _reviewerContext;

        public ReviewerRepository(BookDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _reviewerContext.Remove(reviewer);
            return Save();
        }

        public ICollection<Review> GetAllReviewsByReviewer(int reviewerId)
        {
            return _reviewerContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
            var reviewerId = _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer.Id).FirstOrDefault();
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _reviewerContext.Reviewers.OrderBy(r => r.LastName).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = _reviewerContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Update(reviewer);
            return Save();
        }
    }
}