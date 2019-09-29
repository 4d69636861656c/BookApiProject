using BookApiProject.Dtos;

namespace BookGUI.ViewModels
{
    public class ReviewReviewerBookViewModel
    {
        public ReviewDto Review { get; set; }
        public ReviewerDto Reviewer { get; set; }
        public BookDto Book { get; set; }
    }
}