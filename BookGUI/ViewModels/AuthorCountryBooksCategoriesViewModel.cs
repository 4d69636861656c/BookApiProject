using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.ViewModels
{
    public class AuthorCountryBooksCategoriesViewModel
    {
        public AuthorDto Author { get; set; }
        public CountryDto Country { get; set; }
        public IDictionary<BookDto, IEnumerable<CategoryDto>> BookCategories { get; set; }
    }
}