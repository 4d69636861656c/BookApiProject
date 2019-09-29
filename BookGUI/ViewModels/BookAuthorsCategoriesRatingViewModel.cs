using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.ViewModels
{
    public class BookAuthorsCategoriesRatingViewModel
    {
        public BookDto Book { get; set; }
        public IEnumerable<AuthorDto> Authors { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public decimal Rating { get; set; }
    }
}