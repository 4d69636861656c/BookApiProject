using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.ViewModels
{
    public class CompleteBookViewModel
    {
        public BookDto Book { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IDictionary<AuthorDto, CountryDto> AuthorsCountries { get; set; }
        public IDictionary<ReviewDto, ReviewerDto> ReviewsReviewers { get; set; }
        public decimal Rating { get; set; }
    }
}